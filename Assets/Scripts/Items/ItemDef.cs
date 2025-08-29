using UnityEngine;

public enum ItemSlot { Head, Body, Weapon, Offhand }

[CreateAssetMenu(menuName = "IdleRPG/ItemDef")]
public class ItemDef : ScriptableObject {
    public string Id;
    public ItemSlot Slot;
    public Sprite Sprite;
    [Tooltip("Flat STR bonus")]
    public int str;
    [Tooltip("Flat DEX bonus")]
    public int dex;
    [Tooltip("Average damage bonus, e.g. 0.10 = +10%")]
    public float crit;
    [Tooltip("Attack speed bonus (multiplicative), e.g. 0.15 = +15%")]
    public float atkspd;
}
