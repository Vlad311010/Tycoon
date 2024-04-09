using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float zoomSpeed;
    [SerializeField] Vector2 YLimits;
    [SerializeField] Vector3 anchor;
    [SerializeField] float XLimit;
    [SerializeField] float ZLimit;

    // parameters
    private float movementSpeed;
    private float rotationSpeed;

    
    private Vector3 movementDirection;
    private float rotationDirection;

    private void Awake()
    {
        movementSpeed = PersistentDataManager.GameData.CameraMovementSpeed;
        rotationSpeed = PersistentDataManager.GameData.CameraRotationSpeed;
    }

    private void Update()
    {
        Move(movementDirection);
        Rotate(rotationDirection);
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            movementDirection = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);

        if (ctx.canceled)
            movementDirection = Vector2.zero;
    }

    public void OnRotation(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            rotationDirection = ctx.ReadValue<float>();

        if (ctx.canceled)
            rotationDirection = 0;
    }

    public void Move(Vector3 direction)
    {
        Vector3 relativeDirection = transform.rotation * direction;
        relativeDirection.y = 0;

        Vector3 newPosition = transform.position + relativeDirection * movementSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, anchor.x - XLimit, anchor.z + XLimit);
        newPosition.z = Mathf.Clamp(newPosition.z, anchor .z - ZLimit, anchor.z + ZLimit);
        
        transform.position = newPosition;
    }

    public void Rotate(float direction)
    {
        Vector3 curretEulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(curretEulerAngles.x, curretEulerAngles.y + rotationSpeed * direction * Time.deltaTime, curretEulerAngles.z);
    }
}
