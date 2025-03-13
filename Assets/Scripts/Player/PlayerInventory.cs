using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public HashSet<Door.DoorKey> hasKey = new();
    public int coins;
}