using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float speed;
    public float attackRate;
    public float attackDamage;
    public float attackRange;

    private float nextAttack;
    private Rigidbody rb;
    private GameObject gate;
    private Animation attackAnimation;
    private GateController gateController;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        gate = GameObject.FindWithTag("Gate");
        gateController = gate.GetComponent<GateController>();
        attackAnimation = transform.GetChild(0).GetComponent<Animation>();
        nextAttack = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(transform.position, gate.transform.position) <= attackRange)
        {
            rb.velocity = Vector3.zero;
            attackGate();
        }
        else
        {
            moveTowardsGate();
        }
	}

    void moveTowardsGate()
    {
        transform.LookAt(gate.transform);
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

            nextAttack = Time.time + attackRate;
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
