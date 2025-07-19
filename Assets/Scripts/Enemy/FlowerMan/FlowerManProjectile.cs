using System.Collections;
using UnityEngine;

public class FlowerManProjectile : MonoBehaviour
{
    Tween tween;
    public GameObject[] flowers;
    public GameObject aim;
    public float speed;
    public GameObject father;
    void Start()
    {
        tween = gameObject.AddComponent<Tween>();
        tween.AddTween("rotate", x =>
        {
            transform.rotation = Quaternion.Euler(0, 0, x);
        }, 0, 360, 2f).Play();
        // Attack();
        // StartCoroutine(Attack());
        Destroy(gameObject, 2f);
    }

    // IEnumerator Attack()
    // {
    //     foreach (var flower in flowers)
    //     {
    //         flower.GetComponent<Flowers>().speed = (transform.position - aim.transform.position).normalized * speed;
    //         yield return new WaitForSeconds(0.2f);
    //     }
    // }
    void Attack()
    {
        var g = Instantiate(flowers[Random.Range(0, 5)], father.transform.position, Quaternion.identity);
        g.GetComponent<Rigidbody2D>().AddForce((father.transform.position - aim.transform.position).normalized * speed, ForceMode2D.Impulse);
    }
}
