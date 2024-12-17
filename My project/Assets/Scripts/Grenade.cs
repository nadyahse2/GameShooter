using UnityEngine;

public class Grenade: MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionDamage = 30f;
    public float explosionTime = 3f;
    public GameObject explosionEffect; 
    private float timer;
    void Start()
    {
        timer = explosionTime;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Explode();
        }
    }
    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {

            if (collider.TryGetComponent<HealthEnemy>(out HealthEnemy healthEnemy))
            {
                healthEnemy.TakeDamage(explosionDamage);
            }
        }
        
        Destroy(gameObject);
    }

   
}
