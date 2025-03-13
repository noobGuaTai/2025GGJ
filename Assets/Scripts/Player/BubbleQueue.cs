using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BubbleQueue
{
    public Queue<GameObject> bubbles = new();
    public int smallBubbleNums = 0;
    public int bigBubbleNums = 0;

    public void Enqueue(GameObject bubble)
    {
        bubbles.Enqueue(bubble);

        if (bubble.TryGetComponent<SmallBubble>(out var smallBubble))
            smallBubbleNums++;
        else if (bubble.TryGetComponent<BigBubble>(out var bigBubble))
            bigBubbleNums++;

        if (smallBubbleNums + bigBubbleNums * 2 > 4)
        {
            var latest = bubbles.Dequeue();
            DestroyBubble(latest);
        }

    }

    public GameObject Dequeue()
    {
        GameObject bubble = bubbles.Dequeue();

        if (bubble.TryGetComponent<SmallBubble>(out var b))
            smallBubbleNums--;
        else if (bubble.TryGetComponent<BigBubble>(out var bb))
            bigBubbleNums--;

        return bubble;
    }

    public void Clear()
    {
        while (bubbles.Count > 0)
        {
            bubbles.Dequeue().GetComponent<BaseBubble>().Break();
        }
        bubbles.Clear();
        smallBubbleNums = 0;
        bigBubbleNums = 0;
    }

    public void DestroyBubble(GameObject bubble)
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

        bubble.GetComponent<BaseBubble>().Break();
    }
}