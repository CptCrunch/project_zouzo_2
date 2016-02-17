using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Util{

    public static float Ease(float x, float easeAmount)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    /// <summary>Retrun a Array with the GameObject Included</summary>
    /// <returns> returns a array with the GameObject added </returns>
    public static GameObject[] IncludeGameObjectOld(GameObject[] _entrys, GameObject _newEntry)
    {
        GameObject[] newArray = _entrys;

        bool registered = false;
        int emptyIndex = _entrys.Length;

        // check each entry
        for (int i = 0; i < newArray.Length; i++)
        {
            // check if entry is null
            if (newArray[i] == null) { emptyIndex = i; }
            // if not check if GameObject is registered
            else if (newArray[i] == _newEntry) { registered = true; }
        }

        // add entry if not registered
        if (!registered) { newArray[emptyIndex] = _newEntry; }

        return newArray;
    }

    /// <summary>checks if a GameObject is included in an Array</summary>
    /// <param name="_entrys">the Array</param>
    /// <param name="_newEntry">the GameObject</param>
    /// <returns>true if GameObject is included</returns>
    public static bool IsGameObjectIncluded(GameObject[] _entrys, GameObject _newEntry)
    {
        // check each entry
        for (int i = 0; i < _entrys.Length; i++)
        {
            // check if entry is null
            if (_entrys[i] != null)
            { 
                // check if GameObject is registered
                if (_entrys[i] == _newEntry) { return true; }
            }
        }
        return false;
    }

    /// <summary>adds an GameObject to a Array</summary>
    /// <param name="_entrys">the Array</param>
    /// <param name="_newEntry">the GameObject</param>
    public static void IncludeGameObject(GameObject[] _entrys, GameObject _newEntry)
    {
        // check each entry
        for (int i = 0; i < _entrys.Length; i++)
        {
            // check if entry is null and adds new entry
            if (_entrys[i] == null) { _entrys[i] = _newEntry; break; }
        }
    }

    public static void MixArray(int[] _mix)
    {
        for (int i = 0; i < 10; i++)
        {
            int random1 = UnityEngine.Random.RandomRange(0, 3);
            int random2 = UnityEngine.Random.RandomRange(0, 3);

            int pos1 = _mix[random1];
            int pos2 = _mix[random2];

            _mix[random1] = pos2;
            _mix[random2] = pos1;
        }
    }

    #region Test Joysick Axis
    public static bool LeftJoystickUp(string axis) 
    {
        if(Input.GetAxisRaw(axis + "_Vertical") > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }
    public static bool LeftJoystickDown(string axis)
    {
        if (Input.GetAxisRaw(axis + "_Vertical") < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool LeftJoystickRight(string axis)
    {
        if (Input.GetAxisRaw(axis + "_Horizontal") > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool LeftJoystickLeft(string axis)
    {
        if (Input.GetAxisRaw(axis + "_Horizontal") < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region To days
    public static double ConvertMillisecondsToDays(double milliseconds)
    {
        return TimeSpan.FromMilliseconds(milliseconds).TotalDays;
    }

    public static double ConvertSecondsToDays(double seconds)
    {
        return TimeSpan.FromSeconds(seconds).TotalDays;
    }

    public static double ConvertMinutesToDays(double minutes)
    {
        return TimeSpan.FromMinutes(minutes).TotalDays;
    }

    public static double ConvertHoursToDays(double hours)
    {
        return TimeSpan.FromHours(hours).TotalDays;
    }
    #endregion

    #region To hours
    public static double ConvertMillisecondsToHours(double milliseconds)
    {
        return TimeSpan.FromMilliseconds(milliseconds).TotalHours;
    }

    public static double ConvertSecondsToHours(double seconds)
    {
        return TimeSpan.FromSeconds(seconds).TotalHours;
    }

    public static double ConvertMinutesToHours(double minutes)
    {
        return TimeSpan.FromMinutes(minutes).TotalHours;
    }

    public static double ConvertDaysToHours(double days)
    {
        return TimeSpan.FromHours(days).TotalHours;
    }
    #endregion

    #region To minutes
    public static double ConvertMillisecondsToMinutes(double milliseconds)
    {
        return TimeSpan.FromMilliseconds(milliseconds).TotalMinutes;
    }

    public static double ConvertSecondsToMinutes(double seconds)
    {
        return TimeSpan.FromSeconds(seconds).TotalMinutes;
    }

    public static double ConvertHoursToMinutes(double hours)
    {
        return TimeSpan.FromHours(hours).TotalMinutes;
    }

    public static double ConvertDaysToMinutes(double days)
    {
        return TimeSpan.FromDays(days).TotalMinutes;
    }
    #endregion

    #region To seconds
    public static double ConvertMillisecondsToSeconds(double milliseconds)
    {
        return TimeSpan.FromMilliseconds(milliseconds).TotalSeconds;
    }

    public static double ConvertMinutesToSeconds(double minutes)
    {
        return TimeSpan.FromMinutes(minutes).TotalSeconds;
    }

    public static double ConvertHoursToSeconds(double hours)
    {
        return TimeSpan.FromHours(hours).TotalSeconds;
    }

    public static double ConvertDaysToSeconds(double days)
    {
        return TimeSpan.FromDays(days).TotalSeconds;
    }
    #endregion

    #region To milliseconds
    public static double ConvertSecondsToMilliseconds(double seconds)
    {
        return TimeSpan.FromSeconds(seconds).TotalMilliseconds;
    }

    public static double ConvertMinutesToMilliseconds(double minutes)
    {
        return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
    }

    public static double ConvertHoursToMilliseconds(double hours)
    {
        return TimeSpan.FromHours(hours).TotalMilliseconds;
    }

    public static double ConvertDaysToMilliseconds(double days)
    {
        return TimeSpan.FromDays(days).TotalMilliseconds;
    }
    #endregion

    #region AimDirection
    /// <summary> returns te direction which the vector is aiming at </summary>
    /// <param name="_direction"></param>
    /// <returns> right, left, noAim </returns>
    public static string Aim2Direction(Vector2 _direction)
    {
        string direction = "noAim";

        float angle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;

        if (angle < 90 && angle > -90) { direction = "right"; }
        if ((angle < 180 && angle > 90) || angle < -90) { direction = "left"; }

        if ((_direction.x == 0 && _direction.y == 0) || angle == 90 || angle == -90) { direction = "noAim"; }

        return direction;
    }

    /// <summary> returns te direction which the vector is aiming at </summary>
    /// <param name="_direction"></param>
    /// <returns> up, right, down, left, noAim </returns>
    public static string Aim4Direction(Vector2 _direction)
    {
        string direction = "noAim";

        float angle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;

        if (angle <= 90 + 45 && angle > 90 - 45) { direction = "up"; }
        if (angle <= 0 + 45 && angle > 0 - 45) { direction = "right"; }
        if (angle <= -90 + 45 && angle > -90 - 45) { direction = "down"; }
        if ((angle <= 180 && angle > 180 - 45) || (angle < -90 - 45)) { direction = "left"; }

        if (_direction.x == 0 && _direction.y == 0) { direction = "noAim"; }

        return direction;
    }

    /// <summary> returns te direction which the vector is aiming at </summary>
    /// <param name="_direction"></param>
    /// <returns> up, upRight, right, downRight, down, downLeft, left, upLeft, noAim </returns>
    public static string Aim8Direction(Vector2 _direction)
    {
        string direction = "noAim";
        
        float angle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;
        
        if (angle <= 90 + 22.5f && angle > 90 - 22.5f) { direction = "up"; }
        if (angle <= 45 + 22.5f && angle > 45 - 22.5f) { direction = "upRight"; }
        if (angle <= 22.5f && angle > -22.5f) { direction = "right"; }
        if (angle <= -45 + 22.5f && angle > -45 - 22.5f) { direction = "downRight"; }
        if (angle <= -90 + 22.5f && angle > -90 - 22.5f) { direction = "down"; }
        if (angle <= -135 + 22.5f && angle > -135 - 22.5f) { direction = "downLeft"; }
        if ((angle <= 180 && angle > 180 - 22.5f) || angle < -135 - 22.5f) { direction = "left"; }
        if (angle <= 135 + 22.5f && angle > 135 - 22.5f) { direction = "upLeft"; }

        if (_direction.x == 0 && _direction.y == 0)
        {
            direction = "noAim";
        }

        return direction;
    }

    public static Vector2 GetAimDirection(Attacks _spell, Player _playerInstance)
    {
        string playerAxis = _playerInstance.playerAxis;
        bool mirror = _playerInstance.Mirror;

        switch (_spell.SpellDir)
        {
            case 2:
                // get the aim direction
                switch (Util.Aim2Direction(new Vector2(Input.GetAxis(playerAxis + "_Vertical"), Input.GetAxis(playerAxis + "_Horizontal"))))
                {
                    case "right": return new Vector2(1, 0);
                    case "left": return new Vector2(-1, 0);
                    case "noAim":
                        if (mirror) { return new Vector2(1, 0); }
                        else { return new Vector2(-1, 0); }
                }
                break;

            case 4:
                // get the aim direction
                switch (Util.Aim4Direction(new Vector2(Input.GetAxis(playerAxis + "_Vertical"), Input.GetAxis(playerAxis + "_Horizontal"))))
                {
                    case "up": return new Vector2(0, 1);
                    case "right": return new Vector2(1, 0);
                    case "down": return new Vector2(0, -1);
                    case "left": return new Vector2(-1, 0);
                    case "noAim":
                        if (mirror) { return new Vector2(1, 0); }
                        else { return new Vector2(-1, 0); }
                }
                break;

            case 8:
                // get the aim direction
                switch (Util.Aim8Direction(new Vector2(Input.GetAxis(playerAxis + "_Vertical"), Input.GetAxis(playerAxis + "_Horizontal"))))
                {
                    case "up": return new Vector2(0, 1);
                    case "upRight": return new Vector2(0.5f, 0.5f);
                    case "right": return new Vector2(1, 0);
                    case "downRight": return new Vector2(0.5f, -0.5f);
                    case "down": return new Vector2(0, -1);
                    case "downLeft": return new Vector2(-0.5f, -0.5f);
                    case "left": return new Vector2(-1, 0);
                    case "upLeft": return new Vector2(-1, 0.5f);
                    case "noAim":
                        if (mirror) { return new Vector2(1, 0); }
                        else { return new Vector2(-1, 0); }
                }
                break;
        }

        return new Vector2(0, 0);
    }
    #endregion
}
