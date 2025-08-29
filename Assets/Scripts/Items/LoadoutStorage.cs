using UnityEngine;
using System.Collections.Generic;

public static class LoadoutStorage
{
    [System.Serializable]
    private class Ids
    {
        public string Head;
        public string Body;
        public string Weapon;
        public string Offhand;
    }

    // Build a map of ItemDef.Id → ItemDef by scanning Resources (if any).
    // If you don't keep ItemDefs under Resources/, this will just return an empty map
    // and we gracefully skip unresolved items.
    private static Dictionary<string, ItemDef> BuildIdMap()
    {
        var map = new Dictionary<string, ItemDef>();
        var all = Resources.LoadAll<ItemDef>(""); // searches all Resources folders
        foreach (var i in all)
        {
            if (!string.IsNullOrEmpty(i.Id) && !map.ContainsKey(i.Id))
                map[i.Id] = i;
        }
        return map;
    }

    public static Loadout Load()
    {
        var data = SaveService.LoadOrNew();
        if (string.IsNullOrEmpty(data.equippedItemIdsJson))
            return new Loadout();

        Loadout l = new Loadout();
        try
        {
            var ids = JsonUtility.FromJson<Ids>(data.equippedItemIdsJson);
            var map = BuildIdMap();

            if (ids != null)
            {
                if (!string.IsNullOrEmpty(ids.Head) && map.TryGetValue(ids.Head, out var h)) l.Head = h;
                if (!string.IsNullOrEmpty(ids.Body) && map.TryGetValue(ids.Body, out var b)) l.Body = b;
                if (!string.IsNullOrEmpty(ids.Weapon) && map.TryGetValue(ids.Weapon, out var w)) l.Weapon = w;
                if (!string.IsNullOrEmpty(ids.Offhand) && map.TryGetValue(ids.Offhand, out var o)) l.Offhand = o;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"LoadoutStorage.Load: failed to parse JSON. {e.Message}");
            // Return empty Loadout if parse fails
        }

        return l;
    }

    public static void Save(Loadout l)
    {
        var data = SaveService.LoadOrNew();
        var ids = new Ids
        {
            Head = l?.Head?.Id,
            Body = l?.Body?.Id,
            Weapon = l?.Weapon?.Id,
            Offhand = l?.Offhand?.Id
        };

        data.equippedItemIdsJson = JsonUtility.ToJson(ids);
        SaveService.Save(data);
    }
}