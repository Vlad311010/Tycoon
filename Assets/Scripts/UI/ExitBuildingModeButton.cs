using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitBuildingModeButton : MonoBehaviour, IPointerClickHandler
{
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        GameEvents.current.onBuilingModeEnter += SetActive;
        GameEvents.current.onBuilingModeExit += SetInactive;
    }

    private void OnDestroy()
    {
        GameEvents.current.onBuilingModeEnter -= SetActive;
        GameEvents.current.onBuilingModeExit -= SetInactive;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (btn.interactable)
        {
            GameEvents.current.ExitBuildngMode();
        }
    }

    private void SetActive()
    {
        btn.interactable = true;
    }

    private void SetInactive()
    {
        btn.interactable = false;
    }


}
