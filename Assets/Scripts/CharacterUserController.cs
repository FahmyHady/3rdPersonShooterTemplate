// ClickToMove.cs
using UnityEngine;
using UnityEngine.AI;

public class CharacterUserController : MonoBehaviour
{
    Transform camera;
    Rigidbody rigidbody;
    [SerializeField] float aimSmoothTime = 0.1f;
    [SerializeField] float aimSenstivity = 5f;
    [SerializeField] Transform myAimFocusPoint;
    bool aiming;
    [SerializeField] bool hasControl = true;
    float lastYRot;
    float lastXRot;
    float h = 0;
    float v = 0;
    CharacterLocomotion locomotion;
    private void OnEnable()
    {
        EventManager.StartListening("Penetrating Shot Triggered", DisableControl);
        EventManager.StartListening("Penetrating Shot Hit", EnableControl);
    }
    private void OnDisable()
    {
        EventManager.StopListening("Penetrating Shot Triggered", DisableControl);
        EventManager.StopListening("Penetrating Shot Hit", EnableControl);
    }
    void DisableControl() => hasControl = false;
    void EnableControl() => hasControl = true;

    void Start()
    {
        camera = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
        locomotion = GetComponent<CharacterLocomotion>();

    }
    void Update()
    {
        if (hasControl)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                transform.rotation = (Quaternion.Euler(0, camera.eulerAngles.y, 0));
                aiming = true;
                EventManager.TriggerEvent("Aiming State Changed", aiming);

            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                aiming = false;
                EventManager.TriggerEvent("Aiming State Changed", aiming);
            }
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
        locomotion.UpdateAnimatorAimingState(aiming);
        if (!aiming)
        {
            Vector3 cameraForward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * cameraForward + h * camera.right;
            locomotion.rotationVector = Vector3.zero;
            locomotion.upperBodyRotationEulers = Vector3.zero;
            locomotion.Move(move);
        }
        else
        {
            Vector3 move = new Vector3(h, 0, v);
            float mouseDeltaX = Input.GetAxis("Mouse X");
            float mouseDeltaY = -Input.GetAxis("Mouse Y");

            //float finalYRotation = mouseDeltaX * Time.deltaTime * aimSenstivity;
            //float yRotationVariable = Mathf.Lerp(lastYRot, finalYRotation, Time.deltaTime * aimSmoothTime);
            //targetYRotation += yRotationVariable;
            //lastYRot = yRotationVariable;
            float targetYRotation = SmoothAimInput(ref lastYRot, mouseDeltaX * Time.deltaTime * aimSenstivity);
            float targetXRotation = SmoothAimInput(ref lastXRot, mouseDeltaY * Time.deltaTime * aimSenstivity * 0.5f);


            Vector3 rotationEulers = Vector3.zero;
            locomotion.upperBodyRotationEulers.x = Mathf.Clamp(locomotion.upperBodyRotationEulers.x + targetXRotation, -15, 30);
            myAimFocusPoint.eulerAngles = new Vector3(locomotion.upperBodyRotationEulers.x, transform.eulerAngles.y, transform.eulerAngles.z);
            rotationEulers.y = targetYRotation;
            locomotion.rotationVector = rotationEulers;
            locomotion.MoveAiming(move);
        }
    }

    float SmoothAimInput(ref float lastInputValue, float valueToSmooth)
    {
        float target = 0;
        float final = valueToSmooth;// mouseDeltaX * Time.deltaTime * aimSenstivity;
        float smoothingVariable = Mathf.Lerp(lastInputValue, final, Time.deltaTime * aimSmoothTime);
        target += smoothingVariable;
        lastInputValue = smoothingVariable;
        return target;
    }

}