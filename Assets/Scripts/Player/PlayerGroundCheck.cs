using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    PlayerFSM playerFSM;

    void Start()
    {
        playerFSM = GetComponent<PlayerFSM>();
    }

    void Update()
    {
        playerFSM.param.isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
