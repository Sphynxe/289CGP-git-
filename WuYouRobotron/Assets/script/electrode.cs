using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electrode : MonoBehaviour
{
    [SerializeField]
    GameObject explosionPrefab;

    [SerializeField] private AudioClip explosionSound;


    private void OnTriggerEnter2D(Collider2D theOtherCollider)
    {
        Debug.Log("someone touch me");

        if (theOtherCollider.gameObject.CompareTag("Projectile"))
        {
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Debug.Log("player shoot me");
            Destroy(gameObject);
        }

    }


}