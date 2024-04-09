using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }

    public event Action<GoodsContainer> onGoodsContainerPlaced;
    public void GoodsContainerPlaced(GoodsContainer container)
    {
        if (onGoodsContainerPlaced != null)
        {
            onGoodsContainerPlaced(container);
        }
    }

    public event Action<GoodsContainer> onGoodsContainerRemoved;
    public void GoodsContainerRemoved(GoodsContainer container)
    {
        if (onGoodsContainerRemoved != null)
        {
            onGoodsContainerRemoved(container);
        }
    }

    public event Action<int> onCustomerPayment;
    public void CustomerPays(int money)
    {
        if (onCustomerPayment != null)
        {
            onCustomerPayment(money);
        }
    }

    public event Action<int, int> onMoneyAmountChange;
    public void MoneyAmountChange(int total, int add)
    {
        if (onMoneyAmountChange != null)
        {
            onMoneyAmountChange(total, add);
        }
    }

    public event Action<PlaceableSO> onSelectedPlacableObjectChange;
    public void SelectedPlacableObjectChange(PlaceableSO objectData)
    {
        if (onSelectedPlacableObjectChange != null)
        {
            onSelectedPlacableObjectChange(objectData);
        }

    }

    public event Action<string, bool, bool, Action> onPopupWindowCall;
    public void PopupWindowCall(string text, bool textAlingCenter, bool confirmationWindow, Action okBtnAction)
    {
        if (onPopupWindowCall != null)
        {
            onPopupWindowCall(text, textAlingCenter, confirmationWindow,  okBtnAction);
        }
    }

    public event Action onBuilingModeExit;
    public void ExitBuildngMode()
    {
        if (onBuilingModeExit != null)
        {
            onBuilingModeExit();
        }
    }

    public event Action onBuilingModeEnter;
    public void EnterBuildngMode()
    {
        if (onBuilingModeEnter != null)
        {
            onBuilingModeEnter();
        }
    }

    public event Action onCameraSettingsChange;
    public void CameraSettingsChange()
    {
        if (onCameraSettingsChange != null)
        {
            onCameraSettingsChange();
        }
    }

}
