namespace UniversalTweaks.Utilities;

/// <summary>A cache that can drop the entries of destroyed objects.</summary>
internal interface IPrunable
{
    /// <summary>Removes the entries whose objects have been destroyed.</summary>
    void Prune();
}

/// <summary>Every <see cref="ObjectStateCache{TObject, TState}"/>, so they can all be pruned together.</summary>
/// <remarks>
/// Instance IDs are never reused, so without pruning a cache would grow forever. <see cref="Mod"/> prunes them all when
/// a scene loads.
/// </remarks>
internal static class ObjectStateCaches
{
    /// <summary>The caches made so far.</summary>
    private static readonly List<IPrunable> Caches = [];

    /// <summary>Adds a cache to the ones <see cref="PruneAll"/> prunes.</summary>
    /// <param name="cache">The cache.</param>
    internal static void Register(IPrunable cache) => Caches.Add(cache);

    /// <summary>Prunes every cache of destroyed objects.</summary>
    internal static void PruneAll()
    {
        foreach (var cache in Caches)
            cache.Prune();
    }
}

/// <summary>
/// State kept for each Unity object, by its instance ID. Each entry holds the object too, so destroyed ones can be
/// found and pruned.
/// </summary>
/// <typeparam name="TObject">The type of object.</typeparam>
/// <typeparam name="TState">What's kept for each object.</typeparam>
internal sealed class ObjectStateCache<TObject, TState> : IPrunable where TObject : UnityEngine.Object
{
    /// <summary>The entries, by the object's instance ID.</summary>
    private readonly Dictionary<int, (TObject Owner, TState State)> m_Entries = new();

    /// <summary>Makes an empty cache and registers it with <see cref="ObjectStateCaches"/>.</summary>
    internal ObjectStateCache() => ObjectStateCaches.Register(this);

    /// <summary>The objects with an entry, including any destroyed since the last prune.</summary>
    internal IEnumerable<TObject> Owners => m_Entries.Values.Select(entry => entry.Owner);

    /// <summary>Whether an object has an entry.</summary>
    /// <param name="owner">The object.</param>
    /// <returns><see langword="true"/> if it has one.</returns>
    internal bool Contains(TObject owner) => m_Entries.ContainsKey(owner.GetInstanceID());

    /// <summary>Gets an object's state.</summary>
    /// <param name="owner">The object.</param>
    /// <param name="state">Its state, if it has an entry.</param>
    /// <returns><see langword="true"/> if it has an entry.</returns>
    internal bool TryGet(TObject owner, [MaybeNullWhen(false)] out TState state)
    {
        if (m_Entries.TryGetValue(owner.GetInstanceID(), out var entry))
        {
            state = entry.State;
            return true;
        }

        state = default;
        return false;
    }

    /// <summary>Sets an object's state, replacing any it had.</summary>
    /// <param name="owner">The object.</param>
    /// <param name="state">Its state.</param>
    internal void Set(TObject owner, TState state) => m_Entries[owner.GetInstanceID()] = (owner, state);

    /// <summary>Sets an object's state, unless it already has one.</summary>
    /// <param name="owner">The object.</param>
    /// <param name="state">Its state.</param>
    /// <returns><see langword="true"/> if it was added.</returns>
    internal bool TryAdd(TObject owner, TState state) => m_Entries.TryAdd(owner.GetInstanceID(), (owner, state));

    /// <summary>Removes an object's entry.</summary>
    /// <param name="owner">The object.</param>
    /// <param name="state">The state it had, if it had an entry.</param>
    /// <returns><see langword="true"/> if it had an entry.</returns>
    internal bool Remove(TObject owner, [MaybeNullWhen(false)] out TState state)
    {
        if (m_Entries.Remove(owner.GetInstanceID(), out var entry))
        {
            state = entry.State;
            return true;
        }

        state = default;
        return false;
    }

    /// <inheritdoc />
    public void Prune()
    {
        foreach (var (id, entry) in m_Entries.ToArray())
        {
            if (entry.Owner == null)
                m_Entries.Remove(id);
        }
    }
}
