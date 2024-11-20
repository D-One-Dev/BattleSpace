using TMPro;
using Zenject;

public class PlayerMoney
{
    public int CurrentMoney { get; private set; }
    [Inject(Id = "MoneyText")]
    private readonly TMP_Text _moneyText;
    private SpaceshipSelector _spaceshipSelector;

    [Inject]
    public void Construct(SpaceshipSelector spaceshipSelector)
    {
        _spaceshipSelector = spaceshipSelector;
        CurrentMoney = 30;
        UpdateMoneyText();
        _spaceshipSelector.UpdateSpaceshipsAvailability();
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        UpdateMoneyText();
        _spaceshipSelector.UpdateSpaceshipsAvailability();
    }

    public bool CheckPurchaseAbility(int cost)
    {
        return CurrentMoney >= cost;
    }

    public void BuyItem(int cost)
    {
        CurrentMoney -= cost;
        UpdateMoneyText();
        _spaceshipSelector.UpdateSpaceshipsAvailability();
    }

    private void UpdateMoneyText()
    {
        _moneyText.text = "Money: " + CurrentMoney;
    }
}