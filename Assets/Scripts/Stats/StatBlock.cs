using System;

public class StatBlock {
    public double STR;
    public double DEX;
    public double CRIT;
    public double ATKSPD;

    public double ComputeDPS() {
        double baseDamage = 5.0 + STR * 0.5 + DEX * 0.25;
        double critMult = 1.0 + CRIT;
        return baseDamage * ATKSPD * critMult;
    }
}
