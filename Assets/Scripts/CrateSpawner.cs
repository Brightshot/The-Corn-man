using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField]public List<crateinfo> Crates = new List<crateinfo>();
    public GameObject CratePrefab;

    public static CrateSpawner instance;

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        for (int i = 0; i < Crates.Count; i++)
        {
            if (Crates[i].empty == true)
            {
                Crate crate = Instantiate(CratePrefab, Crates[i].SpawnPoint.position, CratePrefab.transform.rotation).GetComponent<Crate>();
                crate.index = i;
                Crates[i].empty = false;
            }
        }
    }

    void CheckCrates()
    {
        for (int i = 0; i < Crates.Count; i++)
        {
            if (Crates[i].empty == true)
            {
                StartCoroutine(StartSpawn(Crates[i],i));
                Crates[i].empty = false;
            }
        }

    }



    IEnumerator StartSpawn(crateinfo info,int id)
    {
        return Spawn(info,id);
    }

    IEnumerator Spawn(crateinfo info,int id)
    {
        int delay = Random.Range(20,55);
        yield return new WaitForSeconds(delay);
        Crate crate = Instantiate(CratePrefab, info.SpawnPoint.position, CratePrefab.transform.rotation).GetComponent<Crate>();
        crate.index = id;
    }

    public void RemoveCrate(int index)
    {
        Crates[index].empty = true;
        CheckCrates();
    }

}



[System.Serializable]
public class crateinfo
{
    public bool empty;
    public Transform SpawnPoint;
}