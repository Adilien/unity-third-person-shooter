using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class RaycastWeapon : MonoBehaviour
{
    public bool isFiring = false;
    public int firerate = 10;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public WeaponRecoil recoil;
    public GameObject magazine;
    public int ammoCount;
    public int clipSize;
    public ReloadWeapon reload;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;

    private void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        accumulatedTime += Time.deltaTime;
    }

    public void StartFiring()
    {
        if (reload.reloading == false)
        {
            isFiring = true;
            FireBullet();
            recoil.Reset();
            if (accumulatedTime > 0)
            {
                accumulatedTime = 0;
            }
        }
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / firerate;

        while (accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    private void FireBullet()
    {
        if (ammoCount <= 0)
        {
            return;
        }

        ammoCount--;

        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }

        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;

        var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);

            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            tracer.transform.position = hitInfo.point;
        }

        recoil.GenerateRecoil();
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
