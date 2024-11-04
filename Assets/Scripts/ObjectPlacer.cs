using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

public class ObjectPlacer
{
    [Inject(Id = "TestPrefab")]
    private readonly GameObject _testPrefab;

    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;
    private Controls _controls;
    private DiContainer _container;
    private GloballGameState _globalGameState;

    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private TrackableId _currentPlane;

    [Inject]
    public void Construct(DiContainer container, ARRaycastManager raycastManager, ARPlaneManager planeManager, Controls controls, GloballGameState globallGameState)
    {
        _container = container;
        _raycastManager = raycastManager;
        _planeManager = planeManager;
        _controls = controls;
        _globalGameState = globallGameState;

        _controls.Gameplay.PrimaryTouch.performed += ctx => Touch();
        _controls.Enable();

        _globalGameState.OnStateChangeEvent += OnGameStateChanged;
    }

    private void Touch()
    {
        Raycast();
    }

    private void Raycast()
    {
        if (_raycastManager.Raycast(_controls.Gameplay.PrimaryTouchPosition.ReadValue<Vector2>(), _hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = _hits[0].pose;
            if(_globalGameState.CurrentState == State.PlaneSelection)
            {
                _currentPlane = _hits[0].trackableId;
                _container.InstantiatePrefab(_testPrefab, pose.position, pose.rotation, null);

                _globalGameState.ChangeCurrentState(State.Gameplay);
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
}
