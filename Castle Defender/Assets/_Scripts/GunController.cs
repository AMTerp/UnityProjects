using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently using this sound https://freesound.org/people/deleted_user_4401185/sounds/253438/
public class GunController : MonoBehaviour {

    public float damage;
    public float recoilAmount;
    public float recoilYBias;
    public AudioClip gunSoundClip;

    private int recoilHash;
    private AudioSource gunShotSound;
    private Animation recoilAnimation;

	// Use this for initialization
	void Start () {
        gunShotSound = GetComponent<AudioSource>();
        recoilAnimation = transform.GetChild(0).GetComponent<Animation>();
    }

    public void Fire()
    {
        gunShotSound.PlayOneShot(gunSoundClip, 1);
        recoilAnimation.Play();
        applyRecoil(recoilAmount, recoilYBias);

        RaycastHit hit;
        if (Physics.Raycast(transform.parent.position, transform.up, out hit))
        {
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject.transform.parent && hit.collider.gameObject.transform.parent.CompareTag("Enemy"))
            {
                Health health = hit.collider.gameObject.transform.parent.GetComponent<Health>();
                health.TakeDamage(damage);
            }
        }
    }

    public void applyRecoil(float amount, float yBias)
    {
        MouseLookX xRotation = transform.parent.parent.GetComponent<MouseLookX>();
        MouseLookY yRotation = transform.parent.GetComponent<MouseLookY>();

        float angle = Random.Range(15, 46);
        if (Random.Range(0, 2) == 0)
        {
            angle = 180 - angle;
        }

        angle = calcAngle(angle, yBias);

        float xRecoil = Mathf.Cos(Mathf.Deg2Rad * angle) * amount;
        float yRecoil = Mathf.Sin(Mathf.Deg2Rad * angle) * amount;

        xRotation.rotationX += xRecoil;
        yRotation.rotationY += yRecoil;
    }

    public float calcAngle(float initAngle, float bias)
    {
        return ((90 * bias) + initAngle) / (bias + 1);
    }
}
