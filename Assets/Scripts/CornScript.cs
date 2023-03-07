using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CornScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static CornScript instance;
    public int collected;

    public void Awake()
    {
        instance = this;
    }

    public void Collected()
    {
        collected += 1;
    }

    private void Update()
    {
        text.text = collected.ToString();
    }
}
