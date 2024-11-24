using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private int health;
    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            OnDeath();
        }
    }

    protected void OnDeath()
    {
        Destroy(gameObject);
    }
}