using System.Collections;
using UnityEngine;

public class ShootingEntity : MonoBehaviour
{
    [SerializeField] private float recoilTime;
    private Transform _target;
    private bool _canShoot = true;

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
        Debug.Log("Shooting at " + _target.name);
    }
}