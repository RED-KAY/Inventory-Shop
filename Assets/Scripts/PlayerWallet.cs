using TMPro;
using UnityEngine;

public class PlayerWallet : GenericMonoSingleton<PlayerWallet>
{
    [SerializeField] TextMeshProUGUI _BalanceT;

    int _Balance;

    public int Balance { get { return _Balance; } }

    private void Start()
    {
        _Balance = 300;

        EventService.Instance._OnItemSoldAddMoney.AddListener(AddMoney);
    }

    private void OnDisable()
    {
        EventService.Instance._OnItemSoldAddMoney.RemoveListener(AddMoney);
    }

    public void AddMoney(int amount)
    {
        _Balance += amount;
    }

    private void DeductMoney(int amount)
    {
        _Balance -= amount;

        if (Balance < 0) {
            _Balance = 0;
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
        _BalanceT.text = _Balance.ToString();
    }

   
}

