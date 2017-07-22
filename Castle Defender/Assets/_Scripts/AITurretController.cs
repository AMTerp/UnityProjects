using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurretController : MonoBehaviour {

    public float aiFireDelay;
    public float gateBias;

    private Transform aiCameraTransform;
    private GameObject currTarget;
    private GameObject[] enemies;
    private AILookX lookXScript;
    private AILookY lookYScript;
    private int currTargetObjectID;
    private GunController gunController;
    private GameObject gate;
    private bool targetInCrosshair;

    void Awake()
    {
        aiCameraTransform = transform.Find("AI Camera");
        lookXScript = GetComponent<AILookX>();
        lookYScript = GetComponent<AILookY>();
        gunController = aiCameraTransform.GetChild(0).GetChild(0).gameObject.GetComponent<GunController>();
        gate = GameObject.FindWithTag("Gate");
    }

    void Start()
    {
        gunController.firePause *= aiFireDelay;
        targetInCrosshair = false;
    }

    void Update () {
        Behavior();
	}

    void Behavior()
    {
        if (!currTarget)
        {
            targetInCrosshair = false;
            currTargetObjectID = -1;
            currTarget = FindNearestEnemy();
            if (!currTarget)
            {
                // If there's STILL no target, try to reload.
                aiReload();
            }

            // Regardless of whether or not a new target has been found, if the current 
            // magazine is empty, attempt to reload.
            if (gunController.currAmmoInClip == 0)
            {
                aiReload();
            }
        }
        else
        {
            // First, look towards the target.
            lookXScript.LookAtTarget(currTarget.transform);
            lookYScript.LookAtTarget(currTarget.transform);

            if (targetInCrosshair)
            {
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
            else
            {
                targetInCrosshair = IsTargetInCrosshair();
            }

        }
    }

    bool IsTargetInCrosshair()
    {
        RaycastHit crosshairTarget;
        if (Physics.Raycast(aiCameraTransform.position, aiCameraTransform.forward, out crosshairTarget))
        {
            if (crosshairTarget.collider.gameObject.CompareTag("Enemy") && 
                crosshairTarget.collider.gameObject.transform.parent.GetComponent<EnemyController>().objectID == currTargetObjectID)
            {
                return true;
            }
        }

        return false;
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
        Vector3 gateTurretMidPoint = Vector3.Lerp(transform.position, gate.transform.position, gateBias);
        GameObject currClosestEnemy = null;
        float tempDistance;

        foreach (GameObject enemy in enemies)
        {
            tempDistance = Vector3.Distance(gateTurretMidPoint, enemy.transform.position);
            if (tempDistance < minDistance)
            {
                currClosestEnemy = enemy;
                minDistance = tempDistance;
            }
        }

        // If enemy found, set the target object ID.
        if (currClosestEnemy)
            currTargetObjectID = currClosestEnemy.GetComponent<EnemyController>().objectID;

        return currClosestEnemy;
    }
}
