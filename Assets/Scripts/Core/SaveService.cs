using UnityEngine;

public static class SaveService {
    const string Key = "SAVE_V1";

    public static void Save(SaveData data) {
        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();
    }

    public static SaveData LoadOrNew() {
        if (!PlayerPrefs.HasKey(Key))
            return new SaveData { lastSavedUnix = NowUnix() };
        var json = PlayerPrefs.GetString(Key);
        var data = JsonUtility.FromJson<SaveData>(json);
        return data ?? new SaveData { lastSavedUnix = NowUnix() };
    }

    public static long NowUnix() =>
        (long)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
}
