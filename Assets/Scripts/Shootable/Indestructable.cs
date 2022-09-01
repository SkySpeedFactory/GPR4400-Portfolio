using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indestructable : ShootableObjects
{
    public UnityEngine.GameObject particlesPrefab;
    public UnityEngine.GameObject impactPrefab;
    public override void OnHit(RaycastHit hit)
    {
        UnityEngine.GameObject particles = Instantiate(particlesPrefab, hit.point, hit.transform.rotation);
        UnityEngine.GameObject holes = Instantiate(impactPrefab, hit.point, hit.transform.rotation);
        Destroy(particles, 2);
        Destroy(holes, 10);
        print("Indestructible");
    }
}
