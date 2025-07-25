using TMPro;
using UnityEngine;

public class PlayerWallet : GenericMonoSingleton<PlayerWallet>
{
    [SerializeField] TextMeshProUGUI m_BalanceT;

    int m_Balance;

    public int Balance { get { return m_Balance; } }

    private void Start()
    {
        m_Balance = 300;

        EventService.Instance.m_OnItemSoldAddMoney.AddListener(AddMoney);
    }

    private void OnDisable()
    {
        EventService.Instance.m_OnItemSoldAddMoney.RemoveListener(AddMoney);
    }

    public void AddMoney(int amount)
    {
        m_Balance += amount;
    }

    private void DeductMoney(int amount)
    {
        m_Balance -= amount;

        if (Balance < 0) {
            m_Balance = 0;
        }
    }

    private void OnItemSold(string id, int amount)
    {

    }

    public bool TryTransaction(int requiredAmount)
    {
        if (requiredAmount <= 0)
        {
            return true;
        }

        if(Balance >= requiredAmount)
        {
            DeductMoney(requiredAmount);
            return true;
        }
        
        return false;
    }

    private void Update()
    {
        m_BalanceT.text = m_Balance.ToString();
    }

   
}

