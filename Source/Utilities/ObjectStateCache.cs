namespace UniversalTweaks.Utilities;

internal interface IPrunable
{
    void Prune();
}

// Instance IDs are never reused, so every cache is pruned of destroyed objects on scene load to stop it growing forever.
internal static class ObjectStateCaches
{
    private static readonly List<IPrunable> Caches = [];

    internal static void Register(IPrunable cache) => Caches.Add(cache);

    internal static void PruneAll()
    {
        foreach (var cache in Caches)
        {
            cache.Prune();
        }
    }
}

// Per-object state keyed by instance ID, holding the object itself so destroyed ones can be detected.
internal sealed class ObjectStateCache<TObject, TState> : IPrunable where TObject : UnityEngine.Object
{
    private readonly Dictionary<int, (TObject Owner, TState State)> _entries = new();

    internal ObjectStateCache() => ObjectStateCaches.Register(this);

    internal IEnumerable<TObject> Owners => _entries.Values.Select(entry => entry.Owner);

    internal bool Contains(TObject owner) => _entries.ContainsKey(owner.GetInstanceID());

    internal bool TryGet(TObject owner, [MaybeNullWhen(false)] out TState state)
    {
        if (_entries.TryGetValue(owner.GetInstanceID(), out var entry))
        {
            state = entry.State;
            return true;
        }

        state = default;
        return false;
    }

    internal void Set(TObject owner, TState state) => _entries[owner.GetInstanceID()] = (owner, state);

    internal bool TryAdd(TObject owner, TState state) => _entries.TryAdd(owner.GetInstanceID(), (owner, state));

    internal bool Remove(TObject owner, [MaybeNullWhen(false)] out TState state)
    {
        if (_entries.Remove(owner.GetInstanceID(), out var entry))
        {
            state = entry.State;
            return true;
        }

        state = default;
        return false;
    }

    public void Prune()
    {
        foreach (var (id, entry) in _entries.ToArray())
        {
            if (entry.Owner == null)
            {
                _entries.Remove(id);
            }
        }
    }
}