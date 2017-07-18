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
    public bool automatic;
    public bool chamberBullet;
    public float zoomTime;
    public float zoomFOV;
    public AudioClip gunSoundClip;

    internal int currAmmoInClip;
    internal int currSpareAmmo;
    internal bool canFire;
    internal Animation animations;
    internal bool zoomedIn;
    private Vector3 idlePosition;
    private Vector3 idleRotation;
    private Vector3 idleScale;

    private int ammoBefore;
    private Camera mainCamera;
    private AmmoUIController ammoUI;
    private BulletTimer bulletTimerUI;
    private AudioSource gunShotSound;
    private float initFOV;

	// Use this for initialization
	void Start () {
        gunShotSound = GetComponent<AudioSource>();
        animations = transform.GetChild(0).GetComponent<Animation>();
        ammoUI = GameObject.FindWithTag("Ammo UI").GetComponent<AmmoUIController>();
        mainCamera = transform.parent.parent.GetComponent<Camera>();
        bulletTimerUI = GameObject.FindWithTag("Bullet Timer").GetComponent<BulletTimer>();

        idlePosition = transform.GetChild(0).localPosition;
        idleRotation = transform.GetChild(0).localEulerAngles;
        idleScale = transform.GetChild(0).localScale;

        currAmmoInClip = magazineSize;
        currSpareAmmo = initSpareAmmo;
        canFire = true;
        zoomedIn = false;
        initFOV = mainCamera.fieldOfView;
    }

    public void Fire()
    {
        if (currAmmoInClip > 0)
        {
            applyRecoil(recoilAmount, recoilYBias);
            gunShotSound.PlayOneShot(gunSoundClip, 1);
            animations.Play(gameObject.name + " Shot");

            // If last bullet is being fired, don't do chamber bullet animation nor play .
            if (currAmmoInClip != 1)
            {
                if (chamberBullet)
                {
                    StartCoroutine(ChamberBullet());
                }

                StartCoroutine(bulletTimerUI.RunTimer(firePause));
            }
            else
            {
                StartCoroutine(bulletTimerUI.RunTimer(firePause, "empty"));
            }


            RaycastHit hit;
            if (Physics.Raycast(transform.parent.position, transform.up, out hit))
            {
                Debug.Log("Target hit: " + hit.collider.gameObject);
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
        MouseLookX xRotation = transform.parent.parent.parent.GetComponent<MouseLookX>();
        MouseLookY yRotation = transform.parent.parent.GetComponent<MouseLookY>();

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

    IEnumerator ChamberBullet()
    {
        bool played = false;
        while (!played)
        {
            if (!animations.isPlaying)
            {
                animations.Play(gameObject.name + " Chamber Bullet");
                played = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Reload()
    {
        canFire = false;
        ammoBefore = currAmmoInClip;
        animations.Play(gameObject.name + " Reload");
        yield return new WaitForSeconds(reloadTime);
        if (canFire)
        {
            // If canFire is true, then we must have swapped weapons mid-reload. Cancel the rest of the reload.
            Debug.Log("BREAK RELOAD");
            yield break;
        }

        currAmmoInClip = Mathf.Clamp(magazineSize, 0, currSpareAmmo + ammoBefore);
        currSpareAmmo -= currAmmoInClip - ammoBefore;
        ammoUI.setAmmoCount(currAmmoInClip, currSpareAmmo);
        StartCoroutine(bulletTimerUI.RunTimer(firePause, "full"));
        canFire = true;
    }

    public void resetTransform()
    {
        transform.GetChild(0).localPosition = idlePosition;
        transform.GetChild(0).localEulerAngles = idleRotation;
        transform.GetChild(0).localScale = idleScale;
        Debug.Log("Setting lPos: " + idlePosition);
        Debug.Log("Setting lRot: " + idleRotation);
        Debug.Log("Setting lSca: " + idleScale);
    }

    public IEnumerator Zoom()
    {
        Debug.Log("Zoom called");
        float currTime = 0.0f;
        if (!zoomedIn && canFire)
        {
            while (mainCamera.fieldOfView > zoomFOV)
            {
                mainCamera.fieldOfView = Mathf.Lerp(initFOV, zoomFOV, currTime / zoomTime);
                yield return new WaitForEndOfFrame();
                currTime += Time.deltaTime;
            }

            zoomedIn = true;
        }
        else if (zoomedIn)
        {
            while (mainCamera.fieldOfView < initFOV)
            {
                mainCamera.fieldOfView = Mathf.Lerp(zoomFOV, initFOV, currTime / zoomTime);
                yield return new WaitForEndOfFrame();
                currTime += Time.deltaTime;
            }

            zoomedIn = false;
        }
    }
}
