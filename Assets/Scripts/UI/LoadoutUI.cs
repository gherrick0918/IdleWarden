using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutUI : MonoBehaviour
{
    public ItemCatalog catalog;
    public LoadoutPresenter presenter;
    public CombatSimulator simulator;
    public RectTransform root;
    public TextMeshProUGUI header;

    private readonly Dictionary<ItemSlot, TextMeshProUGUI> _sectionHeaders = new Dictionary<ItemSlot, TextMeshProUGUI>();

    void Start()
    {
        if (!catalog)
            catalog = Resources.Load<ItemCatalog>("ItemCatalog");
        if (!catalog)
        {
            var canvas = EnsureCanvas();
            var label = new GameObject("NoCatalog", typeof(TextMeshProUGUI));
            label.transform.SetParent(canvas.transform, false);
            label.GetComponent<TextMeshProUGUI>().text = "No ItemCatalog assigned";
            return;
        }

        var c = EnsureCanvas();
        if (!root)
        {
            var panel = new GameObject("LoadoutPanel", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            panel.transform.SetParent(c.transform, false);
            var img = panel.GetComponent<Image>();
            img.color = new Color(0, 0, 0, 0.25f);
            var vg = panel.GetComponent<VerticalLayoutGroup>();
            vg.padding = new RectOffset(10, 10, 10, 10);
            vg.spacing = 4;
            vg.childControlWidth = true;
            vg.childForceExpandWidth = true;
            var fitter = panel.GetComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            root = panel.GetComponent<RectTransform>();
        }

        if (header)
            header.text = "Loadout";

        BuildSection(ItemSlot.Head, "Head");
        BuildSection(ItemSlot.Body, "Body");
        BuildSection(ItemSlot.Weapon, "Weapon");
        BuildSection(ItemSlot.Offhand, "Offhand");

        var current = LoadoutStorage.Load();
        UpdateHeader(ItemSlot.Head, current.Head);
        UpdateHeader(ItemSlot.Body, current.Body);
        UpdateHeader(ItemSlot.Weapon, current.Weapon);
        UpdateHeader(ItemSlot.Offhand, current.Offhand);
    }

    Canvas EnsureCanvas()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (!canvas)
        {
            var go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        return canvas;
    }

    void BuildSection(ItemSlot slot, string label)
    {
        var section = new GameObject($"{label}Section", typeof(RectTransform), typeof(VerticalLayoutGroup));
        section.transform.SetParent(root, false);
        var layout = section.GetComponent<VerticalLayoutGroup>();
        layout.spacing = 2;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;

        var headerGO = new GameObject("Header", typeof(TextMeshProUGUI));
        headerGO.transform.SetParent(section.transform, false);
        var headerText = headerGO.GetComponent<TextMeshProUGUI>();
        headerText.text = label;
        _sectionHeaders[slot] = headerText;

        CreateButton(section.transform, "Unequip", () => Equip(slot, null));

        var items = catalog.ForSlot(slot);
        foreach (var item in items)
        {
            var id = string.IsNullOrEmpty(item.Id) ? item.name : item.Id;
            CreateButton(section.transform, id, () => Equip(slot, item));
        }
    }

    void CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);
        var img = go.GetComponent<Image>();
        img.color = new Color(1, 1, 1, 0.1f);

        var btn = go.GetComponent<Button>();
        btn.onClick.AddListener(onClick);

        var textGO = new GameObject("Text", typeof(TextMeshProUGUI));
        textGO.transform.SetParent(go.transform, false);
        var tmp = textGO.GetComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableAutoSizing = true;
        var rt = tmp.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var fitter = go.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    void Equip(ItemSlot slot, ItemDef item)
    {
        var loadout = LoadoutStorage.Load();
        switch (slot)
        {
            case ItemSlot.Head: loadout.Head = item; break;
            case ItemSlot.Body: loadout.Body = item; break;
            case ItemSlot.Weapon: loadout.Weapon = item; break;
            case ItemSlot.Offhand: loadout.Offhand = item; break;
        }
        LoadoutStorage.Save(loadout);
        if (presenter) presenter.Apply(loadout.Head, loadout.Body, loadout.Weapon, loadout.Offhand);
        if (simulator) simulator.RecomputeStats(loadout);
        UpdateHeader(slot, item);
    }

    void UpdateHeader(ItemSlot slot, ItemDef item)
    {
        if (_sectionHeaders.TryGetValue(slot, out var t))
        {
            var baseLabel = slot.ToString();
            t.text = item ? $"{baseLabel}: {item.Id}" : baseLabel;
        }
    }
}
