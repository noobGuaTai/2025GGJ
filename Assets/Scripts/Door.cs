using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Need Set")]
    public DoorKey needKey = DoorKey.None;
    public DoorButton doorButton;
    public bool isOpen;
    public bool fullyOpened;
    Tween tween;
    [Serializable]
    public enum DoorKey{
        None,
        Yellow,
        Red
    }
    [Header("Do Not Set")]
    public PlayerInventory playerInventory;
    GameObject doorInnerMask;
    public void Start()
    {
        playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        tween = gameObject.AddComponent<Tween>();
        doorInnerMask = transform.Find("DoorMask").gameObject;
    }
    
    public bool Open(){
        if (needKey != DoorKey.None && !playerInventory.hasKey.Contains(needKey))
            return OpenFailed();
        if (doorButton != null && !doorButton.isPressing)
            return OpenFailed();
        return OpenSuccess();
    }
    public Vector3 startPos;
    public Vector3 toPos;
    void FixedUpdate()
    {
        if(!isOpen)
            Open();
    }
    public bool OpenSuccess(){
        isOpen = true;
        tween.AddTween(x => doorInnerMask.transform.localPosition = x, startPos, toPos, 1f).AddTween(_ => fullyOpened=true,0,0,0).Play();
        return true;
    }
    public bool OpenFailed(){
        return true;
    }
    public bool TryEnter(){
        if(!isOpen) 
            return false;
        return true;
    }

}
