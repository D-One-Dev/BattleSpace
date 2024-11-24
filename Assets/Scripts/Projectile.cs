using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lifetime;

    private void Start()
    {
        rb.linearVelocity = transform.up * projectileSpeed;
        StartCoroutine(Lifetime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<EntityHealth>(out EntityHealth entityHealth))
        {
            entityHealth.TakeDamage();
        }
        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}