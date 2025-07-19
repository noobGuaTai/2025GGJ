using System;
using UnityEngine;

public class EnemyAttackAnything : MonoBehaviour
{
    public delegate void OnAttacked(Collider2D other);
    public event OnAttacked onAttacked;
    public LayerMask aim;
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((aim & (1 << other.gameObject.layer)) != 0)
        {
            onAttacked?.Invoke(other);
        }
    }
}