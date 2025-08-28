using UnityEngine;

public enum ItemSlot { Head, Body, Weapon, Offhand }

[CreateAssetMenu(menuName = "IdleRPG/ItemDef")]
public class ItemDef : ScriptableObject {
    public string Id;
    public ItemSlot Slot;
    public Sprite Sprite;
    // Future: stat modifiers, rarity, set bonuses, etc.
}
