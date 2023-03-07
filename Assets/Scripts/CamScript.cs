using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CamScript : MonoBehaviour
{
    public GameObject Gun;
    public float ShakeRange = 2;
    public float Speed=10;
    public int cycles=3;
    public bool shaking;
    private Vector3 shakeOffset=Vector3.zero;
    Vector3 statpos;

    public Zoom zoom;

    private void Start()
    {
        statpos = transform.localPosition;
        shakeOffset = statpos;
    }

    private void OnEnable()
    {
        EventManager.Shot += ShotAction;
        EventManager.PlayerDead += DeathAction;
        EventManager.Aim += aim;
        EventManager.PlayerDead += Die;
        EventManager.CameraShake += CamShake;
    }

    private void OnDisable()
    {
        EventManager.Shot -= ShotAction;
        EventManager.PlayerDead -= DeathAction;
        EventManager.Aim -= aim;
        EventManager.PlayerDead -= Die;
        EventManager.CameraShake -= CamShake;
    }

    void Die()
    {
        Destroy(this);
    }

    void DeathAction()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        transform.parent = null;
        gameObject.AddComponent<BoxCollider>();
        Gun.AddComponent<BoxCollider>();
        var rb =  gameObject.AddComponent<Rigidbody>();
        Gun.AddComponent<Rigidbody>();
        rb.angularDrag = 1f;
        rb.AddTorque(transform.right * 1,ForceMode.Impulse);
        Destroy(this);
    }

    void aim()
    {
        zoom.currentZoom = 0.7f;
    }

    public void reloading()
    {
        zoom.currentZoom = 0.0f;
    }

    private void Update()
    {
        if (Time.deltaTime <= 0) return;

        if (Input.GetMouseButton(1))
        {
            
        }
        else
        {
            zoom.currentZoom = 0.0f;
        }

        if (shaking)
        {
            pos1 = Vector3.MoveTowards(shakeOffset, new Vector3(x, y, z), Time.deltaTime * Speed);
            transform.localPosition = shakeOffset;
            shakeOffset = pos1;
        }
        else
        {
            transform.localPosition = statpos;
            shakeOffset= statpos;
        }
    }

    void CamShake(float perlin)
    {
        StartCoroutine(Shake(perlin));
    }

    void ShotAction()
    {
        StartCoroutine(Shake(ShakeRange));
    }

    float x, y, z;
    Vector3 pos1;
    IEnumerator Shake(float shake)
    {
        shaking= true;
        for (int i = 0; i < cycles; i++)
        {
            x = Random.Range(-shake, shake);
            y = Random.Range(-shake, shake) + statpos.y;
            z = 0;
            yield return new WaitUntil(() => Vector3.Distance(pos1, new Vector3(x, y, z)) <= 0.01f);
        }
        x = statpos.x;
        y = statpos.y ;
        z = statpos.z;
        yield return new WaitUntil(() => Vector3.Distance(pos1, new Vector3(x, y, z)) <= 0.01f);
        shaking = false;
    }
}
