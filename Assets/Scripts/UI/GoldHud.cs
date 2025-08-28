using TMPro;
using UnityEngine;

public class GoldHud : MonoBehaviour {
    public CombatSimulator Simulator;
    public TextMeshProUGUI GoldText;

    void Update() {
        if (Simulator && GoldText) {
            GoldText.text = $"Gold: {Simulator.GetGold():N0}";
        }
    }
}
