using UnityEngine;

[CreateAssetMenu(fileName = "Customer")]
public class CustomerSO : ScriptableObject
{
    public int Mood { get => mood; }
    public int Money { get => money; }


    [SerializeField] private int initMood;
    [SerializeField] private int initMoney;

    public int goodsCost;

    private int mood;
    private int money;

    public static CustomerSO CreateInstance(CustomerSO customerSO)
    {
        CustomerSO customerData = ScriptableObject.CreateInstance<CustomerSO>();
        customerData.initMoney = customerSO.money;
        customerData.initMood = customerSO.mood;
        customerData.mood = customerSO.initMood;
        customerData.money = customerSO.initMoney;
        return customerData;
    }
}
