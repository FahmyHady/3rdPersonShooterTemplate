// ClickToMove.cs
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    RaycastHit hitInfo = new RaycastHit();
    NavMeshAgent agent;
    CharacterController characterController;
    Camera camera;
    [SerializeField] float speed;
    [SerializeField] float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool stopping;
    
    void Start()
    {
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        agent.updateRotation = false;

    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                agent.destination = hitInfo.point;
        }
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;
        if (direction.magnitude > 0)
        {
            float inputDirectionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float targetAngle = inputDirectionAngle + camera.transform.eulerAngles.y;

            Quaternion angleone = Quaternion.Euler(0, targetAngle, 0);
            Quaternion anlgeTow = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            float final = Quaternion.Angle(angleone,anlgeTow);
            if (final > 60 && agent.speed - agent.velocity.magnitude <6)
            {
                agent.destination = transform.position;
            }
            else if(!stopping)
            {

                float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                Quaternion newRotation = Quaternion.Euler(0, smoothedAngle, 0);
                transform.rotation = newRotation;
                Vector3 cameraAdjustedDirection = newRotation * Vector3.forward;
                //   characterController.SimpleMove(cameraAdjustedDirection.normalized * speed);
                if (agent.enabled)
                    agent.destination = transform.position + cameraAdjustedDirection.normalized * speed;
            }
        }
        agent.SamplePathPosition(agent.areaMask, 0.5f, out NavMeshHit hit);
        Debug.Log((hit.mask & 1 << 3) == 1 << 3);

    }

    public void Stopping()
    {
        stopping = true;
    }
    public void Stopped()
    {
        stopping = false;
    }
}