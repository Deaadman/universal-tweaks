// Renders assets/banner.svg with assets/logo-animated.svg in its strip and a fading title, as PNG frames.
// Usage: node render.mjs <banner.svg> <logo.svg> <outDir> [--video clip.mp4 --start 22.3 --focus 75% --ease 2.4] [--title-at 0.35] [--fps 25] [--seconds 3.2]
// --video plays a clip behind the dark fade in place of the still background, from --start seconds,
// cropped to the banner around --focus (a CSS object-position y). Over the last --ease seconds it slows
// smoothly to a stop, blending neighbouring clip frames so the slow part doesn't stutter.
// It renders in Edge, which can decode H.264.
// Then encode.py turns the frames into assets/images/banner-animated.webp. See docs/development.md.
import { chromium } from 'playwright';
import fs from 'node:fs';
import path from 'node:path';
import { pathToFileURL } from 'node:url';
import { parseArgs } from 'node:util';

const { positionals, values } = parseArgs({
  allowPositionals: true,
  options: {
    video: { type: 'string' },
    start: { type: 'string', default: '0' },
    focus: { type: 'string', default: '50%' },
    ease: { type: 'string', default: '2.4' },
    'video-fps': { type: 'string', default: '29.97' },
    'title-at': { type: 'string' },
    fps: { type: 'string', default: '25' },
    seconds: { type: 'string' },
  },
});
const [bannerPath, logoPath, outDir] = positionals;
const fps = Number(values.fps), start = Number(values.start), videoFps = Number(values['video-fps']);
const ease = values.video ? Number(values.ease) : 0;
// With a video, the default length plays 3.2 seconds of clip whatever the slow-down's length, since
// the slow-down plays half its time's worth (see clipTime).
const seconds = Number(values.seconds ?? (values.video ? 3.2 + ease / 2 : 3.2));

// Seconds of clip played by time t: full speed, then over the last `ease` seconds the speed falls
// along 1 - smoothstep to zero, which plays half that time's worth of clip and settles without a jolt.
function clipTime(t) {
  const easeFrom = seconds - ease;
  if (t <= easeFrom) return t;
  const u = Math.min((t - easeFrom) / ease, 1);
  return easeFrom + ease * (u - (u ** 3 - u ** 4 / 2));
}
fs.mkdirSync(outDir, { recursive: true });

let banner = fs.readFileSync(bannerPath, 'utf8');
const logo = fs.readFileSync(logoPath, 'utf8');

// The logo's <style> and drawing group, nested into the visible strip in place of its raster logo.
const logoStyle = logo.match(/<style>([\s\S]*?)<\/style>/)[1]
  .replace(/@media \(prefers-reduced-motion[\s\S]*$/, '')
  // At 64px wide the original 3-unit stroke is under half a pixel; thicken it so the draw shows.
  .replace('stroke-width:3', 'stroke-width:9');
const logoBody = logo.slice(logo.indexOf('<g class="all">'), logo.lastIndexOf('</svg>'));

// The strip that shows is the last strip layer that isn't hidden. Its logo is drawn in its text colour.
const strips = [...banner.matchAll(/<g id="strip-[^"]*"[^>]*>/g)].filter((m) => !m[0].includes('display:none'));
const strip = strips[strips.length - 1].index;
const stripColour = banner.slice(strip).match(/<text[^>]* fill="([^"]+)"/)[1];
const nested = `<svg x="2293" y="44" width="64" height="85" viewBox="-6 -6 412.3 550" overflow="visible" color="${stripColour}">${logoBody}</svg>`;

const imgStart = banner.indexOf('<image', strip);
const imgEnd = banner.indexOf('/>', imgStart) + 2;
banner = banner.slice(0, imgStart) + nested + banner.slice(imgEnd);

// The title fades in at --title-at seconds. With a video it defaults to the end, so the clip is seen first.
const titleAt = Number(values['title-at'] ?? (values.video ? seconds - 1.4 : 0.35));
const titleStyle = `
#title text{opacity:0;animation:aa-in .9s cubic-bezier(.22,.61,.36,1) both}
#title text:nth-of-type(1){animation-delay:${titleAt}s}
#title text:nth-of-type(2){animation-delay:${titleAt + 0.25}s}
@keyframes aa-in{from{opacity:0;transform:translateY(14px)}to{opacity:1;transform:none}}`;
// With a video, the still background is hidden and the video shows through the SVG under the dark fade.
const videoStyle = values.video ? '#background{display:none}' : '';
banner = banner.replace('</style>', `</style><style>${logoStyle}${titleStyle}${videoStyle}</style>`);

// Two copies of the clip: one on the clip frame before each moment and one on the frame after,
// faded in by how far between them the moment falls.
const videoTag = (id) => `<video id="${id}" muted preload="auto" src="${pathToFileURL(path.resolve(values.video)).href}"
      style="position:absolute;left:0;top:0;width:2250px;height:480px;object-fit:cover;object-position:50% ${values.focus}"></video>`;
const video = values.video ? videoTag('bg') + videoTag('bg-next') : '';
const html = `<!doctype html><html><head><style>html,body{margin:0;background:#000}svg{position:absolute;left:0;top:0}</style></head>
<body>${video}${banner}</body></html>`;
const htmlPath = path.resolve(outDir, 'banner.html');
fs.writeFileSync(htmlPath, html);

const browser = await chromium.launch(values.video ? { channel: 'msedge' } : {});
const page = await browser.newPage({ viewport: { width: 2400, height: 480 } });
await page.goto(pathToFileURL(htmlPath).href);
await page.evaluate(() => document.fonts.ready);
const inter = await page.evaluate(() => document.fonts.check('600 132px Inter'));
console.log('Inter loaded:', inter);
if (values.video) {
  await page.evaluate(() => Promise.all(['bg', 'bg-next'].map((id) => new Promise((resolve, reject) => {
    const v = document.getElementById(id);
    if (v.readyState >= 1) resolve();
    v.onloadedmetadata = resolve;
    v.onerror = () => reject(new Error('video failed to load'));
  }))));
}

const frames = Math.round(seconds * fps) + 1;
for (let i = 0; i < frames; i++) {
  const t = i / fps;
  // Clip frame numbers either side of this moment; seeking to a frame's middle lands on it reliably.
  const clipFrame = (start + clipTime(t)) * videoFps;
  const before = Math.floor(clipFrame);
  await page.evaluate(async ({ t, before, blend, videoFps }) => {
    for (const a of document.getAnimations()) { a.pause(); a.currentTime = t * 1000; }
    const seek = (id, frame) => new Promise((resolve) => {
      const v = document.getElementById(id);
      if (!v) return resolve();
      v.onseeked = resolve;
      v.currentTime = (frame + 0.5) / videoFps;
    });
    await Promise.all([seek('bg', before), seek('bg-next', before + 1)]);
    const next = document.getElementById('bg-next');
    if (next) next.style.opacity = blend;
  }, { t, before, blend: clipFrame - before, videoFps });
  await page.screenshot({ path: path.join(outDir, `f${String(i).padStart(3, '0')}.png`), clip: { x: 0, y: 0, width: 2400, height: 480 } });
}
console.log('frames:', frames);
await browser.close();
