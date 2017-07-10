using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently using this sound https://freesound.org/people/deleted_user_4401185/sounds/253438/
public class GunController : MonoBehaviour {

    public float damage;
    public float firePause;
    public float recoilAmount;
    public float recoilYBias;
    public int magazineSize;
    public int maxSpareAmmo;
    public int initSpareAmmo;
    public float reloadTime;
    public AudioClip gunSoundClip;

    internal int currAmmoInClip;
    internal int currSpareAmmo;
    internal bool canFire;

    private Animation animations;
    private AmmoUIController ammoUI;
    private AudioSource gunShotSound;
    private int ammoBefore;

	// Use this for initialization
	void Start () {
        gunShotSound = GetComponent<AudioSource>();
        animations = transform.GetChild(0).GetComponent<Animation>();
        ammoUI = GameObject.FindWithTag("Ammo UI").GetComponent<AmmoUIController>();

        currAmmoInClip = magazineSize;
        currSpareAmmo = initSpareAmmo;
        canFire = true;

        ammoUI.setAmmoCount(currAmmoInClip, currSpareAmmo);
    }

    public void Fire()
    {
        if (currAmmoInClip > 0)
        {
            gunShotSound.PlayOneShot(gunSoundClip, 1);
            animations.Play(gameObject.name + " Shot");
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

            currAmmoInClip -= 1;

            ammoUI.setAmmoCount(currAmmoInClip, currSpareAmmo);
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

    public IEnumerator Reload()
    {
        canFire = false;
        ammoBefore = currAmmoInClip;
        animations.Play(gameObject.name + " Reload");
        yield return new WaitForSeconds(reloadTime);
        currAmmoInClip = Mathf.Clamp(magazineSize, 0, currSpareAmmo + ammoBefore);
        currSpareAmmo -= currAmmoInClip - ammoBefore;
        ammoUI.setAmmoCount(currAmmoInClip, currSpareAmmo);
        canFire = true;
    }
}
