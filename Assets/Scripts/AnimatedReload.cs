using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedReload : MonoBehaviour
{
    public GunScript gunScript;
    

    void AddBullet()
    {
        gunScript.AddBullet();
    }

    void roll()
    {
        gunScript.Roll();
    }
}
