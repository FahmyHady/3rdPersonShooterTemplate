using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTurns : MonoBehaviour
{
    Animator animator;
    Camera camera;
    private void Start()
    {
        camera = Camera.main;
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 cameraAdjustedDirection = Vector3.zero;
        Vector3 direction = new Vector3(h, 0, v).normalized;
        float targetAngle = 0;
        float angleFromForward = 0;
        if (direction.magnitude > 0)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg - camera.transform.eulerAngles.y;
            Quaternion newRotation = Quaternion.Euler(0, targetAngle, 0);
            //   transform.rotation = newRotation;
            cameraAdjustedDirection = (newRotation * Vector3.forward).normalized;
            float finalAngle = Mathf.Atan2(cameraAdjustedDirection.x, cameraAdjustedDirection.z) * Mathf.Rad2Deg - transform.eulerAngles.y;
            finalAngle %= 180;
         

            animator.SetTrigger("ShouldTurn");
            animator.SetFloat("Angle", finalAngle);
        }
    }
}
