using UnityEngine;

public class TraderSwallowedObject : SwallowedObject
{
    public int health = 3;
    public GameObject coins;
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    public override void OnBreak(BaseBubble bubble)
    {
        base.OnBreak(bubble);
    }

    public override void OnLoad(BaseBubble bubble)
    {
        if (health > 0)
        {
            health--;
            bubble.Break();
            return;
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        base.OnLoad(bubble);
        GetComponent<DoorTrader>().deal = true;
        if (coins != null)
            coins.SetActive(true);
    }
}
