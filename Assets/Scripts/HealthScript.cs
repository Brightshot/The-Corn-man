using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthScript : MonoBehaviour
{
    public float CurrentHealth=100; //current health of player
    public Image blood;
    public Image HealthIcon;
    public TextMeshProUGUI HealthText;
    private Color bloodColor = Color.red;
    public AnimationCurve alphaCurve;

    public AudioClip hurtAudio;
    public AudioClip healthAudio;

    public GameObject DeathPanel;

    private void Start()
    {
        bloodColor.a = 0.00f;
    }


    private void Update()
    {
        if(CurrentHealth <= 0)
        {
            EventManager.Instance.Die();    //invoke the death event
            DeathPanel.SetActive(true);
            Destroy(gameObject);
        }
        blood.color = bloodColor;
        HealthText.text = CurrentHealth.ToString() + "%";
        HealthIcon.fillAmount = (CurrentHealth / 100);
    }
  
    public void attacked(float damage)
    {
        AudioManager.PlaySound(gameObject, hurtAudio, 1, false);
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, 100);
        bloodColor.a = 1 - alphaCurve.Evaluate((CurrentHealth / 100));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            if (CurrentHealth < 100)
            {
                AudioManager.PlaySound(gameObject,healthAudio,1,false);
                CurrentHealth += 20f;
                CurrentHealth = Mathf.Clamp(CurrentHealth, 0, 100);
                bloodColor.a = 1 - alphaCurve.Evaluate((CurrentHealth / 100));
                Destroy(other.gameObject);
            }
        }
    }
}
