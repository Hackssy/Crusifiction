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

    //burst
    public int BulletsPerBurst = 3;
    public int BurstBulletsLeft;

    //spread
    public float SpreadIntensity;

    //bullet prop
    public GameObject bulletprefab;
    public Transform BulletSpawn;
    public float bulletVelocity = 30;
    public float bulletprefablifetime = 3f;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readytoShoot = true;
        BurstBulletsLeft = BulletsPerBurst;
    }

    void Update()
    {
        if(currentShootingMode == ShootingMode.Auto)
        {
            //holding down left mouse
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(currentShootingMode == ShootingMode.Single ||
            currentShootingMode == ShootingMode.Burst)
        {
            //clicking leftmouse 1
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readytoShoot & isShooting)
        {
            BurstBulletsLeft = BulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        readytoShoot = false;

        Vector3 ShootingDirection = CalculateDirectionAndSpread().normalized;

        //make bullet shoot
        GameObject bullet = Instantiate(bulletprefab, BulletSpawn.position, Quaternion.identity);

        //pointing the bullet the right direction
        bullet.transform.position = ShootingDirection;

        //shoot bullet
        bullet.GetComponent<Rigidbody>().AddForce(ShootingDirection * bulletVelocity, ForceMode.Impulse);

        //remove da bullet
        StartCoroutine(DestroybulletAfterTime(bullet, bulletprefablifetime));


        //checking if we are done shooting
        if (AllowReset)
        {
            Invoke("ResetShot", shootingdelay);
            AllowReset = false;
        }


        //burst mode
        if (currentShootingMode == ShootingMode.Burst && BurstBulletsLeft > 1) //we alr shot once before dis
        {
            BurstBulletsLeft--;
            Invoke("FireWeapon", shootingdelay);
        }
    }


    private void ResetShot()
    {
       readytoShoot = true;
        AllowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //shoot from middle of screen
        Ray Ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetpoint;
        if (Physics.Raycast(Ray, out hit))
        {
            //hitting something
            targetpoint = hit.point;
        }
        else
        {
            //shooting at the target
            targetpoint = Ray.GetPoint(100);
        }

        Vector3 direction = targetpoint - BulletSpawn.position;

        float x = UnityEngine.Random.Range(-SpreadIntensity, SpreadIntensity);
        float y = UnityEngine.Random.Range(-SpreadIntensity, SpreadIntensity);

        // Returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroybulletAfterTime(GameObject bullet, float Delay)
    {
        yield return new WaitForSeconds(Delay);
        Destroy(bullet);
    }
}
