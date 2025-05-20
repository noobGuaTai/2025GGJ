using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource dieAudio;
    public AudioSource winAudio;
    public AudioSource walkAudio;
    public AudioSource pushAudio;
    public AudioSource blowAudio;
    public AudioSource underAttackAudio;
    public AudioSource punchAudio;
    PlayerFSM playerFSM => PlayerFSM.Instance;

    void Start()
    {
        playerFSM.delegateParam.onDie += () =>
        {
            if (!dieAudio.isPlaying)
                dieAudio.Play();
        };

        playerFSM.delegateParam.onBlowBubble += () =>
        {
            if (!blowAudio.isPlaying)
                blowAudio.Play();
        };
    }
}
