using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision ObjWeHit)
    {
        if (ObjWeHit.gameObject.CompareTag("Target"))
        {

            CreateBulletImpactEffect(ObjWeHit);

            print("hit" +  ObjWeHit.gameObject.name + "!");
            Destroy(gameObject);
        }

        if (ObjWeHit.gameObject.CompareTag("Wall"))
        {

            CreateBulletImpactEffect(ObjWeHit);

            print("hit a wall");
            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision ObjWeHit)
    {
        ContactPoint contact = ObjWeHit.contacts[0];

        GameObject hole = Instantiate(
            globalReferences.Instance.bulletimpacteffectprefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(ObjWeHit.gameObject.transform);
    }

}
