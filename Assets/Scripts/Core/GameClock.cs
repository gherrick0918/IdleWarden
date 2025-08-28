using UnityEngine;

public class GameClock : MonoBehaviour {
    [Tooltip("Seconds between simulation ticks")]
    public float TickInterval = 0.1f; // 10 ticks/sec
    private float _accum;

    public static event System.Action<float> OnTick; // dt per tick

    void Update() {
        _accum += Time.unscaledDeltaTime;
        while (_accum >= TickInterval) {
            OnTick?.Invoke(TickInterval);
            _accum -= TickInterval;
        }
    }
}
