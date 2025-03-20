using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BaseBubble : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D[] colliders;
    public GameObject swallowedObject;
    public LayerMask destoryLayer;
    public AudioSource destoryAudio;
    public Vector2 initSpeed;
    float swallowedObjectMass;
    public virtual void Awake()
    {
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = initSpeed;
        destoryAudio = GetComponent<AudioSource>();
        Addressables.LoadAssetAsync<AudioClip>("Assets/Sound/bubble7.mp3").Completed += OnAudioClipLoaded;

    }

    public virtual void Start()
    {
        BubbleQueue.Enqueue(gameObject);
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
        colliders.Any(c => c.enabled = false);
        destoryAudio.Play();
        Destroy(gameObject, 0.4f);
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        SwallowObject(other.gameObject);
        DestroyBubble(other.gameObject);
    }

    public virtual void SwallowObject(GameObject other)
    {
        if (other.TryGetComponent<SwallowedObject>(out var s) && swallowedObject == null)
        {
            s.OnLoad(this);
            swallowedObject = other;
            if (other.TryGetComponent<EnemyFSM>(out var e))
            {
                swallowedObjectMass = e.rb.mass;
                rb.mass += e.rb.mass;
                rb.gravityScale = 1;
            }
        }
    }

    public virtual void DestroyBubble(GameObject other)
    {
        if (((1 << other.layer) & destoryLayer) != 0)
        {
            BubbleQueue.DestroyBubble(gameObject);
        }
    }

    void OnAudioClipLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            destoryAudio.clip = handle.Result;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {

    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {

    }
}