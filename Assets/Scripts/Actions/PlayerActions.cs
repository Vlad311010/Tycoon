using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    static DefaultControls control;

    [SerializeField] CameraController cameraController;
    PlayerInteractions playerInteractions;

    void Awake()
    {
        control = new DefaultControls();
        control.Enable();

        playerInteractions = GetComponent<PlayerInteractions>();

        control.Gameplay.Movement.performed += cameraController.OnMovement;
        control.Gameplay.Movement.canceled += cameraController.OnMovement;

        control.Gameplay.Rotation.performed += cameraController.OnRotation;
        control.Gameplay.Rotation.canceled += cameraController.OnRotation;


        GameEvents.current.onBuilingModeEnter += EnterBuildingMode;
        GameEvents.current.onBuilingModeExit += ExitBuildingMode;
    }

    private void OnDestroy()
    {
        control.Gameplay.Movement.performed -= cameraController.OnMovement;
        control.Gameplay.Movement.canceled -= cameraController.OnMovement;

        control.Gameplay.Rotation.performed -= cameraController.OnRotation;
        control.Gameplay.Rotation.canceled -= cameraController.OnRotation;

        GameEvents.current.onBuilingModeEnter -= EnterBuildingMode;
        GameEvents.current.onBuilingModeExit -= ExitBuildingMode;
    }

    private void EnterBuildingMode()
    {
        control.Gameplay.LeftClick.performed += playerInteractions.Build;
        control.Gameplay.RightClick.performed += playerInteractions.Remove;

        control.Gameplay.LeftClick.performed -= playerInteractions.OnLeftClick;
        control.Gameplay.RightClick.performed -= playerInteractions.OnRightClick;
        control.Gameplay.MouseMove.performed -= playerInteractions.OnMouseMovement;
    }

    private void ExitBuildingMode()
    {
        control.Gameplay.LeftClick.performed += playerInteractions.OnLeftClick;
        control.Gameplay.RightClick.performed += playerInteractions.OnRightClick;
        control.Gameplay.MouseMove.performed += playerInteractions.OnMouseMovement;

        control.Gameplay.LeftClick.performed -= playerInteractions.Build;
        control.Gameplay.RightClick.performed -= playerInteractions.Remove;
    }

    public void MouseInteractionSetActive(bool active)
    {
        if (active)
        {
            control.Gameplay.LeftClick.Enable();
            control.Gameplay.RightClick.Enable();
        }
        else
        {
            control.Gameplay.LeftClick.Disable();
            control.Gameplay.RightClick.Disable();
        }
    }
}
