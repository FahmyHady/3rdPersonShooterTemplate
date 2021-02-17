using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public enum State { Normal, Hobbling, Crawling, OneArmed, NoArms }
public class HittableBodyHandler : MonoBehaviour
{
    List<AvatarIKGoal> bonesHit = new List<AvatarIKGoal>();
    List<Transform> bonesHitTargets = new List<Transform>();
    List<GameObject> allIKTargets = new List<GameObject>();
    CharacterManipulatorScript manipulatorScript;
    HittableBodyPart[] hittableBodyParts;
    Animator animator;
    bool oneLegGone;
    bool bothLegsGone;
    bool oneArmGone;
    bool bothArmsGone;
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    private void Start()
    {
        manipulatorScript = GetComponent<CharacterManipulatorScript>();
        hittableBodyParts = GetComponentsInChildren<HittableBodyPart>();
        animator = GetComponentInParent<Animator>();
    }

    public void AddIKTarget(GameObject target)
    {
        if (!allIKTargets.Contains(target))
            allIKTargets.Add(target);
    }
    public void AddPartToLoop(AvatarIKGoal which, Transform boneTarget)
    {
        switch (which)
        {
            case AvatarIKGoal.LeftFoot:
                animator.SetLayerWeight(3, 0);
                if (oneLegGone)
                {
                    oneLegGone = false;
                    bothLegsGone = true;
                }
                else
                    oneLegGone = true;
                break;
            case AvatarIKGoal.RightFoot:
                animator.SetLayerWeight(1, 0);
                if (oneLegGone)
                {
                    oneLegGone = false;
                    bothLegsGone = true;
                }
                else
                    oneLegGone = true;
                break;
            case AvatarIKGoal.LeftHand:
                animator.SetLayerWeight(4, 0);
                if (oneArmGone)
                    bothArmsGone = true;
                else
                    oneArmGone = true;
                break;
            case AvatarIKGoal.RightHand:
                animator.SetLayerWeight(2, 0);
                if (oneArmGone)
                    bothArmsGone = true;
                else
                    oneArmGone = true;
                break;
        }
        bonesHit.Add(which);
        bonesHitTargets.Add(boneTarget);
        CheckState();
    }
    void CheckState()
    {
        if (bothLegsGone && bothArmsGone)
        {
            Died();
        }
        else if (oneLegGone)
        {
            //animator.applyRootMotion = true;
            animator.SetBool("Hop", true);
        }
        else if (bothLegsGone)
        {
            animator.applyRootMotion = true;
            animator.SetBool("Crawl", true);
        }

    }
    public void Died(bool addForce = false, Vector3 direction = default, float hitImpact = 0)
    {
        for (int i = 0; i < allIKTargets.Count; i++)
        {
            Destroy(allIKTargets[i]);
        }
        animator.enabled = false;
        manipulatorScript.RagdollMe(addForce, direction, hitImpact);
        for (int i = 0; i < hittableBodyParts.Length; i++)
        {
            Destroy(hittableBodyParts[i]);
        }
    }
    private void OnAnimatorIK()
    {
        if (bonesHit.Count > 0)
        {
            for (int i = 0; i < bonesHit.Count; i++)
            {
                animator.SetIKPositionWeight(bonesHit[i], 1);
                //   animator.SetIKRotationWeight(bonesHit[i], 1);
                animator.SetIKPosition(bonesHit[i], bonesHitTargets[i].position);
                //    animator.SetIKRotation(bonesHit[i], bonesHitTargets[i].rotation);
            }
        }
    }
}
