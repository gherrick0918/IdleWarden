using TMPro;
using UnityEngine;

public class StatsReadout : MonoBehaviour
{
    public CombatSimulator sim;
    public TextMeshProUGUI text;

    void Update()
    {
        if (sim && text)
        {
            var dps = sim.DebugDPS();
            text.text = $"Gold: {sim.GetGold():N0}\nDPS: {dps:F1}\nGPS: {dps * sim.goldPerDamage:F2}";
        }
    }
}
