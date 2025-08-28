using UnityEngine;

public class LoadoutPresenter : MonoBehaviour {
    [Header("Slot Renderers")]
    public SpriteRenderer Head;
    public SpriteRenderer Body;
    public SpriteRenderer Weapon;
    public SpriteRenderer Offhand;

    public void Apply(ItemDef head, ItemDef body, ItemDef weapon, ItemDef offhand) {
        if (Head)   Head.sprite   = head   ? head.Sprite   : null;
        if (Body)   Body.sprite   = body   ? body.Sprite   : null;
        if (Weapon) Weapon.sprite = weapon ? weapon.Sprite : null;
        if (Offhand)Offhand.sprite= offhand? offhand.Sprite: null;
    }
}
