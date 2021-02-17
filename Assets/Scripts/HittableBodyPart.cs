using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BodyParts { Core, Head, LeftLeg, RightLeg, LeftArm, RightArm }
public class HittableBodyPart : MonoBehaviour
{
    List<HittableBodyPart> scriptsPointingToSameTarget = new List<HittableBodyPart>();
    [SerializeField] AvatarIKGoal whichBoneAmI;
    [SerializeField] bool fatalHit;
    [SerializeField] Rigidbody myIKTargetBody;
    [SerializeField] Rigidbody[] directlyConnectedRigidbodies;
    HittableBodyHandler ourHandler;
    [HideInInspector] public bool alreadyHit;
    private void Start()
    {
        ourHandler = GetComponentInParent<HittableBodyHandler>();
        if (myIKTargetBody != null)
            ourHandler.AddIKTarget(myIKTargetBody.gameObject);
        for (int i = 0; i < directlyConnectedRigidbodies.Length; i++)
        {
            var h = directlyConnectedRigidbodies[i].GetComponent<HittableBodyPart>();
            if (h != null)
            {
                scriptsPointingToSameTarget.Add(h);
            }

        }
    }
    public void GotHit(Vector3 hitDirection = default, float hitImpact = 0)
    {
        if (!alreadyHit)
        {

            if (fatalHit)
            {
                ourHandler.Died(true, hitDirection, hitImpact);
            }
            else
            {

                myIKTargetBody.transform.parent = null;
                myIKTargetBody.isKinematic = false;
                for (int i = 0; i < directlyConnectedRigidbodies.Length; i++)
                {
                    directlyConnectedRigidbodies[i].isKinematic = false;
                }
                ourHandler.AddPartToLoop(whichBoneAmI, myIKTargetBody.transform);
            }
            for (int i = 0; i < scriptsPointingToSameTarget.Count; i++)
            {
                scriptsPointingToSameTarget[i].alreadyHit = true; ;
            }
        }
        if (myIKTargetBody != null)
            myIKTargetBody.AddForce(hitDirection * hitImpact,ForceMode.Impulse);
    }

}
