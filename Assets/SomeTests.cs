using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeTests : MonoBehaviour
{
    [SerializeField]
    Transform child;
    private void OnDrawGizmos()
    {
        Vector3 direction = new Vector3(1,0,1);
        Debug.DrawRay(transform.position, direction*10, Color.red);
        direction =transform.TransformDirection(direction) ;
        Debug.DrawRay(transform.position, direction*10, Color.green);
        child.forward = direction;
        Vector3 angle = transform.forward;
        float angleFloat = Mathf.Atan2(angle.z, angle.x)*Mathf.Rad2Deg;
        Debug.Log(angleFloat);
    }
}
