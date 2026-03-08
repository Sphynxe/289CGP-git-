using UnityEngine;
using System.Collections;

public class humanfamily : MonoBehaviour
{

    public float speed = 0.1f;
    public float minWalkTime = 3f;
    public float maxWalkTime = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveDirection;
    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    GameObject gain1000Prefab;
    [SerializeField] 
    AudioClip collectSound;

    private Vector2[] directions = {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right,
        new Vector2(1, 1).normalized,
        new Vector2(-1, 1).normalized,
        new Vector2(1, -1).normalized,
        new Vector2(-1, -1).normalized
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();


        StartCoroutine(ContinuousWander());
    }

    IEnumerator ContinuousWander()
    {
        while (true)
        {

            ChangeDirection();

            float waitTime = Random.Range(minWalkTime, maxWalkTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void ChangeDirection()
    {

        moveDirection = directions[Random.Range(0, directions.Length)];


        if (anim != null)
        {
            anim.SetFloat("moveX", moveDirection.x);
            anim.SetFloat("moveY", moveDirection.y);

            anim.SetBool("isMoving", true);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }

   private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("someone touch me");
        StopAllCoroutines();
        StartCoroutine(ContinuousWander());

        if (collision.gameObject.CompareTag("enemy"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Debug.Log("kill by robot");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(gain1000Prefab, transform.position, Quaternion.identity);

            AudioSource.PlayClipAtPoint(collectSound, transform.position);

            Debug.Log("collected by player");
            Destroy(gameObject);
        }
    }

}