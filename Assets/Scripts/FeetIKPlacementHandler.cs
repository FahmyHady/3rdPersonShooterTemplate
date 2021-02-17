// LocomotionSimpleAgent.cs
using System;
using UnityEngine;
using UnityEngine.AI;

public class FeetIKPlacementHandler : MonoBehaviour
{
    Animator anim;
    [SerializeField] LayerMask IKTerrain;
    [SerializeField] float IKSpeed;
    [SerializeField] float pelvisAdjustmentSpeed;
    [SerializeField] [Range(0, 3)] float heightFromTheGroundPos = 1.5f;
    [SerializeField] [Range(0, 3)] float raycastDownDistance = 0.5f;
    [SerializeField] [Range(0, 3)] float pelvisOffset = 0;
    Vector3 leftFootPos, rightFootPos, leftFootIKPos, rightFootIKPos;
    Quaternion leftFootIKRotation, rightFootIKRotation;
    float lastLeftFootY, lastRightFootY, lastPelvisY;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        AdjustRaycastTargetPosition(ref rightFootPos, HumanBodyBones.RightFoot);
        AdjustRaycastTargetPosition(ref leftFootPos, HumanBodyBones.LeftFoot);

        RaycastAndSolveForFeetIK(rightFootPos, ref rightFootIKPos, ref rightFootIKRotation);
        RaycastAndSolveForFeetIK(leftFootPos, ref leftFootIKPos, ref leftFootIKRotation);
    }
    private void OnAnimatorIK()
    {
        AdjustPelvisHeight();
        MoveFeetToIKPos(AvatarIKGoal.RightFoot, rightFootIKPos, rightFootIKRotation, ref lastRightFootY);
        MoveFeetToIKPos(AvatarIKGoal.LeftFoot, leftFootIKPos, leftFootIKRotation, ref lastLeftFootY);
    }

    private void MoveFeetToIKPos(AvatarIKGoal foot, Vector3 footIKPos, Quaternion footIKRotation, ref float lastYPos)
    {
        anim.SetIKPositionWeight(foot, 1);
        anim.SetIKRotationWeight(foot, 1);
        Vector3 targetIKPos = anim.GetIKPosition(foot);
        if (footIKPos != Vector3.zero)
        {
            targetIKPos = transform.InverseTransformPoint(targetIKPos);
            footIKPos = transform.InverseTransformPoint(footIKPos);
            float YlerpValue = Mathf.Lerp(lastYPos, footIKPos.y, Time.deltaTime * IKSpeed);
            targetIKPos.y += YlerpValue;
            lastYPos = YlerpValue;

            targetIKPos = transform.TransformPoint(targetIKPos);
            anim.SetIKRotation(foot, footIKRotation);
        }
        anim.SetIKPosition(foot, targetIKPos);
    }
    private void AdjustPelvisHeight()
    {
        if (rightFootIKPos == Vector3.zero || leftFootIKPos == Vector3.zero || lastPelvisY == 0)
        {
            lastPelvisY = anim.bodyPosition.y;
            return;
        }
        float leftOffsetPos = leftFootIKPos.y - transform.position.y;
        float rightOffsetPos = rightFootIKPos.y - transform.position.y;
        float offset = leftOffsetPos < rightOffsetPos ? leftOffsetPos : rightOffsetPos;
        Vector3 newPelvisPos = anim.bodyPosition + Vector3.up * offset*0.75f;
        newPelvisPos.y = Mathf.Lerp(lastPelvisY, newPelvisPos.y, Time.deltaTime * pelvisAdjustmentSpeed);
        anim.bodyPosition = newPelvisPos;
        lastPelvisY = anim.bodyPosition.y;
    }
    private void RaycastAndSolveForFeetIK(Vector3 raycastOrigin, ref Vector3 footIKPos, ref Quaternion footIKRotation)
    {
        //Visualize Raycast
            Debug.DrawRay(raycastOrigin, Vector3.down * (raycastDownDistance + heightFromTheGroundPos), Color.red);
        //
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDownDistance + heightFromTheGroundPos, IKTerrain, QueryTriggerInteraction.Collide))
        {
            footIKPos = raycastOrigin;
            footIKPos.y = hit.point.y + pelvisOffset;
            footIKRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
            return;
        }
        footIKPos = Vector3.zero;
    }

    private void AdjustRaycastTargetPosition(ref Vector3 footPosition, HumanBodyBones foot)
    {
        footPosition = anim.GetBoneTransform(foot).position;
        footPosition.y = transform.position.y + heightFromTheGroundPos;
    }


}