using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SpaceshipSelector : MonoBehaviour
{
    [Inject(Id = "SpaceshipsButtons")]
    private readonly Button[] _buttons;
    [Inject(Id = "Spaceships")]
    private readonly Spaceship[] _spaceships;
    [Inject(Id = "ShipDescription")]
    private TMP_Text _shipDescription;
    private ObjectPlacer _objectPlacer;
    private PlayerMoney _playerMoney;

    [Inject]
    public void Construct(ObjectPlacer objectPlacer, PlayerMoney playerMoney)
    {
        _objectPlacer = objectPlacer;
        _playerMoney = playerMoney;
    }
    public void SelectSpaceship(Spaceship spaceship)
    {
        _objectPlacer.SetCurrentObject(spaceship);
        _shipDescription.text = spaceship.Description;
    }

    public void UpdateSpaceshipsAvailability()
    {
        for(int i = 0; i < _spaceships.Length; i++)
        {
            if (_playerMoney.CheckPurchaseAbility(_spaceships[i].Cost)) _buttons[i].interactable = true;
            else _buttons[i].interactable = false;
        }
    }

    public void ClearSpaceship()
    {
        _shipDescription.text = "";
    }
}