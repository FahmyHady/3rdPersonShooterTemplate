using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    Ray ray;
    Transform camera;
    [Header("Gun Properties")]
    int bulletsInMagazine;
    [SerializeField] float gunImpact = 100;
    [SerializeField] float timeBetweenShots = 0.2f;
    [SerializeField] int maxBulletsInMagazine = 8;
    [Header("Gun VFX")]
    private Animator gunAnimator;
    [SerializeField] GameObject hitParticlePrefab;
    [SerializeField] GameObject wallPenetratingBulletSystemPrefab;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform nozzlePos;
    [SerializeField] float bulletSpeed;
    [SerializeField] ParticleSystem muzzleFlashSystem;
    [SerializeField] ParticleSystem bulletCasingEjectSystem;
    [SerializeField] ParticleSystem magazineEjectSystem;
    AudioSource gunAudioSource;
    [SerializeField] AudioClip gunShootClip;
    [SerializeField] AudioClip gunReloadClip;
    CharacterGunController characterGunControl;
    float elapsedTime;
    bool reloading;
    bool aiming;
    bool doTheVFX;
    private void Start()
    {
        camera = Camera.main.transform;
        gunAudioSource = GetComponentInChildren<AudioSource>();
        gunAnimator = GetComponentInChildren<Animator>();
        characterGunControl = GetComponentInParent<CharacterGunController>();
        bulletsInMagazine = maxBulletsInMagazine;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        EventManager.StartListening("Aiming State Changed", (UnityEngine.Events.UnityAction<bool>)JustDoAimVFX);
    }
    private void OnDisable()
    {
        EventManager.StopListening("Aiming State Changed", (UnityEngine.Events.UnityAction<bool>)JustDoAimVFX);
    }
    private void JustDoAimVFX(bool aiming)
    {
        this.aiming = aiming;
    }
    private void Update()
    {
        if (elapsedTime >= timeBetweenShots)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (bulletsInMagazine > 0)
                {
                    elapsedTime = 0;
                    bulletsInMagazine--;
                    if (aiming) ShootPhysics();
                    if (doTheVFX) ShootVFX();
                    characterGunControl.Shot();
                }
                else if (!reloading)
                {
                    Reload();
                }
            }
        }
        elapsedTime += Time.deltaTime;
    }

    private void Reload()
    {
        reloading = true;
        characterGunControl.Reload();
        gunAudioSource.clip = gunReloadClip;
        gunAudioSource.Play();
    }
    public void EjectMag()
    {
        magazineEjectSystem.Emit(1);
    }
    public void Reloaded()
    {
        reloading = false;
        bulletsInMagazine = maxBulletsInMagazine;
    }
    private void ShootVFX()
    {
        gunAnimator.SetTrigger("Fire");
        StartCoroutine(ShootVFXRoutine());

    }
    IEnumerator ShootVFXRoutine()
    {
        yield return new WaitForSeconds(0.11f);
        muzzleFlashSystem.Emit(30);
        bulletCasingEjectSystem.Emit(1);
        gunAudioSource.clip = gunShootClip;
        gunAudioSource.Play();
    }
    //private void ShootPhysics()
    //{
    //    ray.origin = camera.position;
    //    ray.direction = camera.forward;
    //    if (Physics.Raycast(ray, out RaycastHit hit, 100))
    //    {
    //        Instantiate(hitParticlePrefab, hit.point, Quaternion.identity);
    //        if (hit.collider.attachedRigidbody != null)
    //        {
    //            var hittable = hit.collider.gameObject.GetComponent<HittableBodyPart>();
    //            if (hittable != null)
    //            {
    //                hittable.GotHit(ray.direction, gunImpact);
    //            }
    //            else
    //            {
    //                hit.collider.attachedRigidbody.AddForce(ray.direction * gunImpact, ForceMode.Impulse);
    //            }
    //        }
    //    }
    //}   
    private void ShootPhysics()
    {
        bool hitLivingEnemy = false;
        bool hitWall = false;
        RaycastHit penetratingHit = default;
        HittableBodyPart hittable = null;
        ray.origin = camera.position;
        ray.direction = camera.forward;
        RaycastHit[] hits = Physics.RaycastAll(ray, 100);
        doTheVFX = true;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit.collider.attachedRigidbody != null)
                {
                    hittable = hit.collider.gameObject.GetComponent<HittableBodyPart>();
                    if (hittable != null)
                    {
                        hitLivingEnemy = true;
                    }
                    else
                    {
                        hit.collider.attachedRigidbody.AddForce(ray.direction * gunImpact, ForceMode.Impulse);
                    }
                }
                else if (hit.collider.gameObject.tag == "Wall")
                {
                    if (i == 0)
                    {
                        hitWall = true;
                        penetratingHit = hit;
                    }
                }
            }
        }

        if (hitLivingEnemy && hitWall)
        {
            doTheVFX = false;
            FirePenetratingShot(penetratingHit,hittable);
        }
        else if (hitLivingEnemy)
        {
            hittable.GotHit(ray.direction, gunImpact);
        }
        else
        {
            Instantiate(hitParticlePrefab, hits[0].point, Quaternion.identity);
        }

    }
    void FirePenetratingShot(RaycastHit targetHit, HittableBodyPart hittableBodyPart)
    {
        var rot = camera.rotation;
        EventManager.TriggerEvent("Penetrating Shot Triggered");
        StartCoroutine(PenetratingShotRoutine(targetHit, rot,hittableBodyPart));

    }
    IEnumerator PenetratingShotRoutine(RaycastHit penetratingHit, Quaternion bulletRot, HittableBodyPart hittableBodyPart)
    {
        Vector3 targetPos = penetratingHit.point;
        yield return new WaitForSeconds(2);
        SlowMotion(0.2f);
        ShootVFX();
        yield return new WaitForSeconds(0.11f);
        Transform bullet = Instantiate(bulletPrefab, nozzlePos.position, bulletRot).transform;
        float t = 0;
        bool triggered = false;
        Vector3 euler = bullet.rotation.eulerAngles;
        while (Vector3.Distance(bullet.position, targetPos) > 0.1f)
        {
            if (t < 0.05f) t += Time.deltaTime;
            else if (!triggered)
            {
                SlowMotion(0.5f);
                triggered = true;
                EventManager.TriggerEvent("Penetrating Shot Fired", bullet);
            }

            bullet.position = Vector3.MoveTowards(bullet.position, targetPos, Time.deltaTime * bulletSpeed);
            euler.z += Time.deltaTime * bulletSpeed*100;
            bullet.eulerAngles = euler;
            yield return null;
        }
        SlowMotion(1);
        EventManager.TriggerEvent("Penetrating Shot Hit");
        Destroy(bullet.gameObject);
        Instantiate(wallPenetratingBulletSystemPrefab, penetratingHit.point, penetratingHit.transform.rotation);
        hittableBodyPart.GotHit(ray.direction, gunImpact);
    }
    void SlowMotion(float timescaleValue)
    {
        Time.timeScale = timescaleValue;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
