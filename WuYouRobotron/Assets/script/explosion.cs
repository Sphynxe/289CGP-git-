using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    AudioSource audioSource;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //audioSource.Play();

        //Self-destruct in .5 seconds, which
        //gives the audio time to play
        Destroy(gameObject, 0.5f);
    }



}