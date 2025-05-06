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
    public float reboundVelocity;

    // 跟踪合并状态全局
    private static HashSet<int> bubblesBeingMerged = new HashSet<int>();

    // 跟踪当前帧已处理的泡泡碰撞
    private static HashSet<int> handledBubbleCollisions = new HashSet<int>();

    // 注册清理标记
    private static bool isCleanupRegistered = false;

    // 标记泡泡是否正在被销毁
    public bool isBeingDestroyed = false;


    public AudioSource mergeAudio;

    public override void Awake()
    {
        base.Awake();
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/Bubble/BigBubble.prefab").Completed += OnPrefabLoaded;

        if (!isCleanupRegistered)
        {
            isCleanupRegistered = true;
            Application.quitting += () =>
            {
                bubblesBeingMerged.Clear();
                handledBubbleCollisions.Clear();
            };
        }
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

        // 每隔一帧清理已处理的碰撞
        if (Time.frameCount % 2 == 0)
        {
            handledBubbleCollisions.Clear();
        }
    }

    void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            bigBubblePrefab = handle.Result;
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        // 如果已经在销毁过程中，跳过所有碰撞处理
        if (isBeingDestroyed)
            return;

        base.OnCollisionEnter2D(other);

        int myID = GetInstanceID();
        if (bubblesBeingMerged.Contains(myID))
            return;

        if (other.gameObject.TryGetComponent<SmallBubble>(out var otherBubble))
        {
            int otherID = otherBubble.GetInstanceID();

            if (bubblesBeingMerged.Contains(otherID) || otherBubble.isBeingDestroyed)
                return;

            if (myID < otherID && !handledBubbleCollisions.Contains(myID) && !handledBubbleCollisions.Contains(otherID))
            {
                bubblesBeingMerged.Add(myID);
                bubblesBeingMerged.Add(otherID);
                handledBubbleCollisions.Add(myID);
                handledBubbleCollisions.Add(otherID);

                if (bigBubblePrefab != null)
                    MergeToBigBubble(otherBubble);
                else
                    collisionActions.Enqueue(() => MergeToBigBubble(otherBubble));
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Item") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            float angle = Vector2.Angle(Vector2.up, (other.transform.position - transform.position).normalized);
            if (angle <= 60f && !isBeingDestroyed)
            {
                isBeingDestroyed = true;
                other.gameObject.GetComponent<Rigidbody2D>().linearVelocityY = reboundVelocity;
                bubblesBeingMerged.Remove(GetInstanceID());
                handledBubbleCollisions.Remove(GetInstanceID());
                SafeDestroyBubble();
            }
        }
    }

    private void SafeDestroyBubble()
    {
        if (!isBeingDestroyed)
            return;

        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return null;
        if (this != null && gameObject != null)
        {
            BubbleQueue.DestroyBubble(gameObject);
        }
    }

    void MergeToBigBubble(SmallBubble otherBubble)
    {
        try
        {
            if (otherBubble == null || this == null || isBeingDestroyed || otherBubble.isBeingDestroyed)
                return;

            isBeingDestroyed = true;
            otherBubble.isBeingDestroyed = true;

            Vector3 centerPosition = (transform.position + otherBubble.transform.position) / 2;

            var bigBubble = Instantiate(bigBubblePrefab, centerPosition, Quaternion.identity);
            if (bigBubble != null)
            {
                Vector2 averageVelocity = (rb.linearVelocity + otherBubble.rb.linearVelocity) / 2;
                bigBubble.GetComponent<BigBubble>().initSpeed = averageVelocity;
            }

            BubbleQueue.DestroyBubble(otherBubble.gameObject, true);
            BubbleQueue.DestroyBubble(gameObject, true);

            mergeAudio.Play();
        }
        finally
        {
            bubblesBeingMerged.Remove(GetInstanceID());
            if (otherBubble != null)
                bubblesBeingMerged.Remove(otherBubble.GetInstanceID());
        }
    }

    public override void SwallowObject(GameObject other)
    {
        if (isBeingDestroyed)
            return;

        if (other.TryGetComponent<EnemyFSM>(out var e))
        {
            if (e.somatoType == EnemyFSM.EnemySomatoType.Heavy)
            {
                isBeingDestroyed = true;
                BubbleQueue.DestroyBubble(gameObject);
            }
            else
                base.SwallowObject(other);
        }
    }

    void OnDestroy()
    {
        bubblesBeingMerged.Remove(GetInstanceID());
        handledBubbleCollisions.Remove(GetInstanceID());
    }
}