using UnityEngine;

// 在玩家角色上挂载
public class PlayerCollisionHandler : MonoBehaviour
{
    public LayerMask deadlyLayers;
    PlayerFSM playerFSM;

    void Start()
    {
        playerFSM = GetComponent<PlayerFSM>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & deadlyLayers) != 0)
        {
            playerFSM.Die();
        }

        if (other.TryGetComponent<AggressiveEnemy>(out var a))
        {
            playerFSM.Die();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && playerFSM.currentState == playerFSM.state[PlayerStateType.Jump])
        {
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }
}