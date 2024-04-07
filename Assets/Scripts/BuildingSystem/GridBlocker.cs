using Interfaces;
using UnityEngine;

public class GridBlocker : MonoBehaviour, IClickable
{
    [SerializeField] BuildingGrid grid;
    [SerializeField] string popupWindowText;

    public bool removed;

    public void Remove()
    {
        removed = true;
        grid.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        GameEvents.current.PopupWindowCall(popupWindowText, () => Remove());
    }
}
