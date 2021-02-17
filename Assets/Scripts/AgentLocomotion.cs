// LocomotionSimpleAgent.cs
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLocomotion : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    CharacterController characterController;
    bool shouldMove;
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 velocity = Vector3.zero;
        if (agent.enabled)
            velocity = agent.velocity;
        else
            velocity = characterController.velocity;
        float playerForwardVelocity = Vector3.Dot(velocity, transform.forward);
        float playerRightdVelocity =  Vector3.Dot(velocity, transform.right);
        Vector3 movement = new Vector3(playerRightdVelocity, 0, playerForwardVelocity);
        if (movement.sqrMagnitude >=0.01f)
            shouldMove = true;
        else
            shouldMove = false;
        anim.SetBool("ShouldMove", shouldMove);
        anim.SetFloat("Speed X", playerRightdVelocity);
        anim.SetFloat("Speed Z", playerForwardVelocity);
    }

}