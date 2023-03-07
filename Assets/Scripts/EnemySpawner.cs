using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemySpawner : MonoBehaviour,IInteractable
{
    public GameObject Enemy;
    public Vector3 SpawnSize;
    public bool Attacking;
    public bool CanAttack=true;
    private bool infected = false;
    public Material  mat;
    private ParticleSystem toxinSmoke;
    public int id;

    [Space]
    public float delay = 1;

    public Transform arrow;


    [ContextMenu("FIndArrow")]
    public void find()
    {
        arrow = transform.Find("DownArrow").transform;
    }

    [ContextMenu("CreateID")]
    void CreateId()
    {
        id = Random.Range(0, 999999999);
    }

    public void Looking()
    {

    }

    private void OnEnable()
    {
        EventManager.PlayerDead += die;
    }

    private void OnDisable()
    {
        EventManager.PlayerDead += die;
    }

    void die()
    {
        Destroy(gameObject);
    }

    public string prompt() => "Hold E to heal plants";
    public void interact()
    {
        Attacking = false;
        CanAttack = false;
        gameObject.tag = "empty";
        CornManager.InfectedPlants--;
    }

    void Start()
    {
        toxinSmoke= GetComponent<ParticleSystem>();
        if (Attacking)
        {
            
            StartCoroutine(Spawn());
        }
    }

    private void Update()
    {
        if (Attacking)
        {
            arrow.gameObject.SetActive(true);
            gameObject.tag = "Spawner";
            if (!toxinSmoke.isPlaying)
            {
                toxinSmoke.Play();
            }
            if (!infected)
            {
                infected= true;
                CornManager.InfectedPlants++;
            }
        }
        else
        {
            arrow.gameObject.SetActive(false);
            if (toxinSmoke.isPlaying)
            {
                toxinSmoke.Stop();
            }
        }

        if (Attacking&&CanAttack==true)
        {
            gameObject.tag = "Spawner";
            CanAttack= false;
            StartCoroutine(Spawn());
        }
        else if(!Attacking && CanAttack==false) 
        {
            CanAttack = true;
            StopAllCoroutines();
        }
    }

    IEnumerator Spawn()
    {
        float d = Random.Range(0.5f, 3);
        yield return new WaitForSeconds(d);
        var size = new Vector3
        {
            x = Random.Range(-SpawnSize.x / 2f, SpawnSize.x / 2f),
            y = Random.Range(-SpawnSize.y / 2f, SpawnSize.y / 2f),
            z = Random.Range(-SpawnSize.z / 2f, SpawnSize.z / 2f)
        };

        if (CornManager.corns.Count < CornManager.max)
        {
            var corn = Instantiate(Enemy, transform.position + size, Quaternion.identity);
            CornManager.corns.Add(corn);
        }
        if (Attacking)
        {
            StartCoroutine(Spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UFO"))
        {
            Attacking = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("UFO"))
        {
            Attacking = true;
        }
    }


    private void OnDrawGizmos()
    {
        Color col = Color.red;
        col.a = 0.3f;
        Gizmos.color = col;
        Gizmos.DrawCube(transform.position, SpawnSize);
    }
}
