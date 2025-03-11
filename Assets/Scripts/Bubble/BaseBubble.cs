using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BaseBubble : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D colliders;
    public GameObject swallowedObject;
    public LayerMask destoryLayer;
    public AudioSource destoryAudio;
    public Vector2 initSpeed;
    public virtual void Awake()
    {
        colliders = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = initSpeed;
        destoryAudio = GetComponent<AudioSource>();
        Addressables.LoadAssetAsync<AudioClip>("Assets/Sound/bubble7.mp3").Completed += OnAudioClipLoaded;
    }

    public virtual void Start()
    {
        PlayerFSM.Instance.param.existingBubble.Enqueue(gameObject);
    }


    public virtual void Update()
    {

    }

    public virtual void Break()
    {
        if (swallowedObject != null)
            swallowedObject?.GetComponent<SwallowedObject>().OnBreak(this);
        swallowedObject = null;
        animator.Play("bomb");
        colliders.enabled = false;
        destoryAudio.Play();
        Destroy(gameObject, 0.4f);
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SwallowedObject>() != null && swallowedObject == null)
        {
            other.gameObject.GetComponent<SwallowedObject>().OnLoad(this);
            swallowedObject = other.gameObject;
        }

        if (((1 << other.gameObject.layer) & destoryLayer) != 0)
        {
            Break();
        }
    }

    void OnAudioClipLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            destoryAudio.clip = handle.Result;
    }
}