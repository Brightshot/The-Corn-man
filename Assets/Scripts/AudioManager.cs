using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup mixer;
    public static AudioMixerGroup mix;
    // Start is called before the first frame update
    void Start()
    {
        mix = mixer;
    }

    public static void PlaySound(GameObject Sender,AudioClip clip, float vol, bool loop)
    {
        var sound = new GameObject(clip.name).AddComponent<AudioSource>();
        sound.clip = clip;
        sound.outputAudioMixerGroup = AudioManager.mix;
        var pitch = Random.Range(0.7f, 1.2f);
        sound.pitch = pitch;
        sound.loop = loop;
        sound.spatialBlend = 1f;
        sound.rolloffMode = AudioRolloffMode.Linear;
        sound.maxDistance = 20f;
        sound.volume = vol;
        sound.Play();
        sound.transform.parent = Sender.transform;
        sound.transform.localPosition = Vector3.zero;
        sound.AddComponent<DestroyAfterSound>();
    }
}
