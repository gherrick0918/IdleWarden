using UnityEngine;

public class LoadoutDemo : MonoBehaviour {
    public LoadoutPresenter Presenter;
    public ItemDef Head, Body, Weapon, Offhand;
    void Start() { if (Presenter) Presenter.Apply(Head, Body, Weapon, Offhand); }
}
