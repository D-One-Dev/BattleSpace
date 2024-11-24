using UnityEngine;
using Zenject;

public class EnemyShipHealth : EntityHealth
{
    [SerializeField] private int shipCost;
    private PlayerMoney _playerMoney;

    [Inject]
    public void Construct(PlayerMoney playerMoney)
    {
        _playerMoney = playerMoney;
    }

    protected override void OnDeath()
    {
        _playerMoney.AddMoney(shipCost);
        Destroy(gameObject);
    }
}