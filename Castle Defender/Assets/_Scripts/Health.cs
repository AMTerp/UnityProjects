using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float health;

	// Use this for initialization
	void Start () {
        health = 100;
	}

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage taken");
        health -= damage;
        CheckDead();
    }

    void CheckDead()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
