using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string scene;
    public float Delay;

    public void change(float delay)
    {
        Delay = delay;
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSecondsRealtime(Delay);
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
