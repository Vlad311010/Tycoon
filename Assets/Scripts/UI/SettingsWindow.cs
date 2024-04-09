using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] Slider cameraMovementSpeedSlider;
    [SerializeField] Slider cameraRotationSpeedSlider;

    private void OnEnable()
    {
        cameraMovementSpeedSlider.onValueChanged.AddListener(MovementSpeedChange);
        cameraRotationSpeedSlider.onValueChanged.AddListener(RotationSpeedChange);

        cameraMovementSpeedSlider.value = PersistentDataManager.GameData.CameraMovementSpeed;
        cameraRotationSpeedSlider.value = PersistentDataManager.GameData.CameraRotationSpeed;
    }

    private void MovementSpeedChange(float value)
    {
        PersistentDataManager.GameData.CameraMovementSpeed = (int)value;
        PersistentDataManager.Save();
    }

    private void RotationSpeedChange(float value)
    {
        PersistentDataManager.GameData.CameraRotationSpeed = (int)value;
        PersistentDataManager.Save();
    }

    private void OnDisable()
    {
        cameraMovementSpeedSlider.onValueChanged.RemoveListener(MovementSpeedChange);
        cameraRotationSpeedSlider.onValueChanged.RemoveListener(RotationSpeedChange);

        PersistentDataManager.Save();
    }
}
