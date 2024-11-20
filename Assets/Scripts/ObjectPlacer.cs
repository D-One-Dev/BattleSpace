using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

public class ObjectPlacer
{
    [Inject(Id = "TestPrefab")]
    private readonly Spaceship _testPrefab;

    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;
    private Controls _controls;
    private DiContainer _container;
    private GloballGameState _globalGameState;

    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private TrackableId _currentPlane;
    private Spaceship _objectPrefab;
    private PlayerMoney _playerMoney;

    [Inject]
    public void Construct(DiContainer container, ARRaycastManager raycastManager, ARPlaneManager planeManager, Controls controls, GloballGameState globallGameState, PlayerMoney playerMoney)
    {
        _container = container;
        _raycastManager = raycastManager;
        _planeManager = planeManager;
        _controls = controls;
        _globalGameState = globallGameState;
        _playerMoney = playerMoney;

        _controls.Gameplay.PrimaryTouch.performed += ctx => Touch();
        _controls.Enable();

        _globalGameState.OnStateChangeEvent += OnGameStateChanged;

        _objectPrefab = _testPrefab;
    }

    private void Touch()
    {
        Raycast();
    }

    private void Raycast()
    {
        if(_objectPrefab != null)
        {
            if (_raycastManager.Raycast(_controls.Gameplay.PrimaryTouchPosition.ReadValue<Vector2>(), _hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = _hits[0].pose;
                if(_globalGameState.CurrentState == State.PlaneSelection)
                {
                    _currentPlane = _hits[0].trackableId;
                    _container.InstantiatePrefab(_objectPrefab.Prefab, pose.position, pose.rotation, _hits[0].trackable.gameObject.transform);
                    _objectPrefab = null;

                    _globalGameState.ChangeCurrentState(State.Gameplay);
                }
                else if(_globalGameState.CurrentState == State.Gameplay)
                {
                    if (_playerMoney.CheckPurchaseAbility(_objectPrefab.Cost))
                    {
                        _playerMoney.BuyItem(_objectPrefab.Cost);
                        _container.InstantiatePrefab(_objectPrefab.Prefab, pose.position, pose.rotation, _hits[0].trackable.gameObject.transform);
                        _objectPrefab = null;
                    }
                }
            }
        }
    }

    private void OnGameStateChanged(State newState)
    {
        switch (newState)
        {
            case State.Gameplay:
                foreach(var plane in _planeManager.trackables)
                {
                    if(plane.trackableId != _currentPlane) plane.gameObject.SetActive(false);
                }
                _planeManager.requestedDetectionMode = PlaneDetectionMode.None;
                break;
            default:
                break;
        }
    }

    public void SetCurrentObject(Spaceship obj)
    {
        _objectPrefab = obj;
    }
}
