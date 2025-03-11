using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int amount;
    public AudioSource collectAudio;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var player = other.gameObject;
            var playerInv = player.GetComponent<PlayerInventory>();
            playerInv.coins += amount;
            if (!collectAudio.isPlaying)
                collectAudio.Play();
            Destroy(gameObject);
        }
    }
}