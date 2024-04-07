using TMPro;
using UnityEngine;

public class MoneyCounterUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private void Awake()
    {
        GameEvents.current.onMoneyAmountChange += UpdateUI;
    }

    private void UpdateUI(int total, int add)
    {
        text.text = total.ToString() + "$";
    }
}
