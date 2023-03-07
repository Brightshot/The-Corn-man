using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Crate : MonoBehaviour,IDestructable,IInteractable
{

    public GameObject CrateBreak;
    public GameObject[] items;

    public AudioClip CrateSound;
    CrateSpawner spawner;
    public int index;

    private void Start()
    {
        spawner = CrateSpawner.instance;
    }

    public void Destroyed()
    {
        Instantiate(CrateBreak,transform.position,Quaternion.identity);
        PlaySound(CrateSound);
        int index= Random.Range(0,items.Length-1);
        var pos = new Vector3(transform.position.x,transform.position.y - 0.6f,transform.position.z);
        Instantiate(items[index], pos, items[index].transform.rotation);
        Destroy(gameObject);
    }

    public void interact()
    {
        Instantiate(CrateBreak, transform.position, Quaternion.identity);
        PlaySound(CrateSound);
        int index = Random.Range(0, items.Length - 1);
        var pos = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
        Instantiate(items[index], pos, items[index].transform.rotation);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        spawner.RemoveCrate(index);
    }

    public void Looking()
    {

    }

    public string prompt() => "Press E to break Crate";

    public void Aimed()
    {
       
    }

    void PlaySound(AudioClip clip)
    {
        var sound = new GameObject(clip.name).AddComponent<AudioSource>();
        sound.transform.localPosition = transform.position;
        sound.clip = clip;
        sound.outputAudioMixerGroup = AudioManager.mix;
        sound.volume = 0.6f;
        sound.pitch = 0.5f;
        sound.Play();
        sound.AddComponent<DestroyAfterSound>();
    }
}
