using TMPro;
using UnityEngine;

public class GoldHud : MonoBehaviour {
    public CombatSimulator Simulator;
    public TextMeshProUGUI GoldText;

    void Update() {
        if (Simulator && GoldText) {
            var dps = Simulator.DebugDPS();
            GoldText.text = $"Gold: {Simulator.GetGold():N0}\nDPS: {dps:F1}\nGPS: {dps * Simulator.goldPerDamage:F2}";
        }
    }
}
