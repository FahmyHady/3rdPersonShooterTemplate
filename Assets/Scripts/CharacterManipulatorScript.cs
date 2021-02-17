//#define TRIAL_USING_NAVMESH

using UnityEngine;
public class CharacterManipulatorScript : MonoBehaviour
{
    #region Private Fields
    [Header("SET IN INSPECTOR")]
    [HideInInspector] public bool _ragdoll;
    [SerializeField] Rigidbody[] _ragdollsRigidBody;
    #endregion

    #region MonoBehavior Callbacks
    Vector3[] originalPos;
    private void Awake()
    {

        Collider collider;

        int _counter = _ragdollsRigidBody.Length;
        PhysicMaterial _material = new PhysicMaterial();
        _material.dynamicFriction = 0.4f;
        _material.staticFriction = 0.4f;
        _material.bounciness = 0.5f;

        for (int i = 1; i < _counter; i++)
            _ragdollsRigidBody[i].GetComponent<Collider>().material = _material;

        originalPos = new Vector3[_ragdollsRigidBody.Length];
        for (int i = 0; i < _ragdollsRigidBody.Length; i++)
        {
            originalPos[i] = _ragdollsRigidBody[i].transform.position;
        }
    }
    public void RagdollMe(bool addForce = false, Vector3 forceToAddDirection = default, float hitImpact = 0)
    {
        if (!_ragdoll)
        {
            ToggleDeath();
            if (addForce)
                AddForce(forceToAddDirection, hitImpact);
        }
    }

    #endregion

    public void ToggleDeath()
    {
        _ragdoll = !_ragdoll;
        if (_ragdoll)
        {
            EnableRagdoll();
        }
        else
        {
            DisableRagdoll();
        }
    }

    void EnableRagdoll()
    {
        for (int i = 0; i < _ragdollsRigidBody.Length; i++)
        {
            _ragdollsRigidBody[i].detectCollisions = true;
            _ragdollsRigidBody[i].isKinematic = false;
        }
    }
    void DisableRagdoll()
    {
        for (int i = 0; i < _ragdollsRigidBody.Length; i++)
        {
            _ragdollsRigidBody[i].detectCollisions = false;
            _ragdollsRigidBody[i].isKinematic = true;
            _ragdollsRigidBody[i].velocity = Vector3.zero;
            _ragdollsRigidBody[i].angularVelocity = Vector3.zero;
            ///     _ragdollsRigidBody[i].position = originalPos[i];
        }
    }
    private void AddForce(Vector3 forceAfterDeathDirection, float hitImpact)
    {
        foreach (var rb in _ragdollsRigidBody)
        {
            rb.AddForce(forceAfterDeathDirection * hitImpact, ForceMode.Impulse);
        }
    }
}
