using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Need Set")]
    public DoorKey needKey = DoorKey.None;
    public List<DoorButton> doorButtons = new();
    public List<DoorTrader> doorTraders = new();
    [Serializable]
    public enum DoorKey
    {
        None,
        Yellow,
        Red
    }
    [Header("Do Not Set")]
    public bool isOpen;
    public bool fullyOpened;
    Tween tween;
    public PlayerInventory playerInventory;
    GameObject doorInnerMask;
    public void Start()
    {
        playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        tween = gameObject.AddComponent<Tween>();
        doorInnerMask = transform.Find("DoorMask").gameObject;
    }

    public bool Open()
    {
        if (needKey != DoorKey.None && !playerInventory.hasKey.Contains(needKey))
            return OpenFailed();
        if (doorButtons.Count == 0 && doorTraders.Count == 0)
            return OpenSuccess();
        if (doorButtons.All(x => x.isPressing) && doorTraders.All(x => x.deal))
            return OpenSuccess();
        return OpenFailed();
    }
    public Vector3 startPos;
    public Vector3 toPos;
    void FixedUpdate()
    {
        if (!isOpen)
            Open();
        // if (!isOpen && GameManager.Instance.isReturning)
        // {
        //     OpenSuccess();
        // }
    }
    public bool OpenSuccess()
    {
        isOpen = true;
        tween.AddTween(x => doorInnerMask.transform.localPosition = x, startPos, toPos, 1f).AddTween(_ => fullyOpened = true, 0, 0, 0).Play();
        playerInventory.hasKey.Remove(needKey);
        return true;
    }
    public bool OpenFailed()
    {
        return true;
    }
    public bool TryEnter()
    {
        if (!isOpen)
            return false;
        // GameManager.Instance.NextGame();
        return true;
    }

}
