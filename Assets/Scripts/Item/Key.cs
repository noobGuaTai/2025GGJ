using UnityEngine;

public class Key: MonoBehaviour
{
    public Door.DoorKey doorKey = Door.DoorKey.Red;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            var player = other.gameObject;
            var playerInv = player.GetComponent<PlayerInventory>();
            playerInv.hasKey.Add(doorKey);
            Destroy(gameObject);
        }
    }
}