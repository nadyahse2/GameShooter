using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    private Transform player;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float damage = 10f;
    [SerializeField] float range = 10f;
    [SerializeField] float inaccuracy = 3f;
    [SerializeField] float snap;

    private float nextFireTime = 0f;
    [SerializeField] float retSpeed;

    private LineRenderer laserLine;
    public Transform gunEnd;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);  

    public bool isShooting = false;


    

    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();
    }
    void Update()
    {
        if (transform.gameObject.GetComponent<EnemyRun>() != null)
        {
            player = transform.gameObject.GetComponent<EnemyRun>().Player;
        }
        if (transform.gameObject.GetComponent<Enemy2>() != null)
        {
            player = transform.gameObject.GetComponent<Enemy2>().Player;
        }



        if (isShooting == true && Time.time >= nextFireTime)
        {
            SingleShot();
            nextFireTime = Time.time + (1f / fireRate);


        }



    }
    public void SetShooting(bool shooting)
    {
        isShooting = shooting;
    }

    private void SingleShot()
    {



        Vector3 shootingDirection = transform.forward;
        Quaternion randomRotation = Quaternion.Euler(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), 0f);
        Ray ray = new Ray(transform.position, randomRotation * shootingDirection);

        StartCoroutine(ShotEffect());


        
        RaycastHit hit;

        
        laserLine.SetPosition(0, gunEnd.position);


        if (Physics.Raycast(ray, out hit, range))
        {



            if (hit.transform.CompareTag("Hero"))
            {
                Debug.Log("Player");
                hit.transform.gameObject.GetComponent<HealthPlayer>().TakeDamage(damage);

            }
        }

        laserLine.SetPosition(1, hit.point);

    }
    private IEnumerator ShotEffect()
    {
        

       
        laserLine.enabled = true;

        yield return shotDuration;

        
        laserLine.enabled = false;
    }
}