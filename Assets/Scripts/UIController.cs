using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class UIController
{
    [Inject(Id = "PlaneSelectScreen")]
    private readonly GameObject _planeSelectScreen;
    [Inject(Id = "GameplayScreen")]
    private readonly GameObject _gameplayScreen;
    [Inject(Id = "DeathScreen")]
    private readonly GameObject _deathScreen;
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
                _gameplayScreen.GetComponent<CanvasGroup>().DOFade(0f, .1f).SetUpdate(UpdateType.Normal, true).OnKill(() => { _gameplayScreen.SetActive(false);
                    _planeSelectScreen.GetComponent<CanvasGroup>().alpha = 0f;
                    _planeSelectScreen.SetActive(true);
                    _planeSelectScreen.GetComponent<CanvasGroup>().DOFade(1f, .1f).SetUpdate(UpdateType.Normal, true);
                    });
                break;
            case State.Gameplay:
                //_planeSelectScreen.SetActive(false);
                //_gameplayScreen.SetActive(true);
                _planeSelectScreen.GetComponent<CanvasGroup>().DOFade(0f, .1f).SetUpdate(UpdateType.Normal, true).OnKill(() => {
                    _planeSelectScreen.SetActive(false);
                    _gameplayScreen.GetComponent<CanvasGroup>().alpha = 0f;
                    _gameplayScreen.SetActive(true);
                    _gameplayScreen.GetComponent<CanvasGroup>().DOFade(1f, .1f).SetUpdate(UpdateType.Normal, true);
                });
                break;
            case State.Death:
                _gameplayScreen.GetComponent<CanvasGroup>().DOFade(0f, .1f).SetUpdate(UpdateType.Normal, true).OnKill(() => {
                    _gameplayScreen.SetActive(false);
                    _deathScreen.GetComponent<CanvasGroup>().alpha = 0f;
                    _deathScreen.SetActive(true);
                    _deathScreen.GetComponent<CanvasGroup>().DOFade(1f, .1f).SetUpdate(UpdateType.Normal, true);
                });
                break;
            default:
                //_gameplayScreen.SetActive(false);
                //_planeSelectScreen.SetActive(true);
                _gameplayScreen.GetComponent<CanvasGroup>().DOFade(0f, .1f).SetUpdate(UpdateType.Normal, true).OnKill(() => {
                    _gameplayScreen.SetActive(false);
                    _planeSelectScreen.GetComponent<CanvasGroup>().alpha = 0f;
                    _planeSelectScreen.SetActive(true);
                    _planeSelectScreen.GetComponent<CanvasGroup>().DOFade(1f, .1f).SetUpdate(UpdateType.Normal, true);
                });
                break;
        }
    }
}