using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform bear;
    public float panSpeed = 0.5f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 3f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    bool focusOnBear = true;
    private Vector3 dragOrigin;
    private float zoomDistance = 20f;
    private Vector3 cameraTarget;

    void Start()
    {
        focusOnBear = true;
        cameraTarget = bear != null ? bear.position : Vector3.zero;
        zoomDistance = 20f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            focusOnBear = !focusOnBear;
        }

        if (bear != null && focusOnBear)
        {
            cameraTarget = bear.position;
        }
        else
        {
            HandleFreeCamera();
        }

        // Always update position based on target and zoom
        transform.position = cameraTarget - transform.forward * zoomDistance;
    }

    void HandleFreeCamera()
    {
        // Pan with left mouse
        if (Input.GetMouseButtonDown(0))
            dragOrigin = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 diff = Input.mousePosition - dragOrigin;
            Vector3 move = new Vector3(-diff.x, 0, -diff.y) * panSpeed * Time.deltaTime;

            // Move in camera's local space
            cameraTarget += transform.TransformDirection(move);
            dragOrigin = Input.mousePosition;
        }

        // Rotate with right mouse
        if (Input.GetMouseButton(1))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotY = -Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.RotateAround(cameraTarget, Vector3.up, rotX);
            transform.RotateAround(cameraTarget, transform.right, rotY);
        }

        // Zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoomDistance -= scroll * zoomSpeed;
        zoomDistance = Mathf.Clamp(zoomDistance, minZoom, maxZoom);
    }
}
