using System;
using UnityEngine;

public class CoinDoor : MonoBehaviour
{
    public bool isOpenOnStart = false;
    void Start()
    {
        if (GameManager.Instance.isReturning && isOpenOnStart)
            Open();
    }
    public void Open()
    {
        Debug.Log("[CoinDoor] open");
        var tw = gameObject.GetOrAddComponent<Tween>();
        tw.AddTween("down", (x) => transform.position = transform.position.NewY(x), 0, -50, 0.1f);
        tw.Play();
    }
}
