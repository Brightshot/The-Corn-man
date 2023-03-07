using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Rendering.DebugUI.Table;
using Unity.VisualScripting;

public class GunScript : MonoBehaviour
{
    public Transform Gun;
    public float MaxDistance;
    public ParticleSystem ShotVfx;
    [Space]
    public int reloadAmmo=24;
    public int currentAmmo=6;
    private int MaxAmmo=6;
    [Header("UI"), Space(5)]
    public TextMeshProUGUI CurrentAmmoText;
    public TextMeshProUGUI ReloadAmmoText;
    public TextMeshProUGUI ReloadingText;
    [Space]
    public Animator GunAnim;
    public Light P_light;
    [Space(5),Header("AIM")]
    public Transform n_position;
    public Transform a_position;
    [Space]
    public Image crosshair;
    [Space(5),Header("SOUND")]
    public AudioClip ShotSound;
    public AudioClip CockSound;     //bullet cocking sound
    public AudioClip spinSound;     //revolver barrel spin sound
    public AudioClip drySound;      


    public CamScript cam;

    private void OnEnable()
    {
        EventManager.Shot += fire;
        EventManager.PlayerDead+= Die;
    }

    private void OnDisable()
    {
        EventManager.Shot -= fire;
        EventManager.PlayerDead -= Die;
    }

    void Die()
    {
        Destroy(this);
    }

    bool firing, canfire = true;
    bool reloading;
    bool can_reload = true;
    private void fire()
    {
        if (reloading||(currentAmmo==0)) return;
        ShotVfx.Play();
        StartCoroutine(VFX());
        GunAnim.SetTrigger("Shoot");
        PlaySound(ShotSound);
        currentAmmo--;

        StartCoroutine(fired());
        StartCoroutine(reset());
    }

    IEnumerator VFX()
    {
        P_light.intensity = 1;
        yield return new WaitForSeconds(0.1f);
        P_light.intensity = 0;
    }

    int ammoneeded;
    IEnumerator Reload()
    {

        can_reload= false;
        yield return null;
        ammoneeded = MaxAmmo - currentAmmo;

        if (!reloading)
        {
            StopAllCoroutines();
        }

            GunAnim.SetTrigger("reload");
            GunAnim.ResetTrigger("Shoot"); 
           

        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => (currentAmmo == MaxAmmo));

        reloading = false;
        can_reload = true;
    }

    public void AddBullet()
    {
        currentAmmo += 1;
        reloadAmmo -= 1;
        PlaySound(CockSound);
    }

    public void Roll()
    {
        PlaySound(spinSound);
    }

    private void Update()
    {

        if (Time.deltaTime <= 0) return;

        GunUI();

        if (reloading)
        {
            cam.reloading();
        }

        if(currentAmmo == MaxAmmo || reloadAmmo == 0)
        {
            reloading = false;
        }

        currentAmmo = Mathf.Clamp(currentAmmo, 0, 6);

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (can_reload && (currentAmmo != 6))
            {
                reloading = true;
                StartCoroutine(Reload());
            }
        }

        GunAnim.SetBool("reloading", reloading);
        if (currentAmmo <= 0)
        {
            if (can_reload)
            {
                reloading = true;
                StartCoroutine(Reload());
            }
        }

        if (reloading)
        {
            ReloadingText.color = Color.white;
            ReloadingText.text = "RELOADING";
        }
        else
        {
            if(currentAmmo > 0)
            {
                ReloadingText.text = "";
            }
        }

        if(reloadAmmo <= 0)
        {
            can_reload = false;
        }
        else
        {
            if (!reloading)
            {
                can_reload= true;
            }
        }

        if (!reloading)
        {
            if (Input.GetMouseButton(1) && Time.timeScale > 0)
            {
                Gun.position = a_position.position;
                EventManager.Instance.Aiming();
                crosshair.color = new Color(0,0,0,0);
            }
            else
            {
                Gun.position = n_position.position;
            }
        }
        else
        {
            Gun.position = n_position.position;
        }

        if (Input.GetMouseButtonDown(0) && !reloading && (currentAmmo!=0) && canfire && Time.timeScale>0) 
        {
            EventManager.Instance.Shoot();
        }

        if(Input.GetMouseButtonDown(0) && reloadAmmo <= 0 && currentAmmo <= 0)
        {
            PlaySound(drySound);
            ReloadingText.color = Color.red;
            ReloadingText.text = "NO AMMO";
        }


        var distance = (Input.GetMouseButton(1)) ? (MaxDistance + 5) : MaxDistance;


        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, distance))
        {
           var destructable = hit.collider.GetComponent<IDestructable>();   //Get interface 'IDestructable' from Object looked at
            if (destructable != null)   //if Object has interface 'IDestructable'
            {
                if (firing)
                {
                    destructable.Destroyed();
                }
                destructable.Aimed();
                if (Input.GetMouseButton(1)) return;
                    crosshair.color = Color.red;
            }
            else
            {
                if (Input.GetMouseButton(1)) return;
                    crosshair.color = Color.white;
            }
        }
        else
        {
            if (Input.GetMouseButton(1)) return;
                crosshair.color = Color.white;
        }
        
    }

    IEnumerator fired()
    {
        firing = true;
        yield return new WaitForSeconds(0.1f);
        firing = false;
    }

    IEnumerator reset()
    {
        canfire = false;
        yield return new WaitForSeconds(0.3f);
        canfire= true;
    }

    void PlaySound(AudioClip clip)
    {
        var sound = new GameObject(clip.name).AddComponent<AudioSource>();
        sound.clip = clip;
        sound.outputAudioMixerGroup = AudioManager.mix;
        var pitch = Random.Range(0.8f, 1.2f);
        sound.pitch = pitch;
        sound.Play();
        sound.transform.parent= transform;
        sound.AddComponent<DestroyAfterSound>();
    }

    private void GunUI()
    {
        ReloadAmmoText.text = " / " + reloadAmmo.ToString("00");
        CurrentAmmoText.text= currentAmmo.ToString("00");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            if(reloadAmmo < 24)
            {
                PlaySound(CockSound);
                reloadAmmo += 24;
                reloadAmmo = Mathf.Clamp(reloadAmmo, 0, 24);
                Destroy(other.gameObject);
            }
        }
    }
}
