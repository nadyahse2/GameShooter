using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    public Transform Player;
    private float distanse;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyShooter shooter;
    private int anim_num = 0;
    public int health = 25;
    public List<Transform> coverPoints = new List<Transform>();
    private Transform currentCoverPoint;
    private bool isSeekingCover = false;
    [SerializeField] float radius = 10f;
    [SerializeField] float viewDistance = 15f;
    [SerializeField] private float turnSpeed = 5f;
    private bool active_move = false;
    private Transform point;
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
        if(point!= null)
        {
            if(Vector3.Distance(transform.position, point.position) <= 1f)
            {
                active_move = false;
            }
        }

        if (distanse <= radius && distanse >= 3 && !active_move)
        {
            agent.enabled = false;
            
            anim_num = 0;
            animator.SetInteger("anim_num", anim_num);
            transform.LookAt(Player);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            if (CanSeePlayer())
            {
                shooter.SetShooting(true);
            }
        }
        else if(distanse <= radius && distanse < 3)
        {
            isSeekingCover = true;
            agent.enabled = true;
            point = SeekCover();
            shooter.SetShooting(false);
            active_move = true;
            anim_num = 1;
            animator.SetInteger("anim_num", anim_num);

            
        }
        else { shooter.SetShooting(false); }



        if (health <= 0)
        {
            Destroy(gameObject);
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
    private Transform SeekCover()
    {
         
        
        

        Transform closestCover = GetClosestFreeCover();
        currentCoverPoint = closestCover;
        agent.destination = currentCoverPoint.position;
        return currentCoverPoint;


    }
    private Transform GetClosestFreeCover()
    {
        Transform closestFreeCover = null;
        float closestDistance = float.MaxValue;

        foreach (Transform cover in coverPoints)
        {
            bool isUsed = false;
            foreach (Transform enemy in FindObjectsOfType<Enemy2>().Select(x => x.transform))
            {
                if (enemy != transform && Vector3.Distance(enemy.position, cover.position) <= 1f)
                {
                    isUsed = true;
                    break;
                    
                }

                
            }
            if (!isUsed)
            {
                
               closestFreeCover = cover;
                
            }
        }
        return closestFreeCover;
    }
}
