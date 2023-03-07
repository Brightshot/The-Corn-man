using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantHealer : MonoBehaviour
{
    public TextMeshProUGUI Prompt;
    public TextMeshProUGUI healtext;
    public int Plantsfixed;
    public Image fixUI;

    public float maxDelay = 5;
    float time=5;

    public AudioClip healthAudio;

    IInteractable interactable;
    EnemySpawner Object;
    private void Start()
    {
        time = maxDelay;
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
        healtext.text = "";
        Destroy(fixUI);
        Destroy(healtext);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawner"))
        {
            interactable = other.GetComponent<IInteractable>();
            Object = other.GetComponent<EnemySpawner>();
            if (interactable != null)
            {
                Prompt.text = interactable.prompt();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Spawner"))
        {
            interactable = other.GetComponent<IInteractable>();
            Object = other.GetComponent<EnemySpawner>();
            if (interactable != null)
            {
                Prompt.text = interactable.prompt();
            }
        }
    }

    void Update()
    {
        fixUI.fillAmount = 1 - (time / 5);

        healtext.text = Plantsfixed.ToString();

        if(interactable != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                time -= Time.deltaTime;
                time = Mathf.Clamp(time, 0, maxDelay);
            }
            else
            {
                time += Time.deltaTime * 2;
                time = Mathf.Clamp(time, 0, maxDelay);
            }
        }

        if (time <= 0)
        {
            time = maxDelay;
            EventManager.Instance.StartHeal(Object.id);
            interactable.interact();
            interactable = null;
            AudioManager.PlaySound(gameObject, healthAudio, 1, false);
            Prompt.text = "";
            Plantsfixed += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spawner"))
        {
            interactable = null;
            Prompt.text = "";
            time = maxDelay;
        }
    }
}
