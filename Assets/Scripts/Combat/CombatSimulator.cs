using UnityEngine;

public class CombatSimulator : MonoBehaviour {
    [Header("Tuning")]
    public double GoldPerSecondBase = 1.0;
    public double GoldPerDamage = 0.1;
    public int FullRateHoursCap = 8;
    public double PostCapMultiplier = 0.5;

    private double _goldBuffer;
    private SaveData _save;
    private StatBlock baseStats = new StatBlock { STR = 10, DEX = 10, CRIT = 0.10, ATKSPD = 1.2 };
    private StatBlock finalStats;
    private Loadout currentLoadout;

    [SerializeField] private ItemDef devHead, devBody, devWeapon, devOffhand;
    [SerializeField] private bool overwriteSavedOnStart = true;

    void OnEnable() {
        _save = SaveService.LoadOrNew();
        // Offline catch-up
        var deltaSec = Mathf.Max(0, (int)(SaveService.NowUnix() - _save.lastSavedUnix));
        SimulateOffline(deltaSec);

        currentLoadout = LoadoutStorage.Load();
        if (overwriteSavedOnStart || (currentLoadout.Head == null && currentLoadout.Body == null && currentLoadout.Weapon == null && currentLoadout.Offhand == null)) {
            currentLoadout = new Loadout { Head = devHead, Body = devBody, Weapon = devWeapon, Offhand = devOffhand };
            LoadoutStorage.Save(currentLoadout);
        }
        finalStats = currentLoadout.BuildFinalStats(baseStats);

        GameClock.OnTick += HandleTick;
    }

    void OnDisable() {
        GameClock.OnTick -= HandleTick;
        Persist();
    }

    void HandleTick(float dt) {
        double dps = finalStats.ComputeDPS();
        double gps = dps * GoldPerDamage;
        _goldBuffer += gps * dt;
        if (_goldBuffer >= 1.0) {
            var whole = (long)_goldBuffer;
            _save.gold += whole;
            _goldBuffer -= whole;
        }
    }

    void OnApplicationPause(bool paused) { if (paused) Persist(); }
    void OnApplicationQuit() { Persist(); }

    void Persist() {
        _save.lastSavedUnix = SaveService.NowUnix();
        SaveService.Save(_save);
        LoadoutStorage.Save(currentLoadout);
    }

    void SimulateOffline(int seconds) {
        int cap = FullRateHoursCap * 3600;
        int full = Mathf.Min(seconds, cap);
        int rest = Mathf.Max(0, seconds - cap);
        _save.gold += full * GoldPerSecondBase + rest * (GoldPerSecondBase * PostCapMultiplier);
    }

    public double GetGold() => _save?.gold ?? 0;

    public void RecomputeStats(Loadout l) {
        currentLoadout = l;
        finalStats = currentLoadout.BuildFinalStats(baseStats);
        LoadoutStorage.Save(currentLoadout);
    }
}
