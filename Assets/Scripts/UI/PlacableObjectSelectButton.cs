using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacableObjectSelectButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] PlaceableSO objectData;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text nameText;

    [SerializeField] Color priceColorPositive;
    [SerializeField] Color priceColorNegative;

    private void Awake()
    {
        priceText.text = objectData.price.ToString() + "$";
        nameText.text = objectData.name;

        GameEvents.current.onMoneyAmountChange += UpdateUI;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.current.SelectedPlacableObjectChange(objectData);
        GameEvents.current.EnterBuildngMode();
    }

    public void UpdateUI(int totalMoney, int add)
    {
        priceText.color = totalMoney >= objectData.price ? priceColorPositive : priceColorNegative;
    }
}
