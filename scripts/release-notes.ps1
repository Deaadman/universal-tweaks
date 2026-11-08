# Turns a version's section of CHANGELOG.md into release notes:
#
#   release-notes.md   the GitHub release's description: the section as written
#   whats-new.json     {notes, ...} for manifest.json, which the app shows
#                      as "what's new" on the first launch after an update
#
#   ./scripts/release-notes.ps1 -Version 0.2.0 -OutDir notes
#   ./scripts/release-notes.ps1 -Version 0.2.0 -OutDir notes -Extra extra.json
#   ./scripts/release-notes.ps1 -Check        # only check CHANGELOG.md's format
#
# -Extra names a JSON file whose keys are added to whats-new.json beside
# `notes`, for anything else an update should carry.
#
# CHANGELOG.md follows Keep a Changelog 2.0.0 (https://keepachangelog.com/en/2.0.0/),
# with versions and tags that have no v: "## [0.2.0] - 2026-10-21", entries
# under the six types, and a link for every version at the bottom.
#
# From Abalone (abalone::release::NOTES_SCRIPT): change it there, not here.
param(
    [string]$Version,
    [string]$OutDir = '.',
    [switch]$Check,
    [string]$Changelog = (Join-Path (Split-Path -Parent $PSScriptRoot) 'CHANGELOG.md'),
    # https://github.com/owner/repo; the origin remote's when left out.
    [string]$Repo,
    [string]$Extra
)

# Read before errors stop the script: git's complaint about a missing remote isn't one.
if (-not $Repo -and (Test-Path $Changelog)) {
    $origin = git -C (Split-Path -Parent (Resolve-Path $Changelog)) remote get-url origin 2>$null
    if ($origin) { $Repo = "$origin".Trim() -replace '^git@github\.com:', 'https://github.com/' -replace '\.git$', '' }
}

$ErrorActionPreference = 'Stop'

if (-not $Repo) { throw 'Pass -Repo https://github.com/owner/repo (there is no origin remote to read it from)' }
$Repo = $Repo.TrimEnd('/')
$Types = @('Added', 'Changed', 'Deprecated', 'Removed', 'Fixed', 'Security')

function Fail([string]$Why) { throw "CHANGELOG.md: $Why" }

# -1, 0 or 1 as SemVer orders two versions: 1.1.0-beta.2 < 1.1.0-rc.1 < 1.1.0.
# Build metadata (+...) doesn't count.
function Compare-Version([string]$A, [string]$B) {
    $a1, $aPre = ($A -replace '\+.*$', '') -split '-', 2
    $b1, $bPre = ($B -replace '\+.*$', '') -split '-', 2
    $c = ([version]$a1).CompareTo([version]$b1)
    if ($c -ne 0) { return [Math]::Sign($c) }
    # A pre-release comes before its version.
    if (-not $aPre -and -not $bPre) { return 0 }
    if (-not $aPre) { return 1 }
    if (-not $bPre) { return -1 }
    $x = $aPre -split '\.'
    $y = $bPre -split '\.'
    for ($i = 0; $i -lt [Math]::Min($x.Count, $y.Count); $i++) {
        $xNum = $x[$i] -match '^\d+$'
        $yNum = $y[$i] -match '^\d+$'
        if ($xNum -and $yNum) {
            # Compared as text by length then digits, so any length works.
            $p = $x[$i].TrimStart('0'); $q = $y[$i].TrimStart('0')
            $c = $p.Length.CompareTo($q.Length)
            if ($c -eq 0) { $c = [string]::CompareOrdinal($p, $q) }
        } elseif ($xNum) { $c = -1 }
        elseif ($yNum) { $c = 1 }
        else { $c = [string]::CompareOrdinal($x[$i], $y[$i]) }
        if ($c -ne 0) { return [Math]::Sign($c) }
    }
    return [Math]::Sign($x.Count.CompareTo($y.Count))
}

# The changelog as sections, newest first: @{ Name; Date; Yanked; Text; Notes }, and its links.
function Read-Changelog([string]$Path) {
    $lines = [IO.File]::ReadAllLines((Resolve-Path $Path).Path)
    if ($lines.Count -eq 0 -or $lines[0] -ne '# Changelog') { Fail "the first line should be '# Changelog'" }
    $sections = New-Object Collections.Generic.List[object]
    $links = @{}
    $current = $null
    $type = $null
    $inComment = $false
    foreach ($line in $lines) {
        # HTML comments (the notes for whoever edits the file) aren't part of any section.
        if ($inComment) { if ($line -match '-->') { $inComment = $false }; continue }
        if ($line -match '^\s*<!--') { if ($line -notmatch '-->') { $inComment = $true }; continue }

        # The link definitions at the bottom: [0.2.0]: https://...
        if ($line -match '^\[([^\]]+)\]:\s*(\S+)\s*$') {
            $label = $Matches[1].ToLowerInvariant()
            if ($links.ContainsKey($label)) { Fail "the link for [$($Matches[1])] is defined twice" }
            $links[$label] = $Matches[2]
            $current = $null
            continue
        }
        if ($line -match '^## ') {
            if ($line -match '^## \[Unreleased\]\s*$') {
                $current = @{ Name = 'Unreleased'; Date = $null; Yanked = $false }
            } elseif ($line -match '^## \[(\d+\.\d+\.\d+(?:[-+][0-9A-Za-z.-]+)?)\] - (\d{4}-\d{2}-\d{2})( \[YANKED\])?\s*$') {
                $date = [datetime]::MinValue
                if (-not [datetime]::TryParseExact($Matches[2], 'yyyy-MM-dd', [Globalization.CultureInfo]::InvariantCulture, 'None', [ref]$date)) {
                    Fail "'$line' has a date that doesn't exist"
                }
                $current = @{ Name = $Matches[1]; Date = $Matches[2]; Yanked = [bool]$Matches[3] }
            } else {
                Fail "'$line' should be '## [Unreleased]' or '## [0.2.0] - YYYY-MM-DD' (no v)"
            }
            if (@($sections | Where-Object { $_.Name -eq $current.Name }).Count) { Fail "'## [$($current.Name)]' appears twice" }
            $current.Text = New-Object Collections.Generic.List[string]
            $current.Notes = New-Object Collections.Generic.List[string]
            $current.Types = @{}
            $sections.Add($current)
            $type = $null
            continue
        }
        if ($null -eq $current) {
            if ($sections.Count -gt 0 -and $line.Trim() -ne '') { Fail "only link definitions can follow the links at the bottom, but found: $line" }
            continue
        }
        $current.Text.Add($line)
        if ($line -match '^### (.+?)\s*$') {
            $type = $Matches[1]
            if ($Types -notcontains $type) { Fail "'### $type' in [$($current.Name)] isn't one of $($Types -join ', ')" }
            if ($current.Types.ContainsKey($type)) { Fail "[$($current.Name)] has '### $type' twice" }
            $current.Types[$type] = 0
            continue
        }
        if ($null -eq $type) { continue } # A short summary before the first type is allowed.
        if ($line -match '^[-*] (.+)$') {
            $current.Notes.Add($Matches[1].Trim())
            $current.Types[$type]++
        } elseif ($line -match '^\s{2,}(\S.*)$' -and $current.Types[$type] -gt 0) {
            # An entry wrapped onto the next line.
            $current.Notes[$current.Notes.Count - 1] = $current.Notes[$current.Notes.Count - 1] + ' ' + $Matches[1].Trim()
        } elseif ($line.Trim() -ne '') {
            Fail "under '### $type' in [$($current.Name)], every line should be a '- ' entry, but found: $line"
        }
    }

    # Unreleased first, then versions newest first; every type has entries,
    # and every version does too (Unreleased may be empty right after a release).
    $versions = @($sections | Where-Object { $_.Name -ne 'Unreleased' })
    if ($sections.Count -eq 0 -or $sections[0].Name -ne 'Unreleased') { Fail "the first section should be '## [Unreleased]'" }
    for ($i = 1; $i -lt $versions.Count; $i++) {
        if ((Compare-Version $versions[$i].Name $versions[$i - 1].Name) -ge 0) { Fail "[$($versions[$i].Name)] should come after [$($versions[$i - 1].Name)]: newest first" }
    }
    foreach ($s in $sections) {
        foreach ($t in $s.Types.Keys) {
            if ($s.Types[$t] -eq 0) { Fail "'### $t' in [$($s.Name)] has no entries; remove it" }
        }
        if ($s.Name -ne 'Unreleased' -and $s.Notes.Count -eq 0) { Fail "[$($s.Name)] has no entries" }
    }

    # Every heading's link, comparing with the version before (tags have no v).
    $expected = [ordered]@{}
    $expected['unreleased'] = if ($versions.Count) { "$Repo/compare/$($versions[0].Name)...HEAD" } else { "$Repo/commits/main" }
    for ($i = 0; $i -lt $versions.Count; $i++) {
        $v = $versions[$i].Name
        $expected[$v.ToLowerInvariant()] = if ($i + 1 -lt $versions.Count) { "$Repo/compare/$($versions[$i + 1].Name)...$v" } else { "$Repo/releases/tag/$v" }
    }
    foreach ($label in $expected.Keys) {
        $name = if ($label -eq 'unreleased') { 'Unreleased' } else { $label }
        if (-not $links.ContainsKey($label)) { Fail "add this link at the bottom: [$name]: $($expected[$label])" }
        if ($links[$label] -ne $expected[$label]) { Fail "the link for [$name] should be $($expected[$label])" }
    }
    foreach ($label in $links.Keys) {
        if (-not $expected.Contains($label)) { Fail "the link [$label] has no section" }
    }
    # The comma keeps a one-section list a list: PowerShell unrolls what a function returns.
    return , $sections
}

# One entry as plain text for the app: no Markdown links, code marks or bold.
function ConvertTo-PlainNote([string]$Note) {
    $n = $Note -replace '\[([^\]]+)\]\([^)]+\)', '$1'
    $n = $n -replace '\*\*|__', ''
    $n = $n -replace '`', ''
    return $n.Trim()
}

$sections = Read-Changelog $Changelog
if ($Check) {
    Write-Host "CHANGELOG.md is fine: $($sections.Count) section(s)"
    exit 0
}
if (-not $Version) { throw 'Pass -Version, for example -Version 0.2.0' }
$Version = $Version.TrimStart('v')
$section = $sections | Where-Object { $_.Name -eq $Version } | Select-Object -First 1
if (-not $section) {
    throw "CHANGELOG.md has no '## [$Version]' section. Rename '## [Unreleased]' to '## [$Version] - $(Get-Date -Format yyyy-MM-dd)' and update its link."
}
if ($section.Yanked) { throw "[$Version] is marked [YANKED]" }

New-Item -ItemType Directory -Force $OutDir | Out-Null
# .NET resolves relative paths against the process's folder, not PowerShell's.
$OutDir = (Resolve-Path $OutDir).Path
$body = ($section.Text -join "`n").Trim() + "`n"
[IO.File]::WriteAllText((Join-Path $OutDir 'release-notes.md'), $body)

$whatsNew = [ordered]@{ notes = @($section.Notes | ForEach-Object { ConvertTo-PlainNote $_ }) }
if ($Extra) {
    $more = Get-Content -Raw $Extra | ConvertFrom-Json
    foreach ($p in $more.PSObject.Properties) {
        if ($p.Name -eq 'notes') { throw "$Extra can't set notes; they come from CHANGELOG.md" }
        $whatsNew[$p.Name] = $p.Value
    }
}
[IO.File]::WriteAllText((Join-Path $OutDir 'whats-new.json'), (ConvertTo-Json -InputObject $whatsNew -Depth 6))
Write-Host "Release notes for $Version written to $OutDir"
