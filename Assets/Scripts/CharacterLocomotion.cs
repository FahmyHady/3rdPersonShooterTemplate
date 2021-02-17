// LocomotionSimpleAgent.cs
using System;
using UnityEngine;
using UnityEngine.AI;

public class CharacterLocomotion : MonoBehaviour
{
    Animator anim;
    Rigidbody rigidbody;
    float m_TurnAmount;
    float m_ForwardAmount;
    [HideInInspector] public Vector3 rotationVector;
    [HideInInspector] public Vector3 upperBodyRotationEulers;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        Vector3 v = anim.deltaPosition / Time.deltaTime;
        Vector3 newRot = new Vector3(
            transform.eulerAngles.x + rotationVector.x + anim.deltaRotation.eulerAngles.x,
             transform.eulerAngles.y + rotationVector.y + anim.deltaRotation.eulerAngles.y,
              transform.eulerAngles.z + rotationVector.z + anim.deltaRotation.eulerAngles.z
            );
        rigidbody.MoveRotation(Quaternion.Euler(newRot));
        v.y = rigidbody.velocity.y;
        rigidbody.velocity = v;
    }
    private void OnAnimatorIK()
    {
        //   upperBodyRotationEulers.y = 30;
        Quaternion chestRot = Quaternion.Euler(upperBodyRotationEulers);
        anim.SetBoneLocalRotation(HumanBodyBones.Chest, chestRot);
    }

    private RaycastHit PickOneBasedOnHeight(RaycastHit rightHitOne, RaycastHit rightHitTwo)
    {
        if (rightHitOne.point.y >= rightHitTwo.point.y)
            return rightHitOne;
        else
            return rightHitTwo;
    }

    public void Move(Vector3 move)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, Vector3.up);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;
        ApplyExtraTurnRotation();
        anim.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
    }
    public void MoveAiming(Vector3 move)
    {
        if (move.magnitude > 1f) move.Normalize();
        anim.SetFloat("Speed X", move.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Speed Z", move.z, 0.1f, Time.deltaTime);
    }
    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(360, 180, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
    public void UpdateAnimatorAimingState(bool aiming)
    {
        anim.SetBool("Aiming", aiming);
    }
}