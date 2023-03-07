using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CornManager : MonoBehaviour
{
    public int maxCorn;
    public static int max;
    public static List<GameObject> corns = new List<GameObject>();

    public Slider PlantValue;
    public static int InfectedPlants;


    private void Update()
    {
        PlantValue.value = Mathf.MoveTowards(PlantValue.value, InfectedPlants, Time.deltaTime * 0.3f);
        InfectedPlants = Mathf.Clamp(InfectedPlants, 0, 10);
    }

    private void Start()
    {
        max = maxCorn;
    }
}
