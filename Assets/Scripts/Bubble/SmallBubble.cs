using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class SmallBubble : BaseBubble
{
    public GameObject bigBubblePrefab;
    Queue<Action> collisionActions = new();
    public override void Awake()
    {
        base.Awake();
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/Bubble/BigBubble.prefab").Completed += OnPrefabLoaded;
    }
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (bigBubblePrefab != null)
        {
            while (collisionActions.Count > 0)
                collisionActions.Dequeue()();
        }
    }

    void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            bigBubblePrefab = handle.Result;
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.TryGetComponent<SmallBubble>(out var otherBubble))
        {
            if (GetInstanceID() < otherBubble.GetInstanceID())
            {
                if (bigBubblePrefab != null)
                    MergeToBigBubble(otherBubble);
                else
                    collisionActions.Enqueue(() => MergeToBigBubble(otherBubble));
            }
        }
    }

    void MergeToBigBubble(SmallBubble otherBubble)
    {
        PlayerFSM.Instance.param.existingBubble.DestroyBubble(otherBubble.gameObject);

        var bigBubble = Instantiate(bigBubblePrefab, (transform.position + otherBubble.transform.position) / 2, Quaternion.identity);
        // PlayerFSM.Instance.param.existingBubble.Enqueue(bigBubble);
        bigBubble.GetComponent<BigBubble>().initSpeed = (rb.linearVelocity + otherBubble.rb.linearVelocity) / 2;

        PlayerFSM.Instance.param.existingBubble.DestroyBubble(gameObject);
    }
}
