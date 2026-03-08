using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float speed = 2.0f; 
    
    private Transform playerTransform;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            
            rb.linearVelocity = direction * speed;

          
            if (anim != null)
            {
                anim.SetFloat("moveX", direction.x);
                anim.SetFloat("moveY", direction.y);
                anim.SetBool("isMoving", true);
            }
        }
        else
        {
           
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetBool("isMoving", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}