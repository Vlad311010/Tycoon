using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowCloseAction : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Selectable>().interactable)
            GetComponentInParent<WindowsController>().CloseWindow();
    }
}
