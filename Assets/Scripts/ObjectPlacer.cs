using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

public class ObjectPlacer
{
    public Transform playerBase {  get; private set; }

    [Inject(Id = "TestPrefab")]
    private readonly Spaceship _testPrefab;

    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;
    private Controls _controls;
    private DiContainer _container;
    private GlobalGameState _globalGameState;

    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private TrackableId _currentPlane;
    private Spaceship _objectPrefab;
    private PlayerMoney _playerMoney;
    private Transform _worldOrigin;
    private NavMeshSurface _navMeshSurface;

    [Inject]
    public void Construct(DiContainer container, ARRaycastManager raycastManager, ARPlaneManager planeManager, Controls controls, GlobalGameState globallGameState, PlayerMoney playerMoney, 
        NavMeshSurface navMeshSurface)
    {
        _container = container;
        _raycastManager = raycastManager;
        _planeManager = planeManager;
        _controls = controls;
        _globalGameState = globallGameState;
        _playerMoney = playerMoney;
        _navMeshSurface = navMeshSurface;

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
                    _worldOrigin = _hits[0].trackable.gameObject.transform;
                    _navMeshSurface.transform.SetParent(_worldOrigin);
                    _navMeshSurface.transform.position = pose.position;
                    _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
                    playerBase = PlaceObject(_objectPrefab.Prefab, pose.position, pose.rotation).transform;
                    _objectPrefab = null;

                    _globalGameState.ChangeCurrentState(State.Gameplay);
                }
                else if(_globalGameState.CurrentState == State.Gameplay)
                {
                    if (_playerMoney.CheckPurchaseAbility(_objectPrefab.Cost))
                    {
                        _playerMoney.BuyItem(_objectPrefab.Cost);
                        PlaceObject(_objectPrefab.Prefab, pose.position, pose.rotation);
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
                    else plane.gameObject.isStatic = true;
                }
                _planeManager.requestedDetectionMode = PlaneDetectionMode.None;
                _navMeshSurface.BuildNavMesh();
                break;
            default:
                break;
        }
    }

    public void SetCurrentObject(Spaceship obj)
    {
        _objectPrefab = obj;
    }

    public GameObject PlaceObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return _container.InstantiatePrefab(prefab, position, rotation, _worldOrigin);
    }

    public GameObject PlaceEnemyShip(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return _container.InstantiatePrefab(prefab, position, rotation, _navMeshSurface.transform);
    }
}
