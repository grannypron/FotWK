using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    const int HIDE_X_OFFSET = 1000;
    public static void HideObject(GameObject obj)
    {
        obj.transform.position = new Vector3(HIDE_X_OFFSET + obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
    }

    public static void ShowObject(GameObject obj)
    {
        obj.transform.position = new Vector3((-1*HIDE_X_OFFSET) + obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
    }
}
