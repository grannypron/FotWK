using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{

    public static bool inittedAlreadyTODORemove = false; // TODO: Remove

    const int HIDE_X_OFFSET = 1000;
    public static void HideObject(GameObject obj)
    {
        obj.transform.position = new Vector3(HIDE_X_OFFSET + obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
    }

    public static void ShowObject(GameObject obj)
    {
        obj.transform.position = new Vector3((-1*HIDE_X_OFFSET) + obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
    }

    public static string centerString(string s)
    {
        int width = Constants.SCREEN_NUM_CHARS_PER_LINE;
        if (s.Length >= width)
        {
            return new string(' ', Constants.SCREEN_LINE_CENTERING_OFFSET) + s;
        }

        int leftPadding = (width - s.Length) / 2;
        int rightPadding = width - s.Length - leftPadding;

        return new string(' ', Constants.SCREEN_LINE_CENTERING_OFFSET) + new string(' ', leftPadding) + s + new string(' ', rightPadding);
    }

    public static string addS(int quantity, string str)
    {
        return str + (quantity == 1 ? "" : "S");

    }
    public static void assert(bool assert)
    {
        System.Diagnostics.Debug.Assert(assert);
    }
}




