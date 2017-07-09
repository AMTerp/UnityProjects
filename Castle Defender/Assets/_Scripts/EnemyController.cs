using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float speed;
    public float attackPause;
    public float attackDamage;
    public float attackRange;
    public float checkpointRadius;

    private int numCheckpoints = 2;
    private int checkpoint;
    private float nextAttack;
    private Rigidbody rb;
    private GameObject gate;
    private Animation attackAnimation;
    private GateController gateController;
    private Transform postGateCheckpoint1;
    private Transform postGateCheckpoint2;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        gate = GameObject.FindWithTag("Gate");


        postGateCheckpoint1 = gate.transform.GetChild(0);
        postGateCheckpoint2 = gate.transform.GetChild(1);
        gateController = gate.GetComponent<GateController>();
        attackAnimation = transform.GetChild(0).GetComponent<Animation>();
        nextAttack = 0.0f;

        checkpoint = (gate.transform.GetChild(numCheckpoints).gameObject.activeSelf) ? 0 : 1;
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
            }
        }
    }

    void moveTowards(Transform target)
    {
        transform.LookAt(target);
        rb.velocity = transform.forward * speed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void attackGate()
    {
        if (Time.time > nextAttack)
        {
            transform.LookAt(gate.transform);
            attackAnimation.Play();

            gateController.takeDamage(attackDamage);

            if (gateController.health <= 0)
            {
                gate.transform.GetChild(numCheckpoints).gameObject.SetActive(false);
            }

            nextAttack = Time.time + attackPause;
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
