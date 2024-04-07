using Interfaces;
using UnityEngine;

public class GridBlocker : MonoBehaviour, IClickable
{
    [SerializeField] BuildingGrid grid;
    [SerializeField] string popupWindowText;

    [SerializeField]  bool blocked = true;

    private void Awake()
    {
        if (!blocked)
            Remove();
    }

    public void Remove()
    {
        blocked = false;
        grid.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        GameEvents.current.PopupWindowCall(popupWindowText, () => Remove());
    }
}
