using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSensor : MonoBehaviour {

    public float remainingDamageCanDeal;

    private Health enemyHealth;
    private float damageToDeal;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Enemy"))
        {
            // An enemy stepped on the trap. Destroy it.
            //enemyHealth = other.transform.parent.GetComponent<Health>();
            //enemyHealth.TakeDamage(enemyHealth.health);

            //numEnemiesCanKill--;
            //if (numEnemiesCanKill == 0)
            //{
            //    // Have killed as many enemies as the trap can. Destroy the game object.
            //    Destroy(transform.parent.gameObject);
            //}

            enemyHealth = other.transform.parent.GetComponent<Health>();
            damageToDeal = Mathf.Clamp(enemyHealth.health, 0, remainingDamageCanDeal);

            enemyHealth.TakeDamage(damageToDeal);
            remainingDamageCanDeal -= damageToDeal;

            if (Mathf.Approximately(remainingDamageCanDeal, 0.0f))
            {
                // Have killed as many enemies as the trap can. Destroy the game object.
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
