using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerBaseHealth : EntityHealth
{
    private int _startHealth;
    [Inject(Id = "PlayerBaseHealthbar")]
    private readonly Image _healtbar;
    [Inject(Id = "BaseHealthText")]
    private readonly TMP_Text _baseHealthText;
    private GlobalGameState _globalGameState;

    [Inject]
    public void Construct(GlobalGameState globalGameState)
    {
        _globalGameState = globalGameState;
    }

    private void Start()
    {
        _startHealth = health;
        _healtbar.fillAmount = (float)health / _startHealth;
        _baseHealthText.text = "Base health: " + health;
    }

    protected override void OnDeath()
    {
        Time.timeScale = 0f;
        Debug.Log("Death");
        _globalGameState.ChangeCurrentState(State.Death);
    }

    protected override void OnDamage()
    {
        _healtbar.fillAmount = (float)health / _startHealth;
        _baseHealthText.text = "Base health: " + health;
    }
}
