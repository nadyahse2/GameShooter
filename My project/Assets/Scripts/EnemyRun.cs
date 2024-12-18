using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRun: MonoBehaviour
{
    public Transform Player;
    private float distanse;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyShooter shooter;
    private int anim_num = 0;
    [SerializeField] float radius = 10f;
    [SerializeField] float viewDistance = 15f;
    [SerializeField] private float turnSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shooter = GetComponent<EnemyShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        distanse = Vector3.Distance(Player.transform.position, transform.position);
        if(distanse >= radius)
        {
            agent.enabled = false;
            anim_num = 0;
            animator.SetInteger("anim_num", anim_num);
            shooter.SetShooting(false);
        }
        else if(distanse < radius && distanse>= 3)
        {
            agent.enabled = true;
            agent.destination = Player.position;
            anim_num = 1;
            animator.SetInteger("anim_num", anim_num);
            
            if (CanSeePlayer())
            {
                shooter.SetShooting(true);
                transform.LookAt(Player);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }
        else
        {
          
            agent.enabled = false;
            anim_num = 0;
            animator.SetInteger("anim_num", anim_num);
            
            transform.LookAt(Player);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            if (CanSeePlayer())
            {
                Debug.Log("Shoot");
                shooter.SetShooting(true);

            }
        }

        

        
        
    }
    public void SetPlayer(Transform player)
    {
        Player = player;
    }
    
    private bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 shootingDirection = transform.forward;
        Ray ray = new Ray(transform.position, shootingDirection);
        if (Physics.Raycast(ray, out hit, viewDistance))
        {
            if (hit.transform.gameObject.tag == "Hero")
            {
                return true;
            }
        }
        return false;
    }
   



}
