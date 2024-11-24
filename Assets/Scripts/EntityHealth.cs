using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected int health;
    public void TakeDamage()
    {
        health--;
        OnDamage();
        if(health <= 0)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDamage()
    {

    }
}