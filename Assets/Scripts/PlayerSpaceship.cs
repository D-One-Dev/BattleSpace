using UnityEngine;
using Zenject;

public class PlayerSpaceship : MonoBehaviour
{
    [Inject(Id = "EnemyShipsLayer")]
    private readonly LayerMask _enemyShipsLayer;
    [SerializeField] private float spaceshipDetectRadius;
    [SerializeField] private float shootRadius;
    [SerializeField] private ShootingEntity shootingEntity;
    private Transform _currentTarget;
    private void FixedUpdate()
    {
        if(_currentTarget != null)
        {
            Vector3 target = _currentTarget.position - transform.position;
            transform.forward = new Vector3(target.x, transform.forward.y, target.z);
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, spaceshipDetectRadius, _enemyShipsLayer);
        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                if (_currentTarget == null)
                {
                    _currentTarget = hit.gameObject.transform;
                    break;
                }
                if (hit.gameObject.transform != _currentTarget &&
                    Vector3.Distance(transform.position, hit.gameObject.transform.position) <
                    Vector3.Distance(transform.position, _currentTarget.position))
                {
                    _currentTarget = hit.gameObject.transform;
                }
            }
        }
        else _currentTarget = null;

        Collider[] shoothits = Physics.OverlapSphere(transform.position, shootRadius, _enemyShipsLayer);
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
