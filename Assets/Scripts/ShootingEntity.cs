using System.Collections;
using UnityEngine;
using Zenject;

public class ShootingEntity : MonoBehaviour
{
    [SerializeField] private float recoilTime;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    private ObjectPlacer _objectPlacer;
    private Transform _target;
    private bool _canShoot = true;

    [Inject]
    public void Construct(ObjectPlacer objectPlacer)
    {
        _objectPlacer = objectPlacer;
    }

    private void Start()
    {
        StartCoroutine(WaitForShoot());
    }

    private IEnumerator Recoil(float recoilTime)
    {
        yield return new WaitForSeconds(recoilTime);
        _canShoot = true;
    }

    private IEnumerator WaitForShoot()
    {
        yield return new WaitUntil(() => (_canShoot && _target != null));
        Shoot();
        _canShoot = false;
        StartCoroutine(Recoil(recoilTime));
        StartCoroutine(WaitForShoot());
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity, _objectPlacer.WorldOrigin);
        projectile.transform.up = _target.position - projectile.transform.position;
    }
}