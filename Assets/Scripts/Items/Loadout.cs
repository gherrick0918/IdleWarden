using UnityEngine;

[System.Serializable]
public class Loadout {
    public ItemDef Head, Body, Weapon, Offhand;

    public StatBlock BuildFinalStats(StatBlock baseStats) {
        var result = new StatBlock {
            STR = baseStats.STR,
            DEX = baseStats.DEX,
            CRIT = baseStats.CRIT,
            ATKSPD = baseStats.ATKSPD
        };
        void Apply(ItemDef item) {
            if (item == null) return;
            result.STR += item.str;
            result.DEX += item.dex;
            result.CRIT += item.crit;
            result.ATKSPD *= 1f + item.atkspd;
        }
        Apply(Head);
        Apply(Body);
        Apply(Weapon);
        Apply(Offhand);
        return result;
    }
}
