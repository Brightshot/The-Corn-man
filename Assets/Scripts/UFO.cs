using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public Transform[] CornFields;

    public bool Moving;
    public ParticleSystem laser;
    public Light Light;

    public BoxCollider col;
    public GameObject volume;

    public float speed;
    public int currentID;

    public int mindelay, maxdelay;
    public AudioSource Laser;

    private void OnEnable()
    {
        EventManager.Heal += change;
        EventManager.PlayerDead += die;
    }

    private void OnDisable()
    {
        EventManager.Heal -= change;
        EventManager.PlayerDead -= die;
    }

    private void Start()
    {
        
        StartCoroutine(random());
    }

    void die()
    {
        Destroy(gameObject);
    }

    void change(int id)
    {
        if (id == currentID)
        {
            col.enabled = false;
            RandomTarget();
            Moving = true;
        }
    }

    IEnumerator random()
    {
        Moving = true;
        int d = Random.Range(mindelay, maxdelay);
        yield return new WaitForSeconds(d);
        RandomTarget();
        StartCoroutine(random());
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, CornFields[index].position, Time.deltaTime * speed);
            laser.Stop();
           Light.intensity= 0;
            col.enabled= false;
            volume.SetActive(false);
        }
        else
        {
            if (laser.isPlaying == false)
            {
                laser.Play();
            }
            if (Laser.isPlaying == false)
            {
                Laser.Play();
            }
            Light.intensity = 100;
            volume.SetActive(true);
            col.enabled = true;
        }

        currentID =  CornFields[index].GetComponentInChildren<EnemySpawner>().id;

        if (transform.position == CornFields[index].position)
        {
            Moving= false;
        }
    }

    int index;
    void RandomTarget()
    {
        index = Random.Range(0, CornFields.Length);
    }
}
