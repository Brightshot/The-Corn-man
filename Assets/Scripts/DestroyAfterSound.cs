using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DestroyAfterSound : MonoBehaviour
{
    AudioSource source;

    private void Start()
    {
        source= GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
