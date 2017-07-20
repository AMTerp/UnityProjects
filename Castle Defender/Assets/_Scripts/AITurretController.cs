using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurretController : MonoBehaviour {

    public float aiFireDelay;

    private GameObject currTarget;
    private GameObject[] enemies;
    private AILookX lookXScript;
    private AILookY lookYScript;
    private GunController gunController;

    void Awake()
    {
        lookXScript = GetComponent<AILookX>();
        lookYScript = GetComponent<AILookY>();
        gunController = transform.Find("AI Camera").GetChild(0).GetChild(0).gameObject.GetComponent<GunController>();
    }

    void Start()
    {
        gunController.firePause *= aiFireDelay;
    }

    void Update () {
        Debug.Log("Curr target: " + currTarget);
        Behavior();
	}

    void Behavior()
    {
        if (!currTarget)
        {
            currTarget = FindNearestEnemy();
            if (!currTarget)
            {
                // If there's STILL no target, try to reload.
                aiReload();
            }
        }
        else
        {
            // First ensure that the turret is looking at the target.
            lookXScript.LookAtTarget(currTarget.transform);
            lookYScript.LookAtTarget(currTarget.transform);

            if (gunController.currAmmoInClip > 0)
            {
                if (Time.time > gunController.nextFire)
                {
                    gunController.Fire();
                }
            }
            else
            {
                aiReload();
            }
        }
    }

    void aiReload()
    {
        if (gunController.currSpareAmmo > 0 && gunController.canFire && gunController.currAmmoInClip < gunController.magazineSize)
        {
            StartCoroutine(gunController.Reload());
        }
    }
	
	GameObject FindNearestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        GameObject currClosestEnemy = null;
        float tempDistance;

        foreach (GameObject enemy in enemies)
        {
            tempDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (tempDistance < minDistance)
            {
                currClosestEnemy = enemy;
                minDistance = tempDistance;
            }
        }

        return currClosestEnemy;
    }
}
