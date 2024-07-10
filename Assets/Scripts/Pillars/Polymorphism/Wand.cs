using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    public bool canFire = false;
    private readonly float fireRate = 0.5f;
    private float lastShot;

    public void Fire()
    {
        if (!canFire)
        {
            return;
        }
        if (Time.time > lastShot + fireRate)
        {
            GameObject instantiated = Instantiate(bullet, firePoint.position, firePoint.rotation);
            Destroy(instantiated, 3f);
            lastShot = Time.time;
        }
    }
    public void SetCanFire(bool canFire)
    {
        this.canFire = canFire;
    }
}
