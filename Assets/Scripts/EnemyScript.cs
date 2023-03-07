using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static EventManager;

public class EnemyScript : MonoBehaviour,IDestructable
{
    public Transform target;
    public Transform spawnPoint;
    private NavMeshAgent agent;

    private HealthScript PlayerHealth;
    public Animator anim;

    private bool InRange;
    private bool attacking=false;
    public bool CanAttack=true;
    public float AttackPower = 5;
    public GameObject PopCorn;

    [Header("Sounds"),Space(10)]
    public AudioClip clip;
    public AudioClip death;
    public AudioClip Shout;
    public AudioClip ghost;
    public AudioClip kicksound;
    public AudioClip dance;
    public AudioClip zombieSound;



    public void Destroyed()
    {
        agent.speed = 0;
        agent.angularSpeed= 0;
        StartCoroutine(Death());
    }

    public void Aimed()
    {

    }

    bool alive = true;
    IEnumerator Death()
    {
        CanAttack= false;
        SkinnedMeshRenderer[] child = GetComponentsInChildren<SkinnedMeshRenderer>();

        //On death destroy all audiosources in children
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<AudioSource>() != null)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        //play deathSound
        if (alive)
        {
            AudioManager.PlaySound(gameObject,death, 1f, false);
            alive = false;
        }

        anim.enabled= false;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CapsuleCollider>());
        yield return new WaitForSeconds(1f);
        Instantiate(PopCorn, spawnPoint.position, Quaternion.identity);
        PlaySound();
        CornManager.corns.Remove(gameObject);
        Destroy(gameObject);
    }


    void Awake()
    {
        index = Random.Range(0, 6);
    }

    int index;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        switch (index)
        {
            case 0:
                AudioManager.PlaySound(gameObject,Shout, 1, true); agent.speed = 4f; break;
            case 1:
                AudioManager.PlaySound(gameObject, Shout, 1, true); agent.speed = 4f; break;
            case 2:
                AudioManager.PlaySound(gameObject,zombieSound, 1, true); agent.speed = 2.5f; break;
            case 3:
                AudioManager.PlaySound(gameObject,ghost, 1, true); agent.speed = 5f; break;
            case 4:
                AudioManager.PlaySound(gameObject,kicksound, 1, true); agent.speed = 6f; break;
            case 5:
                AudioManager.PlaySound(gameObject, dance, 1, true); agent.speed = 3f; break;
            case 6:
                AudioManager.PlaySound(gameObject, kicksound, 1, true); agent.speed = 6f; break;
        }

        anim.SetInteger("index", index);

        if(index == 2)
        {
           
        }

        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
        {
            Destroy(gameObject);
            CornManager.corns.Remove(gameObject);
        }
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScript>();
    }

    private void Update()
    {
        if (target == null)
        {
            Instantiate(PopCorn, spawnPoint.position, Quaternion.identity);
            PlaySound();
            Destroy(gameObject);
            CornManager.corns.Remove(gameObject);
        }

        agent.SetDestination(target.position);

        InRange =  (Vector3.Distance(transform.position, target.position) < 2f) ? true : false;

        if (InRange && CanAttack)
        {
            EventManager.Instance.Shake(0.6f);
            PlayerHealth.attacked(AttackPower);
            Instantiate(PopCorn, spawnPoint.position, Quaternion.identity);
            PlaySound();
            CornManager.corns.Remove(gameObject);
            Destroy(gameObject);
        }

        anim.SetFloat("velocity", agent.velocity.normalized.magnitude);
    }

    void PlaySound()
    {
        var sound = new GameObject(clip.name).AddComponent<AudioSource>();
        sound.transform.localPosition= transform.position;
        sound.clip = clip;
        sound.outputAudioMixerGroup = AudioManager.mix;
        sound.volume = 0.6f;
        sound.pitch = 0.5f;
        sound.spatialBlend = 1f;
        sound.rolloffMode = AudioRolloffMode.Linear;
        sound.maxDistance = 30f;
        sound.Play();
        sound.AddComponent<DestroyAfterSound>();
    }

}
