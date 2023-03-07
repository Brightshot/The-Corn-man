using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
   public static EventManager Instance { get; private set; }

    #region events

    public delegate void shoot();
    public static event shoot Shot;

    public delegate void aim();
    public static event aim Aim;

    public delegate void dead();
    public static event dead PlayerDead;

    public delegate void shake(float range);
    public static event shake CameraShake;

    public delegate void heal(int id);
    public static event heal Heal;

    #endregion

    public void StartHeal(int id)
    {
        Heal?.Invoke(id);
    }

    public void Shake(float Range)
    {
        CameraShake?.Invoke(Range);
    }

    public void Die()
    {
        PlayerDead?.Invoke();
    }

    public void Shoot()
    {
        Shot?.Invoke();
    }

    public void Aiming()
    {
        Aim?.Invoke();
    }

    private void Awake()
    {
        Instance = this; 
    }
}
