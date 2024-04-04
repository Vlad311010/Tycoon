using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float zoomSpeed;
    [SerializeField] Vector2 YLimits;


    private Vector3 movementDirection;
    private float rotationDirection;

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

        transform.position += relativeDirection * movementSpeed * Time.deltaTime;
    }

    public void Rotate(float direction)
    {
        Vector3 curretEulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(curretEulerAngles.x, curretEulerAngles.y + rotationSpeed * direction * Time.deltaTime, curretEulerAngles.z);
    }
}