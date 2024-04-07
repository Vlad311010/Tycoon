using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacableObjectSelectButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] PlaceableSO objectData;
    [SerializeField] TMP_Text priceText;

    [SerializeField] Color priceColorPositive;
    [SerializeField] Color priceColorNegative;

    private void Awake()
    {
        priceText.text = objectData.price.ToString() + "$";

        GameEvents.current.onMoneyAmountChange += UpdateUI;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.current.SelectedPlacableObjectChange(objectData);
    }

    public void UpdateUI(int totalMoney, int add)
    {
        priceText.color = totalMoney >= objectData.price ? priceColorPositive : priceColorNegative;
    }
}
