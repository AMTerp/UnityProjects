using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently using this sound https://freesound.org/people/deleted_user_4401185/sounds/253438/
public class GunController : MonoBehaviour {

    public float damage;
    public float recoilAmount;
    public float recoilYBias;

    private AudioSource gunShotSound;
    private Animation recoilAnimation;

	// Use this for initialization
	void Start () {
        gunShotSound = GetComponent<AudioSource>();
        recoilAnimation = transform.GetChild(0).GetComponent<Animation>();
        Debug.Log(recoilAnimation);
    }

    public void Fire()
    {
        gunShotSound.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.parent.position, transform.up, out hit))
        {
            if (hit.collider.gameObject.transform.parent.CompareTag("Enemy"))
            {
                Health health = hit.collider.gameObject.transform.parent.GetComponent<Health>();
                health.TakeDamage(damage);

                Debug.Log(recoilAnimation.Play());
                applyRecoil(recoilAmount, recoilYBias);
            }
        }
    }

    public void applyRecoil(float amount, float yBias)
    {
        //Transform cameraY = transform.parent;
        //Transform playerX = cameraY.parent;

        MouseLookX xRotation = transform.parent.parent.GetComponent<MouseLookX>();
        MouseLookY yRotation = transform.parent.GetComponent<MouseLookY>();

        float angle = Random.Range(15, 46);
        if (Random.Range(0, 2) == 0)
        {
            angle = 180 - angle;
        }
        Debug.Log("Before bias angle: " + angle);
        angle = calcAngle(angle, yBias);
        Debug.Log("After bias angle : " + angle);

        float xRecoil = Mathf.Cos(Mathf.Deg2Rad * angle) * amount;
        float yRecoil = Mathf.Sin(Mathf.Deg2Rad * angle) * amount;

        //playerX.localEulerAngles = new Vector3(0, playerX.localEulerAngles.x + xRecoil, 0);
        //cameraY.localEulerAngles = new Vector3(cameraY.localEulerAngles.y - yRecoil, 0, 0);

        xRotation.rotationX += xRecoil;
        yRotation.rotationY += yRecoil;
    }

    public float calcAngle(float initAngle, float bias)
    {
        return ((90 * bias) + initAngle) / (bias + 1);
    }
}
