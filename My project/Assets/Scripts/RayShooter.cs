using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    enum ShootingType { Single, Auto }
    public Camera camera;
    private Coroutine autoShotCoroutine;
    [SerializeField] GameObject hole;
    [SerializeField] float retSpeed;
    [SerializeField] float snap;
    [SerializeField] Vector3 recoilPower;
    [SerializeField] ShootingType shootingType = ShootingType.Auto;
    [SerializeField] float damage;
    [SerializeField] float range = 10f;
    private GameObject bull;
    Vector3 targetRot;
    Vector3 currentRot;
    public GameObject grenadePrefab;
    public Transform throwPoint;
    public int maxGrenades = 3;
    public float throwForce = 5f;
    public float gravity = -18f;
    public float throwCooldown = 1f;

    private int currentGrenades;
    private float nextThrowTime = 0f;
    private bool canThrow = true;

    // Start is called before the first frame update
    void Start()
    {
        currentGrenades = maxGrenades;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, retSpeed * Time.deltaTime);
        currentRot = Vector3.Slerp(currentRot, targetRot, snap * 0.1f);
        transform.localRotation = Quaternion.Euler(currentRot);
        if (Input.GetKeyDown(KeyCode.G) && canThrow && currentGrenades > 0)
        {
            ThrowGrenade();
        }

        if (shootingType == ShootingType.Single)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SingleShot();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                StartAutoShot();
            }
            else
            {
                StopAutoShot();
            }
        }




    }

    void SingleShot()
    {
        if(bull != null)
        {
            Destroy(bull);
        }
        CountRecoil();
        Vector3 screenCenter = new Vector3(camera.pixelWidth / 2, camera.pixelHeight / 2, 0);

        Ray ray = camera.ScreenPointToRay(screenCenter);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,range))
        {
            GameObject obj = hit.transform.gameObject;
            if (obj.tag == "Enemy" )
            {
                
                HealthEnemy health = obj.GetComponent<HealthEnemy>();
                health.TakeDamage(damage);
            }
            else
            {
                bull = Instantiate(hole, hit.point, Quaternion.LookRotation(hit.normal));
            }
            

        }
        

        
       
    }
    void StartAutoShot()
    {
        if (autoShotCoroutine == null)
        {
            autoShotCoroutine = StartCoroutine(AutoShot());
        }
    }
    void StopAutoShot()
    {
        if (autoShotCoroutine != null)
        {
            StopCoroutine(autoShotCoroutine);
            autoShotCoroutine = null;
        }
    }
    private void ThrowGrenade()
    {
        canThrow = false;
        StartCoroutine(ThrowCooldown());

        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation); // Instantiate grenade at throwPoint position
        if (grenade.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(CalculateParabolicVelocity(), ForceMode.Impulse);
        }

        currentGrenades--;
    }
    private Vector3 CalculateParabolicVelocity()
    {
        
        float angle = 45f; // Угол броска
                           // Перевод угла в радианы
        float angleInRadians = angle * Mathf.Deg2Rad;
        // Вычисляем скорость по горизонтали и вертикали
        float initialVelocity = throwForce;
        float velocityY = initialVelocity * Mathf.Sin(angleInRadians);
        Vector3 velocity = transform.forward * initialVelocity * Mathf.Cos(angleInRadians);
        velocity.y = velocityY;
        return velocity;
    }


    public void RestoreGrenades()
    {
        currentGrenades = maxGrenades;
    }
    IEnumerator AutoShot()
    {
        while (true)
        {
            SingleShot();
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator ThrowCooldown()
    {
        yield return new WaitForSeconds(throwCooldown);
        canThrow = true;
    }

    void CountRecoil()
    {
        targetRot += new Vector3(recoilPower.x,
            Random.Range(-recoilPower.y, recoilPower.y),
            Random.Range(-recoilPower.z, recoilPower.z));
    }
    
}