using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlanePlacer : MonoBehaviour
{
    public Transform PlayerPrefab;
    private Transform Player;
    public Transform Enemy_Run_Prefab;
    private Transform Enemy_Run;
    public Transform Enemy2_Pref;
    private Transform Enemy2;
    public Block[] BlockPrefabs;
    public List<Transform> enemies;
    public List<Transform> enemies2;
    private float playerSpawnOffset = 2f;
    public int level = 0;


    private List<Block> SpawnedBlocks = new List<Block>();
    private List<Block> blockBag = new List<Block>();
    // Start is called before the first frame update
    void Start()
    {
        InitilizeBlockBag();
        Block FirstBlock = Instantiate(GetNextBlock());
        SpawnedBlocks.Add(FirstBlock);
        enemies = FirstBlock.spawn_point1;
        enemies2 = FirstBlock.spawn_point2;
        Vector3 spawnPosition = SpawnedBlocks[0].Begin.position;



        Player = Instantiate(PlayerPrefab, spawnPosition + new Vector3(0,2f,playerSpawnOffset), Quaternion.identity);
        SpawnEnemies();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameObject.FindObjectsOfType<EnemyRun>().Length);
        Debug.Log(GameObject.FindObjectsOfType<Enemy2>().Length);
        if((Player.position.z > SpawnedBlocks[SpawnedBlocks.Count - 1].End.position.z - 5 && SpawnedBlocks[SpawnedBlocks.Count - 1].tag == "Block_Up" || 
            Player.position.x > SpawnedBlocks[SpawnedBlocks.Count - 1].End.position.x - 5 && SpawnedBlocks[SpawnedBlocks.Count - 1].tag == "Block_Right" || 
            Player.position.z < SpawnedBlocks[SpawnedBlocks.Count - 1].End.position.z + 5 && SpawnedBlocks[SpawnedBlocks.Count - 1].tag == "Block_Down" || 
            Player.position.x < SpawnedBlocks[SpawnedBlocks.Count - 1].End.position.x + 5 && SpawnedBlocks[SpawnedBlocks.Count - 1].tag == "Block_Left") && (GameObject.FindObjectsOfType<EnemyRun>().Length == 0 && GameObject.FindObjectsOfType<Enemy2>().Length == 0))
        {
            SpawnedBlocks[0].door.SetActive(false);
            SpawnedBlocks[0].door.GetComponent<BoxCollider>().enabled = false;
            SpawnBlock();
            SpawnEnemies();
            SpawnedBlocks[SpawnedBlocks.Count - 1].door2.SetActive(false);
            SpawnedBlocks[SpawnedBlocks.Count - 1].door2.GetComponent<BoxCollider>().enabled = false;
            RayShooter rayShooter = Player.GetComponent<RayShooter>();
            rayShooter.RestoreGrenades();
        }
        
        if (SpawnedBlocks.Count >= 2)
        {
            if (Player.position.z > SpawnedBlocks[0].End.position.z+1 && SpawnedBlocks[0].tag == "Block_Up" || 
                Player.position.x > SpawnedBlocks[0].End.position.x+1 && SpawnedBlocks[0].tag == "Block_Right" ||
                Player.position.x < SpawnedBlocks[0].End.position.x - 1 && SpawnedBlocks[0].tag == "Block_Left" ||
                Player.position.z < SpawnedBlocks[0].End.position.z - 1 && SpawnedBlocks[0].tag == "Block_Down" 
                )
            {
                SpawnedBlocks[SpawnedBlocks.Count - 1].door.SetActive(true);
                SpawnedBlocks[SpawnedBlocks.Count - 1].door2.SetActive(true);
                SpawnedBlocks[SpawnedBlocks.Count - 1].door.GetComponent<BoxCollider>().enabled = true;
                SpawnedBlocks[SpawnedBlocks.Count - 1].door2.GetComponent<BoxCollider>().enabled = true;
                Destroy(SpawnedBlocks[0].gameObject);
                SpawnedBlocks.RemoveAt(0);
            }
        }
    }

    private void InitilizeBlockBag()
    {
        blockBag.AddRange(BlockPrefabs);
    }
    private Block GetNextBlock()
    {
        if (blockBag.Count == 0)
        {
            InitilizeBlockBag();
        }
        int randomIndex = Random.Range(0, blockBag.Count);
        Block selectedBlock = blockBag[randomIndex];
        blockBag.RemoveAt(randomIndex);

        return selectedBlock;
    }
    private void SpawnBlock()
    {
        level++;

        Block previousBlock = SpawnedBlocks[SpawnedBlocks.Count - 1];
        Block newBlock = Instantiate(GetNextBlock());
        enemies = newBlock.spawn_point1;
        enemies2 = newBlock.spawn_point2;

        if (SpawnedBlocks[SpawnedBlocks.Count - 1].tag == "Block_Right")
        {
            newBlock.transform.Rotate(0f, 90f, 0f);
            if (newBlock.Right_connection == true)
            {
                newBlock.tag = "Block_Down";
            }
            else { newBlock.tag = "Block_Right"; }

        }
        if (SpawnedBlocks[SpawnedBlocks.Count - 1].tag == "Block_Down")
        {
            newBlock.transform.Rotate(0f, 180f, 0f);
            if (newBlock.Right_connection == true)
            {
                newBlock.tag = "Block_Left";
            }
            else { newBlock.tag = "Block_Down"; }

        }
        if (SpawnedBlocks[SpawnedBlocks.Count - 1].tag == "Block_Left")
        {
            newBlock.transform.Rotate(0f, -90f, 0f);
            if (newBlock.Right_connection == true)
            {
                newBlock.tag = "Block_Up";
            }
            else { newBlock.tag = "Block_Left"; }

        }
        newBlock.transform.position = SpawnedBlocks[SpawnedBlocks.Count - 1].End.position - newBlock.Begin.position;


        SpawnedBlocks.Add(newBlock);

    }
    void SpawnEnemies()
    {

        
        if (level < 2)
        {
            for (int i = 0; i < level+1; i++)
            {
                Vector3 spawnPosition3 = enemies2[i].position;
                Enemy2 = Instantiate(Enemy2_Pref, spawnPosition3, Quaternion.identity);

                Enemy2 enemy2 = Enemy2.GetComponent<Enemy2>();
                HealthEnemy health2 = Enemy2.GetComponent<HealthEnemy>();
                health2.health *= (level + 1);
                enemy2.SetPlayer(Player);
                enemy2.coverPoints = enemies2;
                
            }
        }
        else
        {
            for (int i = 0; i < enemies2.Count; i++)
            {
                Vector3 spawnPosition3 = enemies2[i].position;
                Enemy2 = Instantiate(Enemy2_Pref, spawnPosition3, Quaternion.identity);
                Enemy2 enemy2 = Enemy2.GetComponent<Enemy2>();
                HealthEnemy health2 = Enemy2.GetComponent<HealthEnemy>();
                health2.health *= (level + 1);

                enemy2.SetPlayer(Player);
                enemy2.coverPoints = enemies2;
            }
        }

        if (level < 3)
        {
            for (int i = 0; i < level +1; i++)
            {
                Vector3 spawnPosition3 = enemies[i].position;
                Enemy_Run = Instantiate(Enemy_Run_Prefab, spawnPosition3, Quaternion.identity);
                EnemyRun enemy = Enemy_Run.GetComponent<EnemyRun>();
                HealthEnemy health = Enemy_Run.GetComponent<HealthEnemy>();
                health.health *= (level + 1);
                enemy.SetPlayer(Player);
               
            }
        }
        else
        {
            for (int i = 0; i < enemies.Count-1; i++)
            {
                Vector3 spawnPosition3 = enemies[i].position;
                Enemy_Run = Instantiate(Enemy_Run_Prefab, spawnPosition3, Quaternion.identity);
                EnemyRun enemy = Enemy_Run.GetComponent<EnemyRun>();
                HealthEnemy health = Enemy_Run.GetComponent<HealthEnemy>();
                health.health *= (level + 1);
                enemy.SetPlayer(Player);
            }
        }
        
        
        


    }

}
