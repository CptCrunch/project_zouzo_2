using UnityEngine;
using System.Collections;

[System.Serializable]
public class DebugValues
{
    public bool Active;

    public bool Main;
    public bool Testing;
    public bool Player;
    public bool Damage;
    public bool Spells;
    public bool Cooldown;
    public bool UI;
    public bool Animation;
    public bool Condition;
    public bool Controles;
    public bool MapFeature;
    public bool Time;

    public DebugValues(bool active, bool Main, bool Testing, bool Player, bool Damage, bool Spells, bool Cooldown, bool UI, bool Animation, bool Condition, bool Controles, bool MapFeature, bool Time)
    {
        this.Active = active;
        this.Main = Main;
        this.Testing = Testing;
        this.Player = Player;
        this.Damage = Damage;
        this.Spells = Spells;
        this.Cooldown= Cooldown;
        this.UI = UI;
        this.Animation = Animation;
        this.Condition = Condition;
        this.Controles = Controles;
        this.MapFeature = MapFeature;
        this.Time = Time;
    }

    public void SetAllValues(bool value)
    {
        CustomDebug.Active = value;
        CustomDebug.EnableTag("Main", value);
        CustomDebug.EnableTag("Testing", value);
        CustomDebug.EnableTag("Player", value);
        CustomDebug.EnableTag("Damage", value);
        CustomDebug.EnableTag("Spells", value);
        CustomDebug.EnableTag("Cooldown", value);
        CustomDebug.EnableTag("UI", value);
        CustomDebug.EnableTag("Animation", value);
        CustomDebug.EnableTag("Condition", value);
        CustomDebug.EnableTag("Controles", value);
        CustomDebug.EnableTag("MapFeature", value);
        CustomDebug.EnableTag("Time", value);
    }

    public void ResetValues()
    {
        CustomDebug.Active = false;
        CustomDebug.EnableTag("Main", false);
        CustomDebug.EnableTag("Testing", false);
        CustomDebug.EnableTag("Player", false);
        CustomDebug.EnableTag("Damage", false);
        CustomDebug.EnableTag("Spells", false);
        CustomDebug.EnableTag("Cooldown", false);
        CustomDebug.EnableTag("UI", false);
        CustomDebug.EnableTag("Animation", false);
        CustomDebug.EnableTag("Condition", false);
        CustomDebug.EnableTag("Controles", false);
        CustomDebug.EnableTag("MapFeature", false);
        CustomDebug.EnableTag("Time", false);
    }
}
