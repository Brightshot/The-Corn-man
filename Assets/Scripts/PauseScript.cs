using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    bool Paused = false;
    public GameObject PausePanel;

    private void OnEnable()
    {
        EventManager.PlayerDead += die;
    }

    private void OnDisable()
    {
        EventManager.PlayerDead -= die;
    }

    void die()
    {
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Paused)
        {
            AudioManager.mix.audioMixer.SetFloat("Volume", -80f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                resume();
            }

        }
        else
        {
            AudioManager.mix.audioMixer.SetFloat("Volume", 0f);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pause();
            }
        }
    }

    public void pause()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
        Paused = true;
    }

    public void resume()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
        Paused = false;
    }
}


