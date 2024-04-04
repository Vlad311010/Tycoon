using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    DefaultControls control;

    [SerializeField] CameraController cameraController;

    private bool hodlingCameraMovementInput = false;

    void Awake()
    {
        control = new DefaultControls();
        control.Enable();

        // cameraController = GetComponent<CharacterActor>();

        control.Camera.Movement.performed += cameraController.OnMovement;
        control.Camera.Movement.canceled += cameraController.OnMovement;

        control.Camera.Rotation.performed += cameraController.OnRotation;
        control.Camera.Rotation.canceled += cameraController.OnRotation;
    }

    private void OnDestroy()
    {
        control.Camera.Movement.performed -= cameraController.OnMovement;
        control.Camera.Movement.canceled -= cameraController.OnMovement;

        control.Camera.Rotation.performed -= cameraController.OnRotation;
        control.Camera.Rotation.canceled -= cameraController.OnRotation;
    }
}
