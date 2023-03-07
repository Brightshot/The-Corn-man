using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CornPickup : MonoBehaviour
{
    public AudioClip Clip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlaySound(Clip);
            CornScript.instance.Collected();
            Destroy(gameObject);
        }
    }

    void PlaySound(AudioClip clip)
    {
        var sound = new GameObject(clip.name).AddComponent<AudioSource>();
        sound.clip = clip;
        sound.Play();
        sound.AddComponent<DestroyAfterSound>();
    }
}
