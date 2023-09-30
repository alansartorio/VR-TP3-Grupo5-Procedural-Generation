using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference look;
    [SerializeField] private InputActionReference forward;
    [SerializeField] private InputActionReference backwards;
    [SerializeField] private InputActionReference left;
    [SerializeField] private InputActionReference right;

    [SerializeField] private float sensitivity = 0.5f;
    [SerializeField] private float speed = 2f;

    private Transform child;

    private float horizontalAngle;
    private float verticalAngle;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        look.action.performed += Look;
        var euler = transform.rotation.eulerAngles;
        verticalAngle = 0;
        horizontalAngle = euler.y;
        child = transform.GetChild(0);
    }

    private void Update()
    {
        var objTransform = transform;

        if ((int)forward.action.ReadValue<float>() == 1)
            objTransform.position += objTransform.forward * (speed * Time.deltaTime);

        if ((int)backwards.action.ReadValue<float>() == 1)
            objTransform.position -= objTransform.forward * (speed * Time.deltaTime);

        if ((int)right.action.ReadValue<float>() == 1)
            objTransform.position += objTransform.right * (speed * Time.deltaTime);

        if ((int)left.action.ReadValue<float>() == 1)
            objTransform.position -= objTransform.right * (speed * Time.deltaTime);
    }

    private void Look(InputAction.CallbackContext obj)
    {
        var delta = obj.ReadValue<Vector2>() * sensitivity;

        verticalAngle -= delta.y;
        verticalAngle = Mathf.Clamp(verticalAngle, -90, 90);
        horizontalAngle += delta.x;
        transform.localRotation = Quaternion.Euler(0, horizontalAngle, 0);
        child.localRotation = Quaternion.Euler(verticalAngle, 0, 0);
    }
}