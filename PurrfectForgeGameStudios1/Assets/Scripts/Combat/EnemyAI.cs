using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, EDamage
{
    [SerializeField] ScriptableItems[] lootPool; 
    [SerializeField] float LineOfSight;
    [SerializeField] float meleeAttackDistance;
    [SerializeField] GameObject equipedWeapon;
    [SerializeField] EnemyAttack enemyAttack;
    public bool meleeAttackRange;
    public Transform player;
    public Transform headPOS;
    public NavMeshAgent agent;
    public float enemyHP;
    public bool playerInRange;
    private Vector3 playerDir;
    private Vector3 enemyPos;
    private float angleToPlayer;
    private bool isDead;
    public bool attacked;
    Transform playerTransform;
    

    // Start is called before the first frame update
    void Start()
    {
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
        enemyHP -= damage;
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

        if(LineOfSight > distanceToPlayer) 
        { 
            playerInRange = true;
        }
        if (distanceToPlayer <= meleeAttackDistance)
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
        int randomItem = Random.Range(0, lootPool.Length);
        chosenItem = lootPool[randomItem];
        Debug.Log("Chosen Item: " + chosenItem.itemName);
        lootDropper(chosenItem, dropLocation);
    }
    public void lootDropper(ScriptableItems chosenItem, Vector3 dropLocation)
    {
        GameObject droppingItem;
        droppingItem = chosenItem.loot;
        //at this location drop a random object that is in the loot pool
        Instantiate(droppingItem, dropLocation, transform.rotation);
        //drop the item from the headPOS
    }
}
