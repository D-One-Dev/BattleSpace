using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyNavigation : MonoBehaviour
{
    [Inject(Id = "PlayerShipsLayer")]
    private readonly LayerMask _playerShipsLayer;
    [SerializeField] private float spaceshipDetectRadius;
    [SerializeField] private float shootRadius;
    [SerializeField] private ShootingEntity shootingEntity;
    private Transform _playerBase;
    private ObjectPlacer _objectPlacer;
    private NavMeshAgent _agent;
    private Transform _currentTarget;

    [Inject]
    public void Construct(ObjectPlacer objectPlacer)
    {
        _objectPlacer = objectPlacer;
        _playerBase = _objectPlacer.playerBase;
        _agent = GetComponent<NavMeshAgent>();
        _currentTarget = _playerBase;
    }

    private void Start()
    {
        _agent.destination = _currentTarget.position;
    }

    private void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, spaceshipDetectRadius, _playerShipsLayer);
        if(hits.Length > 0)
        {
            foreach(Collider hit in hits)
            {
                if(_currentTarget == null || (_currentTarget != null && hit.gameObject.transform != _currentTarget &&
                    Vector3.Distance(transform.position, hit.gameObject.transform.position) <
                    Vector3.Distance(transform.position, _currentTarget.position)))
                {
                    _currentTarget = hit.gameObject.transform;
                }
            }
            if(_currentTarget != null) _agent.destination = _currentTarget.position;
        }
        else if(_playerBase != null)
        {
            _currentTarget = _playerBase;
            _agent.destination = _currentTarget.position;
        }

        Collider[] shoothits = Physics.OverlapSphere(transform.position, shootRadius, _playerShipsLayer);
        if (shoothits.Length > 0)
        {
            foreach (Collider hit in shoothits)
            {
                if (hit.gameObject.transform == _currentTarget)
                {
                    shootingEntity.SetTarget(_currentTarget);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spaceshipDetectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRadius);
    }
}