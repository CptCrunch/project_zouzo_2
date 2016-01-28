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
    public bool UI;
    public bool Animation;
    public bool Condition;

    public DebugValues(bool active, bool Main, bool Testing, bool Player, bool Damage, bool Spells, bool UI, bool Animation, bool Condition)
    {
        this.Active = active;
        this.Main = Main;
        this.Testing = Testing;
        this.Player = Player;
        this.Damage = Damage;
        this.Spells = Spells;
        this.UI = UI;
        this.Animation = Animation;
        this.Condition = Condition;
    }

    public void SetAllValues(bool value)
    {
        CustomDebug.Active = value;
        CustomDebug.EnableTag("Main", value);
        CustomDebug.EnableTag("Testing", value);
        CustomDebug.EnableTag("Player", value);
        CustomDebug.EnableTag("Damage", value);
        CustomDebug.EnableTag("Spells", value);
        CustomDebug.EnableTag("UI", value);
        CustomDebug.EnableTag("Animation", value);
        CustomDebug.EnableTag("Condition", value);
    }

    public void ResetValues()
    {
        CustomDebug.Active = false;
        CustomDebug.EnableTag("Main", false);
        CustomDebug.EnableTag("Testing", false);
        CustomDebug.EnableTag("Player", false);
        CustomDebug.EnableTag("Damage", false);
        CustomDebug.EnableTag("Spells", false);
        CustomDebug.EnableTag("UI", false);
        CustomDebug.EnableTag("Animation", false);
        CustomDebug.EnableTag("Condition", false);
    }
}
