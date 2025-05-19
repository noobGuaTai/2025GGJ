using System;
using UnityEngine;

public class ItemCoin : MonoBehaviour
{
    public int amount;
    public AudioSource collectAudio;
    // AnythingCheck anythingCheck => GetComponent<AnythingCheck>();
    // Animator animator => GetComponent<Animator>();
    // AnimatorStateInfo currentStateInfo => animator.GetCurrentAnimatorStateInfo(0);

    // void Update()
    // {
    //     if (anythingCheck.isChecked && currentStateInfo.IsName("drop"))
    //     {
    //         animator.Play("idle", 0, 0);
    //     }
    //     else if (!anythingCheck.isChecked && currentStateInfo.IsName("idle"))
    //     {
    //         animator.Play("drop", 0, 0);
    //     }

    // }
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