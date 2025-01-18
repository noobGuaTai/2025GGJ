using UnityEngine;

static public class ObjectLib
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour{
        var ret = gameObject.GetComponent<T>();
        if (ret != null) return ret;
        return gameObject.AddComponent<T>();
    }   
}