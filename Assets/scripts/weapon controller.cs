using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponcontroller : MonoBehaviour
{
    public Camera PlayerCamera;

    //shooting
    public bool isShooting, readytoShoot;
    bool AllowReset = true;
    public float shootingdelay = 2f;









    public GameObject bulletprefab;
    public Transform BulletSpawn;
    public float bulletVelocity = 30;
    public float bulletprefablifetime = 3f;

    void Update()
    {
        //left mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        //make bullet shoot
        GameObject bullet = Instantiate(bulletprefab, BulletSpawn.position, Quaternion.identity);
        //shoot bullet
        bullet.GetComponent<Rigidbody>().AddForce(BulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        //remove da bullet
        StartCoroutine(DestroybulletAfterTime(bullet, bulletprefablifetime));




    }

    private IEnumerator DestroybulletAfterTime(GameObject bullet, float Delay)
    {
        yield return new WaitForSeconds(Delay);
        Destroy(bullet);
    }
}
