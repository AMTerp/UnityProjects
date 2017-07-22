using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public float damage;
    public float firePause;
    public float recoilAmount;
    public float recoilYBias;
    public int magazineSize;
    public int maxSpareAmmo;
    public int initSpareAmmo;
    public float reloadTime;
    public float zoomTime;
    public float zoomFOV;
    public float aiDelay;
    public bool automatic;
    public bool chamberBullet;
    public string ammoType;
    public AudioClip gunFireSound;
    public float gunFireVolume;
    public AudioClip gunReloadSound;
    public float gunReloadVolume;
    public GameObject objectToSpawn;

    internal int currAmmoInClip;
    internal int currSpareAmmo;
    internal bool canFire;
    internal float nextFire;
    internal Animation animations;
    internal bool zoomedIn;

    private Vector3 idlePosition;
    private Vector3 idleRotation;
    private Vector3 idleScale;

    private float shotAnimationLength;
    private bool attachedToPlayer;
    private int ammoBefore;
    private Camera mainCamera;
    private AmmoUIController ammoUI;
    private BulletTimer bulletTimerUI;
    private AudioSource gunSounds;
    private float initFOV;

	// Use this for initialization
	void Start () {
        attachedToPlayer = transform.parent.parent.name.Equals("Main Camera");
        
        gunSounds = GetComponent<AudioSource>();
        animations = transform.GetChild(0).GetComponent<Animation>();
        ammoUI = GameObject.FindWithTag("Ammo UI").GetComponent<AmmoUIController>();
        bulletTimerUI = GameObject.FindWithTag("Bullet Timer").GetComponent<BulletTimer>();

        idlePosition = transform.GetChild(0).localPosition;
        idleRotation = transform.GetChild(0).localEulerAngles;
        idleScale = transform.GetChild(0).localScale;

        // Get the length of the animation clip for gunfire.
        foreach (AnimationState state in animations)
        {
            if (state.name.Equals(gameObject.name + " Shot"))
            {
                shotAnimationLength = state.length;
                break;
            }
        }

        currAmmoInClip = magazineSize;
        currSpareAmmo = initSpareAmmo;
        canFire = true;
        zoomedIn = false;
        nextFire = 0.0f;

        if (attachedToPlayer)
        {
            mainCamera = transform.parent.parent.GetComponent<Camera>();
            initFOV = mainCamera.fieldOfView;
        }
        else
        {
            // Add a slight random extra delay to each turret such that they don't all shoot the same target.
            firePause *= (aiDelay + Random.Range(0.0f, 0.01f));
        }
            
    }

    public void Fire()
    {
        if (currAmmoInClip > 0)
        {
            if (attachedToPlayer)
                applyRecoil(recoilAmount, recoilYBias);

            gunSounds.PlayOneShot(gunFireSound, gunFireVolume);
            Debug.Log("Gonna play animation: " + gameObject.name + " Shot");
            animations.Play(gameObject.name + " Shot");

            // If last bullet is being fired, don't do chamber bullet animation nor play .
            if (currAmmoInClip != 1)
            {
                if (chamberBullet)
                {
                    StartCoroutine(ChamberBullet());
                }

                if (attachedToPlayer)
                    StartCoroutine(bulletTimerUI.RunTimer(firePause));
            }
            else if (attachedToPlayer)
            {
                StartCoroutine(bulletTimerUI.RunTimer(firePause, "empty"));
            }


            RaycastHit hit;
            if (Physics.Raycast(transform.parent.parent.position, transform.up, out hit))
            {
                if (ammoType.Equals("bullet"))
                {
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        Health health = hit.collider.gameObject.transform.parent.GetComponent<Health>();
                        health.TakeDamage(damage);
                    }
                }
                else if (ammoType.Equals("object"))
                {
                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                        Instantiate(objectToSpawn, hit.point, Quaternion.identity);
                    }
                }
            }

            currAmmoInClip -= 1;
            nextFire = Time.time + firePause;

            if (attachedToPlayer)
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
        // Wait until the shot fire animation is finished.
        yield return new WaitForSeconds(shotAnimationLength);
        // As animations.isPlaying is not false yet, wait another frame for it to become false.
        yield return new WaitForEndOfFrame();

        // With this if statement, the chamber bullet animation will only play if the player is not reloading.
        Debug.Log("Gonna play chamber bullet " + animations.isPlaying);
        if (!animations.isPlaying)
        {
            animations.Play(gameObject.name + " Chamber Bullet");
        }
    }

    public IEnumerator Reload()
    {
        canFire = false;
        nextFire = Time.time + reloadTime;
        ammoBefore = currAmmoInClip;
        animations.Play(gameObject.name + " Reload");
        yield return new WaitForSeconds(reloadTime - gunReloadSound.length);

        if (canFire)
        {
            // If canFire is true, then we must have swapped weapons mid-reload. Cancel the rest of the reload.
            nextFire = 0.0f;
            yield break;
        }

        gunSounds.PlayOneShot(gunReloadSound, gunReloadVolume);
        yield return new WaitForSeconds(gunReloadSound.length);

        if (canFire)
        {
            // If canFire is true, then we must have swapped weapons mid-reload. Cancel the rest of the reload.
            nextFire = 0.0f;
            yield break;
        }

        currAmmoInClip = Mathf.Clamp(magazineSize, 0, currSpareAmmo + ammoBefore);
        currSpareAmmo -= currAmmoInClip - ammoBefore;
        canFire = true;
        if (attachedToPlayer)
            ammoUI.setAmmoCount(currAmmoInClip, currSpareAmmo);
            StartCoroutine(bulletTimerUI.RunTimer(firePause, "full"));
    }

    public void resetTransform()
    {
        transform.GetChild(0).localPosition = idlePosition;
        transform.GetChild(0).localEulerAngles = idleRotation;
        transform.GetChild(0).localScale = idleScale;
    }

    public IEnumerator Zoom()
    {
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
