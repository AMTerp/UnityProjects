using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float speed;
    public float attackPause;
    public float attackDamage;
    public float attackRange;
    public int moneyReward;
    public float checkpointRadius;
    public AudioClip gateAttackSound;
    public float gateAttackVolume;
    public AudioClip[] houseAttackSounds;
    public float houseAttackVolume;

    internal int objectID;

    private int numCheckpoints = 2;
    private int checkpoint;
    private float nextAttack;
    private Rigidbody rb;
    private GameObject gate;
    private GameObject houseTarget;
    private HouseController houseTargetScript;
    private Animation attackAnimation;
    private GateController gateController;
    private Transform postGateCheckpoint1;
    private Transform postGateCheckpoint2;
    private AudioSource enemySounds;

    private GameObject nwHouse;
    private GameObject neHouse;
    private GameObject swHouse;
    private GameObject seHouse;
    private GameObject[] houses;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        enemySounds = GetComponent<AudioSource>();

        gate = GameObject.FindWithTag("Gate");
        nwHouse = GameObject.FindWithTag("NW House");
        neHouse = GameObject.FindWithTag("NE House");
        swHouse = GameObject.FindWithTag("SW House");
        seHouse = GameObject.FindWithTag("SE House");
        houses = new GameObject[4];
        houses[0] = nwHouse;
        houses[1] = neHouse;
        houses[2] = swHouse;
        houses[3] = seHouse;

        postGateCheckpoint1 = gate.transform.GetChild(0);
        postGateCheckpoint2 = gate.transform.GetChild(1);
        gateController = gate.GetComponent<GateController>();
        attackAnimation = transform.GetChild(0).GetComponent<Animation>();
        nextAttack = 0.0f;

        checkpoint = (gate.transform.GetChild(numCheckpoints).gameObject.activeSelf) ? 0 : 1;

        objectID = (int) (Time.time * 100000);
    }

    // Update is called once per frame
    void Update() {
        // If gate is alive...
        if (checkpoint == 0)
        {
            if (gateController.health <= 0)
            {
                checkpoint++;
            }
            else if (Vector3.Distance(transform.position, gate.transform.position) > attackRange)
            {
                moveTowards(gate.transform);
            }
            else
            {
                rb.velocity = Vector3.zero;
                attackGate();
            }
        }
        // If gate is recently dead...
        else if (checkpoint == 1)
        {
            if (Vector3.Distance(transform.position, postGateCheckpoint1.position) > checkpointRadius)
            {
                moveTowards(postGateCheckpoint1);
            }
            else
            {
                checkpoint++;
            }
        }
        // If gate is dead and have gone to first checkpoint, go through gate
        else if (checkpoint == 2)
        {
            if (Vector3.Distance(transform.position, postGateCheckpoint2.position) > checkpointRadius)
            {
                moveTowards(postGateCheckpoint2);
            }
            else
            {
                checkpoint++;

                // Find the nearest house.
                houseTarget = findNearestHouse();
            }
        }
        // In castle, proceed to attack houses...
        else if (checkpoint == 3)
        {
            if (Vector3.Distance(transform.position, houseTarget.transform.position) > attackRange)
            {

                if (houseTargetScript.isAlive)
                {
                    moveTowards(houseTarget.transform);
                }
                else
                {
                    houseTarget = findNearestHouse();
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
                attackHouse();
            }
        }
    }

    // Function that finds and returns the nearest house to the enemy.
    GameObject findNearestHouse()
    {
        GameObject closestHouse = null;

        float closestDistance = 9999.9f;
        float tempDistance;
        foreach (GameObject house in houses)
        {
            tempDistance = Vector3.Distance(transform.position, house.transform.position);
            if (closestDistance > tempDistance && house.GetComponent<HouseController>().isAlive)
            {
                closestHouse = house;
                closestDistance = tempDistance;
            }
        }

        // Before returning, store its HouseController as well.
        houseTargetScript = closestHouse.GetComponent<HouseController>();
        Debug.Log("Closest house: " + closestHouse);
        return closestHouse;
    }

    void moveTowards(Transform target)
    {
        transform.LookAt(target);
        rb.velocity = transform.forward * speed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void attackHouse()
    {
        if (Time.time > nextAttack)
        {
            transform.LookAt(houseTarget.transform);

            attackAnimation.Play();
            enemySounds.PlayOneShot(houseAttackSounds[Random.Range(0, houseAttackSounds.Length)], houseAttackVolume);

            houseTargetScript.takeDamage(attackDamage);

            if (houseTargetScript.health <= 0)
            {
                houseTarget.transform.GetChild(0).gameObject.SetActive(false);
                houseTargetScript.isAlive = false;
                if (checkAllHousesDestroyed())
                {
                    Time.timeScale = 0;
                }
                else
                {
                    houseTarget = findNearestHouse();
                }
            }

            nextAttack = Time.time + attackPause;
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void attackGate()
    {
        if (Time.time > nextAttack)
        {
            transform.LookAt(gate.transform);

            attackAnimation.Play();
            enemySounds.PlayOneShot(gateAttackSound, gateAttackVolume);

            gateController.takeDamage(attackDamage);

            if (gateController.health <= 0)
            {
                gate.transform.GetChild(numCheckpoints).gameObject.SetActive(false);
            }

            nextAttack = Time.time + attackPause;
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    bool checkAllHousesDestroyed()
    {
        foreach (GameObject house in houses)
        {
            if (house.GetComponent<HouseController>().isAlive)
            {
                return false;
            }
        }

        return true;
    }
}
