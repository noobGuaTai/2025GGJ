using System;
using System.Collections.Generic;
using UnityEngine;

public static class BubbleQueue
{
    public static Queue<GameObject> bubbles = new();
    public static int smallBubbleNums = 0;
    public static int bigBubbleNums = 0;

    public static void Enqueue(GameObject bubble)
    {
        bubbles.Enqueue(bubble);

        if (bubble.TryGetComponent<SmallBubble>(out var smallBubble))
            smallBubbleNums++;
        else if (bubble.TryGetComponent<BigBubble>(out var bigBubble))
            bigBubbleNums++;

        if (smallBubbleNums + bigBubbleNums * 2 > 4)
        {
            var latest = Dequeue();
            latest.GetComponent<BaseBubble>().Break();
        }
        Debug.Log($"smallBubbleNums: {smallBubbleNums}, bigBubbleNums: {bigBubbleNums}");
    }

    public static GameObject Dequeue()
    {
        GameObject bubble = bubbles.Dequeue();

        if (bubble.TryGetComponent<SmallBubble>(out var b))
            smallBubbleNums--;
        else if (bubble.TryGetComponent<BigBubble>(out var bb))
            bigBubbleNums--;

        return bubble;
    }

    public static void Clear()
    {
        while (bubbles.Count > 0)
        {
            bubbles.Dequeue().GetComponent<BaseBubble>().Break();
        }
        bubbles.Clear();
        smallBubbleNums = 0;
        bigBubbleNums = 0;
    }

    public static void DestroyBubble(GameObject bubble, bool isMerge = false)
    {
        Queue<GameObject> tempQueue = new Queue<GameObject>();
        while (bubbles.Count > 0)
        {
            GameObject current = bubbles.Dequeue();
            if (current != bubble)
            {
                tempQueue.Enqueue(current);
            }
        }
        bubbles = tempQueue;

        if (bubble.TryGetComponent<SmallBubble>(out var _))
            smallBubbleNums--;
        else if (bubble.TryGetComponent<BigBubble>(out var _))
            bigBubbleNums--;

        bubble.GetComponent<BaseBubble>().Break(isMerge);
        Debug.Log($"smallBubbleNums: {smallBubbleNums}, bigBubbleNums: {bigBubbleNums}");

    }
}