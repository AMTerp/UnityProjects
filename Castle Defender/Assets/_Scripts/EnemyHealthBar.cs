using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

    private float initHealth;
    private float currHealth;
    private Health enemyHealth;
    private Transform playerTransform;

    // Use this for initialization
    void Start () {
        enemyHealth = transform.parent.parent.GetComponent<Health>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        initHealth = enemyHealth.health;
        currHealth = initHealth;
    }

    void Update()
    {
        LookAtTarget(playerTransform);
    }

    void LookAtTarget(Transform target)
    {
        transform.parent.transform.LookAt(target);
    }

    public void updateHealthBar()
    {
        currHealth = Mathf.Clamp(enemyHealth.health, 0, initHealth);
        gameObject.transform.localScale = new Vector3(currHealth / initHealth,
            gameObject.transform.localScale.y,
            gameObject.transform.localScale.z);
    }
}
