using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacableObjectSelectButton : MonoBehaviour, IPointerClickHandler
{
    Image icon;

    [SerializeField] PlaceableSO objectData;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text nameText;

    [SerializeField] Color priceColorPositive;
    [SerializeField] Color priceColorNegative;

    private readonly string objectDataDescriptionTemplate =
        "Placing/selling price: {0}\n"
        + "Goods cost: {1}\n"
        + "Description: {2}";


    private void Awake()
    {
        icon = GetComponent<Image>();
        icon.sprite = objectData.icon;

        priceText.text = objectData.price.ToString() + "$";
        nameText.text = objectData.name;

        GameEvents.current.onMoneyAmountChange += UpdateUI;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameEvents.current.SelectedPlacableObjectChange(objectData);
            GameEvents.current.EnterBuildngMode();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            string popupWindowText = string.Format(objectDataDescriptionTemplate, objectData.price, objectData.goodsCost, objectData.description);
            GameEvents.current.PopupWindowCall(popupWindowText, false, false, () => { });
        }
    }

    public void UpdateUI(int totalMoney, int add)
    {
        priceText.color = totalMoney >= objectData.price ? priceColorPositive : priceColorNegative;
    }
}
