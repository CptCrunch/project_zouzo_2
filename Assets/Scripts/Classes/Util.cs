using UnityEngine;
using System.Collections;
using System;

public static class Util{

    public static float Ease(float x, float easeAmount)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

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

    /// <summary></summary>
    /// <returns> returns a array with the GameObject added </returns>
    public static GameObject[] IsGameObjectIncluded(GameObject[] entrys, GameObject newEntry)
    {
        GameObject[] newArray = entrys;

        bool registered = false;
        int emptyIndex = entrys.Length;

        for (int i = 0; i < newArray.Length; i++)
        {

            if (newArray[i] == null) { emptyIndex = i; }
            else if (newArray[i] == newEntry) { registered = true; }
        }

        if (!registered)
        {
            newArray[emptyIndex] = newEntry;
        }

        return newArray;
    }

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
}
