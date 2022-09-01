using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : ShootableObjects
{
    public UnityEngine.GameObject particlesPrefab;
    public override void OnHit(RaycastHit hit)    {
        
        Instantiate(particlesPrefab, hit.point, hit.transform.rotation);
        Destroy(gameObject,1f);
        print("Enemy");
    }
    
}
