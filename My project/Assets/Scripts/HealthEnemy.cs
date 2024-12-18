using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public float health = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
        else { Destroy(gameObject); }
        
    }
}
