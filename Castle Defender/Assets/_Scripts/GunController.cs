using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently using this sound https://freesound.org/people/deleted_user_4401185/sounds/253438/
public class GunController : MonoBehaviour {

    public float damage;

    private AudioSource gunShotSound;

	// Use this for initialization
	void Start () {
        gunShotSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire()
    {
        Debug.Log("Firing...");
        gunShotSound.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit))
        {
            if (hit.collider.gameObject.transform.parent.CompareTag("Enemy"))
            {
                Health health = hit.collider.gameObject.transform.parent.GetComponent<Health>();
                health.TakeDamage(damage);
                Debug.Log("Hit: " + hit.collider.gameObject.transform.parent.tag);
                Debug.Log("Health reduced");
            }
        }
    }
}
