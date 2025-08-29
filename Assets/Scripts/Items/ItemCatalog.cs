using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "IdleRPG/ItemCatalog")]
public class ItemCatalog : ScriptableObject
{
    public ItemDef[] AllItems;

    public ItemDef[] ForSlot(ItemSlot slot)
    {
        if (AllItems == null || AllItems.Length == 0)
            return new ItemDef[0];

        var list = new List<ItemDef>();
        foreach (var item in AllItems)
        {
            if (item && item.Slot == slot)
                list.Add(item);
        }
        return list.ToArray();
    }
}
