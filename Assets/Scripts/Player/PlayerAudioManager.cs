using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource dieAudio;
    public AudioSource winAudio;
    public AudioSource walkAudio;
    public AudioSource blowAudio;
    public AudioSource shootAudio;
    public AudioSource underAttackAudio;
    PlayerFSM playerFSM;

    void Start()
    {
        playerFSM = GetComponent<PlayerFSM>();
        playerFSM.delegateParam.onDie += () =>
        {
            if (!dieAudio.isPlaying)
                dieAudio.Play();
        };

        playerFSM.delegateParam.onBlowBubble += () =>
        {
            if (!shootAudio.isPlaying)
                shootAudio.Play();
        };
    }
}
