using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, EDamage
{

    [SerializeField] ScriptableEnemies enemyParams;
    [SerializeField] GameObject equipedWeapon;
    [SerializeField] EnemyAttack enemyAttack;
    public bool meleeAttackRange;
    public Transform player;
    public Transform headPOS;
    public NavMeshAgent agent;
    float enemyHP;
    public bool playerInRange;
    private Vector3 playerDir;
    private Vector3 enemyPos;
    private float angleToPlayer;
    private bool isDead;
    public bool attacked;
    Transform playerTransform;
    public float xpDrop;
    

    // Start is called before the first frame update
    void Start()
    {
        enemyHP = enemyParams.HP;
        meleeAttackRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDeathCheck();
        canSeePlayer();
        LookAtPlayer();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void takeDamage(float damage)
    {
        float defMod = enemyParams.Defense * .1F;
        enemyHP -= damage + (PlayerManager.Instance.Attack * .1F) - defMod;
        EnemyDeathCheck();
    }

    bool canSeePlayer()
    {
        if (isDead) // Check if the enemy is dead
        {
            return false; // Return false
        }
        playerDir = player.transform.position - headPOS.position; // Get the direction to the player
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward); // Get the angle to the player
        float distanceToPlayer = Vector3.Distance(player.transform.position, headPOS.position);
        //wave AI
        //    agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the agent to the player's position

        if(enemyParams.LineOfSight > distanceToPlayer) 
        { 
            playerInRange = true;
        }
        if (distanceToPlayer <= enemyParams.MeleeAttackDistance)
        {
            meleeAttackRange = true;
        }
        if (meleeAttackRange)
        {
            if(!attacked)
            {
                StartCoroutine(Attack());
                enemyAttack.weaponUsed = false;
            }
        }
        if (playerInRange)
        {
            RaycastHit hit; // Create a raycast hit variable
            if (Physics.Raycast(headPOS.position, playerDir, out hit)) // Check if the raycast hits something
            {
                if (hit.collider.CompareTag("Player") && playerInRange) // Check if the object hit is the player
                {
                    agent.SetDestination(player.transform.position); // Set the destination of the agent to the player's position
                    return true; // Return true
                }
            }
        }
        return false; // Return false
    }

    public void EnemyDeathCheck()
    {
        enemyPos = this.transform.position;
        if (enemyHP <= 0)
        {
            isDead = true;
            Vector3 dropLocation = enemyPos;
            Destroy(gameObject);
            LootPicker(dropLocation);
            XPGiver();
        }
    }

    public IEnumerator Attack()
    {
        attacked = true;
        enemyAttack.weaponUsed = true;
        equipedWeapon.SetActive(true);
        yield return new WaitForSeconds(.1F);
        equipedWeapon.SetActive(false);
        yield return new WaitForSeconds(1F);
        enemyAttack.weaponUsed = false;
        attacked = false;

    }

    public void LookAtPlayer()
    {
        if (playerInRange && meleeAttackRange)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //smooth sphereical rotations
        }
    }

    public void LootPicker(Vector3 dropLocation)
    {
        ScriptableItems chosenItem;
        //pick the loot from the loot pool and assign it to chosenItem
        int randomItem = Random.Range(0, enemyParams.lootPool.Length);
        chosenItem = enemyParams.lootPool[randomItem];
        Debug.Log("Chosen Item: " + chosenItem.itemName);
        lootDropper(chosenItem, dropLocation);
    }
    public void lootDropper(ScriptableItems chosenItem, Vector3 dropLocation)
    {
        GameObject droppingItem;
        droppingItem = chosenItem.loot;
        if(chosenItem.itemName == "Coin")
        {
            for (int i = 0; i < enemyParams.Level * 3; i++)
            {
                int dropLocationX = Random.Range(0, 1);
                int dropLocationZ = Random.Range(0, 1);
                float dropLocationY = Random.Range(0, .2F);
                Vector3 RandomVectorLocation = new Vector3(dropLocation.x + dropLocationX, dropLocation.y + dropLocationY, dropLocation.z + dropLocationZ);
                Instantiate(droppingItem, RandomVectorLocation /*dropLocation*/, transform.rotation);
            }
        }
        else
        {
            //at this location drop a random object that is in the loot pool
            int dropLocationX = Random.Range(0, 1);
            int dropLocationZ = Random.Range(0, 2);
            Vector3 RandomVectorLocation = new Vector3(dropLocation.x + dropLocationX, dropLocation.y + 0, dropLocation.z + dropLocationZ);
            Instantiate(droppingItem, RandomVectorLocation, transform.rotation);
            //drop the item from the headPOS
        }

    }
    public void XPGiver()
    {
        float xpDrop = enemyParams.XpDrop;
        PlayerManager.Instance.playerXP += xpDrop;
    }
}
