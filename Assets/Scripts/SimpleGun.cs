using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGun : MonoBehaviour
{
    Ray ray;
    Transform camera;
    [Header("Gun Properties")]
    [SerializeField] float gunImpact = 100;
    [SerializeField] GameObject hitParticlePrefab;

    private void Start()
    {
        camera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {


            ShootPhysics();


        }

    }





    private void ShootPhysics()
    {
        ray.origin = camera.position;
        ray.direction = camera.forward;
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Instantiate(hitParticlePrefab, hit.point, Quaternion.identity);
            if (hit.collider.attachedRigidbody != null)
            {
                var hittable = hit.collider.gameObject.GetComponent<HittableBodyPart>();
                if (hittable != null)
                {
                    hittable.GotHit(ray.direction, gunImpact);
                }
                else
                {
                    hit.collider.attachedRigidbody.AddForce(ray.direction * gunImpact, ForceMode.Impulse);
                }
            }
        }
    }
}
