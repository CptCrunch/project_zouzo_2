using UnityEngine;
using System.Collections;

[System.Serializable]
public class DebugValues
{
    public bool Active;

    public bool Main;
    public bool Player;
    public bool Damage;
    public bool Spells;
    public bool UI;
    public bool Testing;

    public DebugValues(bool active, bool Main, bool Player, bool Damage, bool Spells, bool UI, bool Testing)
    {
        this.Active = active;
        this.Main = Main;
        this.Player = Player;
        this.Damage = Damage;
        this.Spells = Spells;
        this.UI = UI;
        this.Testing = Testing;
    }

    public void SetAllValues(bool value)
    {
        CustomDebug.Active = value;
        CustomDebug.EnableTag("Main", value);
        CustomDebug.EnableTag("Player", value);
        CustomDebug.EnableTag("Damage", value);
        CustomDebug.EnableTag("Spells", value);
        CustomDebug.EnableTag("UI", value);
        CustomDebug.EnableTag("Testing", value);
    }
}
