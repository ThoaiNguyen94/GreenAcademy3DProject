using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager
{
    public static void SetVector2(string key, Vector2 value)
    {
        string x = key + "Vector2-X";
        string y = key + "Vector2-Y";
        PlayerPrefs.SetFloat(x, value.x);
        PlayerPrefs.SetFloat(y, value.y);
    }

    public static Vector2 GetVector2(string key)
    {
        Vector2 value;
        string x = key + "Vector2-X";
        string y = key + "Vector2-Y";
        value.x = PlayerPrefs.GetFloat(x);
        value.y = PlayerPrefs.GetFloat(y);
        return value;
    }

    public static void SetVector3(string key, Vector3 value)
    {
        string x = key + "Vector3-X";
        string y = key + "Vector3-Y";
        string z = key + "Vector3-Z";
        PlayerPrefs.SetFloat(x, value.x);
        PlayerPrefs.SetFloat(y, value.y);
        PlayerPrefs.SetFloat(z, value.z);
    }

    public static Vector3 GetVector3(string key)
    {
        Vector3 value;
        string x = key + "Vector3-X";
        string y = key + "Vector3-Y";
        string z = key + "Vector3-Z";
        value.x = PlayerPrefs.GetFloat(x);
        value.y = PlayerPrefs.GetFloat(y);
        value.z = PlayerPrefs.GetFloat(z);
        return value;
    }

    public static void SetTransform(string key, Transform value)
    {
        string position = key + "Position";
        string rotation = key + "Rotation";
        string scale = key + "Scale";
        SetVector3(position, value.position);
        SetVector3(rotation, value.eulerAngles);
        SetVector3(scale, value.localScale);
    }

    public static void GetTransform(string key, Transform value)
    {
        string position = key + "Position";
        string rotation = key + "Rotation";
        string scale = key + "Scale";
        value.position = GetVector3(position);
        value.eulerAngles = GetVector3(rotation);
        value.localScale = GetVector3(scale);
    }
}
