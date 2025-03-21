using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public LayerMask deadlyLayers;
    PlayerFSM playerFSM;
    public float minAngle = 45f;
    // public float wallThreshold = 0.2f;
    public float groundThreshold = 0.8f;

    void Start()
    {
        playerFSM = GetComponent<PlayerFSM>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & deadlyLayers) != 0)
        {
            playerFSM.Die();
        }

        if (other.gameObject.TryGetComponent<AggressiveEnemy>(out var a))
        {
            playerFSM.Die();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && playerFSM.currentState == playerFSM.state[PlayerStateType.Jump])
        {
            // float angle = Vector2.Angle(Vector2.left * transform.localScale.x, other.contacts[0].normal);
            if (other.contacts[0].normal.y >= groundThreshold)
            {
                playerFSM.ChangeState(PlayerStateType.Idle);
            }
        }

        if (other.gameObject.TryGetComponent<BigBubble>(out var b))
        {
            if (playerFSM.currentState == playerFSM.state[PlayerStateType.Jump])
            {
                playerFSM.ChangeState(PlayerStateType.Idle);
            }
        }

    }
}