
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    public Transform target; // The target to focus on
    public float distance = 10.0f; // Distance from the target
    public float sensitivity = 5.0f; // Sensitivity for mouse movement

    public float rotationX = -5f; // Rotation around X-axis
    public float rotationY = 5f; // Rotation around Y-axis

    public bool isLockedEnabled = false;

    private int viewIndex = 0;

    public Vector3 posCamera = Vector3.zero;
    public bool setPosCamera = false;
    bool isLocked = false;

    Vector2 posTouch = Vector2.zero;


    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (!isLockedEnabled)
        {
            return;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            // Code specific to Android
            Debug.Log("Running on Android");

            UnityEngine.InputSystem.EnhancedTouch.Touch touch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                posTouch = touch.screenPosition;
                // Handle touch start
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 posTouch2 = touch.screenPosition;
                sensitivity = 0.01f;
                rotationX += (posTouch2 - posTouch).x * sensitivity;
                rotationY -= (posTouch2 - posTouch).y * sensitivity;

                rotationY = Mathf.Clamp(rotationY, -60f, 60f);
            }
        }
        else
        {

            bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
            bool isCtrlPressed = Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;
            if (!isShiftPressed || !isCtrlPressed)
            {
                if (isLocked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    isLocked = false;
                }
                return;
            }
            if (!isLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                isLocked = true;
            }


            // Get mouse input
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            rotationX += mouseDelta.x * sensitivity;
            rotationY -= mouseDelta.y * sensitivity;

            // Clamp vertical rotation to prevent flipping
            rotationY = Mathf.Clamp(rotationY, -60f, 60f);
        }

        bool isUp = Keyboard.current.pageUpKey.isPressed;
        bool isDown = Keyboard.current.pageDownKey.isPressed;
        if (isUp)
        {
            SetViewPoint(++viewIndex);
        }
        if (isDown)
        {
            SetViewPoint(--viewIndex);
        }

        

    }

    void LateUpdate()
    {
        // Calculate the new position and rotation of the camera
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // Set the camera's position and look at the target
        transform.position = target.position + rotation * direction;
        transform.LookAt(target.position);
    }

    public void SetViewPoint(int viewIndex)
    {
        Vector3[] vpVector = new Vector3[8];
        vpVector[0] = new Vector3(6, 6, -6);
        vpVector[1] = new Vector3(-6, 6, -6);
        vpVector[2] = new Vector3(-6, 6, 6);
        vpVector[3] = new Vector3(6, 6, 6);

        vpVector[4] = new Vector3(6, -4, -6);
        vpVector[5] = new Vector3(-6, -4, -6);
        vpVector[6] = new Vector3(-6, -4, 6);
        vpVector[7] = new Vector3(6, -4, -6);

        if (viewIndex < 0)
            viewIndex += 10000;
        if (viewIndex < 0)
            return;
        viewIndex = viewIndex % 8;

        posCamera = vpVector[viewIndex % 8];
        setPosCamera = true;
        Debug.Log($"View:{viewIndex} pos:{posCamera.x}; {posCamera.y}; {posCamera.z}");

        transform.position = posCamera;
        transform.LookAt(target.position);

    }
}
