using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed;
    public float attackRate;
    public float attackDamage;

    private bool atGate;
    private float nextAttack;
    private Rigidbody rb;
    private Transform gate;
    private Animation attackAnimation;
    private GateController gateController;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        gate = GameObject.FindWithTag("Gate").transform;
        gateController = gate.GetComponent<GateController>();
        attackAnimation = transform.GetChild(0).GetComponent<Animation>();

        atGate = false;
        nextAttack = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (atGate)
        {
            attackGate();
        }
        else
        {
            moveTowardsGate();
        }
	}

    void moveTowardsGate()
    {
        if (Vector3.Distance(transform.position, gate.position) > 4)
        {
            transform.LookAt(gate);
            rb.velocity = transform.forward * speed * Time.deltaTime;
        }
        else
        {
            atGate = true;
            rb.velocity = Vector3.zero;
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void attackGate()
    {
        if (Time.time > nextAttack)
        {
            transform.LookAt(gate);
            attackAnimation.Play();

            gateController.takeDamage(attackDamage);

            nextAttack = Time.time + attackRate;
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        rb.velocity = Vector3.zero;
    }
}
