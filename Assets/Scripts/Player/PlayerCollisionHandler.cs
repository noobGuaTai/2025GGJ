using UnityEngine;

// 在玩家角色上挂载
public class PlayerCollisionHandler : MonoBehaviour
{
    public LayerMask deadlyLayers;
    PlayerFSM playerFSM;
    public float minAngle = 45f;

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
            float angle = Vector2.Angle(Vector2.left * transform.localScale.x, other.contacts[0].normal);
            if (angle >= minAngle)
            {
                playerFSM.ChangeState(PlayerStateType.Idle);
            }
        }
    }
}