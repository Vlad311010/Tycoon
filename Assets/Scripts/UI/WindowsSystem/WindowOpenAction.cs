using UnityEngine;
using UnityEngine.EventSystems;

public class WindowOpenAction : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] WindowUI windowToOpen;

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<WindowsController>().OpenWindow(windowToOpen);
    }
}
