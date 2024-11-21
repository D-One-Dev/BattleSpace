using UnityEngine;
using Zenject;

public class UIController
{
    [Inject(Id = "PlaneSelectScreen")]
    private readonly GameObject _planeSelectScreen;
    [Inject(Id = "GameplayScreen")]
    private readonly GameObject _gameplayScreen;
    private GlobalGameState _globalGameState;
    [Inject]
    public void Construct(GlobalGameState globallGameState)
    {
        _globalGameState = globallGameState;
        _globalGameState.OnStateChangeEvent += UpdateUIState;
    }
    private void UpdateUIState(State state)
    {
        switch (state)
        {
            case State.PlaneSelection:
                _gameplayScreen.SetActive(false);
                _planeSelectScreen.SetActive(true);
                break;
            case State.Gameplay:
                _planeSelectScreen.SetActive(false);
                _gameplayScreen.SetActive(true);
                break;
            default:
                _gameplayScreen.SetActive(false);
                _planeSelectScreen.SetActive(true);
                break;
        }
    }
}