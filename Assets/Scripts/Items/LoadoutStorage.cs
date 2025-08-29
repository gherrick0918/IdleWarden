using UnityEngine;

public static class LoadoutStorage {
    [System.Serializable]
    class Ids { public string Head, Body, Weapon, Offhand; }

    public static Loadout Load() {
        var data = SaveService.LoadOrNew();
        var l = new Loadout();
        if (string.IsNullOrEmpty(data.equippedItemIdsJson)) return l;
        Ids ids = null;
        try { ids = JsonUtility.FromJson<Ids>(data.equippedItemIdsJson); }
        catch { }
        if (ids == null) return l;
        l.Head = Find(ids.Head);
        l.Body = Find(ids.Body);
        l.Weapon = Find(ids.Weapon);
        l.Offhand = Find(ids.Offhand);
        return l;
    }

    public static void Save(Loadout l) {
        var data = SaveService.LoadOrNew();
        var ids = new Ids {
            Head = l?.Head?.Id,
            Body = l?.Body?.Id,
            Weapon = l?.Weapon?.Id,
            Offhand = l?.Offhand?.Id
        };
        data.equippedItemIdsJson = JsonUtility.ToJson(ids);
        SaveService.Save(data);
    }

    static ItemDef Find(string id) {
        if (string.IsNullOrEmpty(id)) return null;
        var all = Resources.FindObjectsOfTypeAll<ItemDef>();
        foreach (var it in all) if (it && it.Id == id) return it;
        return null;
    }
}
