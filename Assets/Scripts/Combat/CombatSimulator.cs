using UnityEngine;

public class CombatSimulator : MonoBehaviour
{
    [Header("Economy")]
    [Tooltip("Gold awarded per point of damage per second.")]
    public float goldPerDamage = 0.10f;

    [Header("Offline Rewards")]
    [Tooltip("Hours of offline time that pay full rate before decay is applied.")]
    public int FullRateHoursCap = 8;
    [Tooltip("Multiplier applied to gold after the full-rate cap (e.g., 0.5 = 50%).")]
    public double PostCapMultiplier = 0.5;

    [Header("Dev Seed (optional)")]
    [SerializeField] private ItemDef devHead;
    [SerializeField] private ItemDef devBody;
    [SerializeField] private ItemDef devWeapon;
    [SerializeField] private ItemDef devOffhand;
    [SerializeField, Tooltip("If true and there is no saved equipment, seed from the dev items once.")]
    private bool overwriteSavedOnStart = true;

    // --- runtime state ---
    private SaveData _save;
    private double _goldBuffer;

    // Stats
    [SerializeField] private StatBlock baseStats = new StatBlock { STR = 10, DEX = 10, CRIT = 0.10f, ATKSPD = 1.2f };
    private StatBlock finalStats;
    private Loadout currentLoadout = new Loadout();

    void OnEnable()
    {
        _save = SaveService.LoadOrNew();

        // Load/seed loadout
        currentLoadout = LoadoutStorage.Load();
        bool hasNoEquip =
            currentLoadout.Head == null &&
            currentLoadout.Body == null &&
            currentLoadout.Weapon == null &&
            currentLoadout.Offhand == null;

        if (hasNoEquip && (overwriteSavedOnStart || string.IsNullOrEmpty(_save.equippedItemIdsJson)))
        {
            if (devHead || devBody || devWeapon || devOffhand)
            {
                currentLoadout = new Loadout { Head = devHead, Body = devBody, Weapon = devWeapon, Offhand = devOffhand };
                LoadoutStorage.Save(currentLoadout);
            }
        }

        finalStats = currentLoadout.BuildFinalStats(baseStats);

        // Offline catch-up (parity with online math)
        long now = SaveService.NowUnix();
        int seconds = Mathf.Max(0, (int)(now - _save.lastSavedUnix));
        if (seconds > 0)
            SimulateOffline(seconds);

        _save.lastSavedUnix = now;
        GameClock.OnTick += HandleTick;
    }

    void OnDisable()
    {
        GameClock.OnTick -= HandleTick;
        _save.lastSavedUnix = SaveService.NowUnix();
        SaveService.Save(_save);
    }

    // Live tick
    void HandleTick(float dt)
    {
        var dps = (finalStats ?? baseStats).ComputeDPS();
        var gps = dps * goldPerDamage;

        _goldBuffer += gps * dt;
        if (_goldBuffer >= 1.0)
        {
            var whole = (long)_goldBuffer;
            _save.gold += whole;
            _goldBuffer -= whole;
        }
    }

    // Offline replay using the same DPS math
    void SimulateOffline(int seconds)
    {
        var dps = (finalStats ?? baseStats).ComputeDPS();
        var gps = dps * goldPerDamage;

        int fullCapSeconds = Mathf.Max(0, FullRateHoursCap) * 3600;
        int full = Mathf.Min(seconds, fullCapSeconds);
        int rest = Mathf.Max(0, seconds - full);

        _save.gold += gps * full + gps * PostCapMultiplier * rest;
    }

    // --- helpers for UI/Debug ---
    public double GetGold() => _save?.gold ?? 0;
    public float DebugDPS() => (float)((finalStats ?? baseStats).ComputeDPS()); // used by UI

    // Called by UI when equipment changes later
    public void RecomputeStats(Loadout l)
    {
        currentLoadout = l ?? new Loadout();
        finalStats = currentLoadout.BuildFinalStats(baseStats);
    }
}
