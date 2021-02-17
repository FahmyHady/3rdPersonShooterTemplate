using UnityEngine;

//This is a camera script made by Haravin (Daniel Valcour).
//This script is public domain, but credit is appreciated!

[RequireComponent(typeof(Camera))]
public class CameraSpectator : MonoBehaviour
{

    public float moveSpeed;
    public float shiftAdditionalSpeed;
    public float mouseSensitivity;
    public bool invertMouse;
    public bool autoLockCursor;

    private Camera cam;

    void Awake()
    {
        cam = this.gameObject.GetComponent<Camera>();
        this.gameObject.name = "SpectatorCamera";
        Cursor.lockState = (autoLockCursor) ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update()
    {
        float speed = (moveSpeed + (Input.GetAxis("Fire3") * shiftAdditionalSpeed));
        this.gameObject.transform.Translate(Vector3.forward * speed * Input.GetAxis("Vertical"));
        this.gameObject.transform.Translate(Vector3.right * speed * Input.GetAxis("Horizontal"));
        this.gameObject.transform.Translate(Vector3.up * speed * (Input.GetAxis("Jump") + (Input.GetAxis("Fire1") * -1)));
        this.gameObject.transform.Rotate(Input.GetAxis("Mouse Y") * mouseSensitivity * ((invertMouse) ? 1 : -1), Input.GetAxis("Mouse X") * mouseSensitivity * ((invertMouse) ? -1 : 1), 0);
        this.gameObject.transform.localEulerAngles = new Vector3(this.gameObject.transform.localEulerAngles.x, this.gameObject.transform.localEulerAngles.y, 0);

        if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Cursor.lockState == CursorLockMode.Locked && Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
