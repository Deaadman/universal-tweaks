namespace UniversalTweaks.Utilities;

internal static class ComponentUtilities
{
    private static readonly Dictionary<string, Dictionary<Type, Component>> StoredComponents = [];

    internal static void RemoveComponent<T>(params string[] itemNames) where T : Component
    {
        foreach (var itemName in itemNames)
        {
            var item = GearItem.LoadGearItemPrefab(itemName);
            if (item == null)
            {
                Mod.Logger.Log($"Gear item prefab '{itemName}' was not found; skipping component removal.", FlaggedLoggingLevel.Warning);
                continue;
            }

            var component = item.gameObject.GetComponent<T>();
            if (component == null)
            {
                Mod.Logger.Log($"'{itemName}' has no {typeof(T).Name} to remove.", FlaggedLoggingLevel.Trace);
                continue;
            }

            if (!StoredComponents.ContainsKey(itemName))
            {
                StoredComponents[itemName] = [];
            }

            StoredComponents[itemName][typeof(T)] = component;
            UnityEngine.Object.Destroy(component);

            Mod.Logger.Log($"Removed {typeof(T).Name} from '{itemName}'.", FlaggedLoggingLevel.Debug);
        }
    }

    internal static void RestoreComponent<T>(params string[] itemNames) where T : Component
    {
        foreach (var itemName in itemNames)
        {
            if (!StoredComponents.TryGetValue(itemName, out var components) ||
                !components.TryGetValue(typeof(T), out var component))
            {
                Mod.Logger.Log($"No stored {typeof(T).Name} to restore for '{itemName}'.", FlaggedLoggingLevel.Trace);
                continue;
            }

            var item = GearItem.LoadGearItemPrefab(itemName);
            if (item != null && item.gameObject.GetComponent<T>() == null)
            {
                item.gameObject.AddComponent<T>().CopyFrom(component);
                Mod.Logger.Log($"Restored {typeof(T).Name} on '{itemName}'.", FlaggedLoggingLevel.Debug);
            }
        }
    }

    private static void CopyFrom<T>(this T destination, Component source) where T : Component
    {
        var type = destination.GetType();
        if (source.GetType() != type)
        {
            return;
        }

        try
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                field.SetValue(destination, field.GetValue(source));
            }
        }
        catch (Exception ex)
        {
            Mod.Logger.Log($"Failed to copy field values onto {type.Name} via reflection.", FlaggedLoggingLevel.Exception, ex);
        }
    }
}