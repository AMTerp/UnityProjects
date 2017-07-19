using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSensor : MonoBehaviour {

    public int numEnemiesCanKill;

    private Health enemyHealth;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trap sensor triggered");
        if (other.transform.parent.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered trap trigger");
            // An enemy stepped on the trap. Destroy it.
            enemyHealth = other.transform.parent.GetComponent<Health>();
            enemyHealth.TakeDamage(enemyHealth.health);

            numEnemiesCanKill--;
            if (numEnemiesCanKill == 0)
            {
                // Have killed as many enemies as the trap can. Destroy the game object.
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
