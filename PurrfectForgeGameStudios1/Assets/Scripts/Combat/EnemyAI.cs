using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, EDamage
{
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
    private float angleToPlayer;
    private bool isDead;
    public bool attacked;
    

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
        if (enemyHP <= 0)
        {
            isDead = true;
            Destroy(gameObject);
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
}
