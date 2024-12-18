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
    
    public List<Vector3> coverPoints = new List<Vector3>();
    private Vector3 currentCoverPoint;
    private bool isSeekingCover = false;
    [SerializeField] float radius = 10f;
    [SerializeField] float viewDistance = 15f;
    [SerializeField] private float turnSpeed = 5f;
    private bool active_move = false;
    private Vector3 point;
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
        if(point!= new Vector3(0,0,0))
        {
            if(Vector3.Distance(transform.position, point) <= 3f)
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
    private Vector3 SeekCover()
    {
         
        
        

        Vector3 closestCover = GetClosestFreeCover();
        currentCoverPoint = closestCover;
        agent.destination = currentCoverPoint;
        return currentCoverPoint;


    }
    private Vector3 GetClosestFreeCover()
    {
        Vector3 closestFreeCover = new Vector3(0,0,0);
        float closestDistance = float.MaxValue;

        foreach (Vector3 cover in coverPoints)
        {
            bool isUsed = false;
            foreach (Transform enemy in FindObjectsOfType<Enemy2>().Select(x => x.transform))
            {
                if (enemy != transform && Vector3.Distance(enemy.position, cover) <= 3f)
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
