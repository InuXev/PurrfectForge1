using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, EDamage
{
    [SerializeField] GameObject Key; 
    [SerializeField] ScriptableEnemies enemyParams; 
    [SerializeField] GameObject equipedWeapon; 
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] Transform ShootPos;
    public bool meleeAttackRange;
    public Transform player;
    public Transform headPOS;
    public NavMeshAgent agent;
    float enemyHP;
    public bool playerInRange;
    private Vector3 playerDir; //direction to player
    private Vector3 enemyPos;
    private float angleToPlayer;
    private bool isDead;
    public bool attacked; //tracks attacks so the dont overlap
    Transform playerTransform;
    public float xpDrop;
    public bool shooting = false;
    int swingCount = 0; //tracks swings to go into other modes
    bool spinAttacking = false; //tracks for spinning attack

    void Start()
    {
        enemyHP = enemyParams.HP;
        meleeAttackRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDeathCheck(); //checks for enemys death
        canSeePlayer(); //checks if the player can be seen and what to do
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //grabs player Transform for loaction info
    }

    public void takeEDamage(float damage) //player takes damage from enemy
    {
        float defMod = enemyParams.Defense * .1F; //defense reduction based on 3% of def
        enemyHP -= damage + (PlayerManager.Instance.Attack * .1F) - defMod;//how much hp is lost aftertakeing into account player attack and the enemy defMod
        EnemyDeathCheck(); //checks to see if they died
    }


    bool canSeePlayer()
    {
        if (isDead) // Check if the enemy is dead
        {
            return false; // Return false
        }
        playerDir = player.transform.position - headPOS.position; // Get the direction to the player
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward); // Get the angle to the player
        float distanceToPlayer = Vector3.Distance(player.transform.position, headPOS.position); //checks distance to player

        if (enemyParams.LineOfSight > distanceToPlayer) //if the LOS is higher than distance to player
        {
            playerInRange = true; //player in range
            LookAtPlayer(); //rotates to look at player

        }
        if (distanceToPlayer <= enemyParams.MeleeAttackDistance) //checks to see if they are in attack distance
        {
            if (enemyParams.type != ScriptableEnemies.Type.Ranged) //if they are anytype but ranged
            {
                meleeAttackRange = true; //flip the attack flag
            }
        }
        if (meleeAttackRange) //if the attack flag is true
        {
            if (!attacked && !spinAttacking) //if we arent attacking or spinattacking
            {
                if (swingCount < 4) //if the sweingCount is under 4(3)
                {
                    StartCoroutine(Attack()); //normal attack
                    enemyAttack.weaponUsed = false; //flip the weapon is used so the anim on the enemy knows 
                }
                else //if the swing count is 3
                {

                    StartCoroutine(SpinAttack()); //spin attack
                    swingCount = 0; //reset swingCount 
                    enemyAttack.weaponUsed = false; //show weapon is not longer being used and put it away
                }
            }
        }
        if (playerInRange) //if the player is in range
        {
            RaycastHit hit; // Create a raycast hit variable
            if (Physics.Raycast(headPOS.position, playerDir, out hit)) // Check if the raycast hits something
            {
                if (hit.collider.CompareTag("Player") && playerInRange) // Check if the object hit is the player
                {
                    if (enemyParams.type == ScriptableEnemies.Type.Normal || enemyParams.type == ScriptableEnemies.Type.Boss)
                    {
                        agent.SetDestination(player.transform.position); // Set the destination of the agent to the player's position
                        return true; // Return true
                    }
                    if (enemyParams.type == ScriptableEnemies.Type.Ranged && !shooting) //if its a ranged enemy and they arent attacking
                    {
                        agent.stoppingDistance = 7; //stopping distance set to 7
                        agent.SetDestination(player.transform.position); //agent heads to player
                        shooting = true; //shooting flag is set to true to prevent multi shots
                        StartCoroutine(Shoot()); //shoot
                    }
                }
            }
        }
        if (enemyParams.type == ScriptableEnemies.Type.Wave) //if the enemy type is wave
        {
            agent.SetDestination(player.transform.position); // Set the destination of the agent to the player's position
        }
        return false; // Return false
    }

    public void EnemyDeathCheck() //checks enemy death
    {
        enemyPos = this.transform.position; //grab enemy position
        if (enemyHP <= 0) //is enemy at 0 health
        {
            isDead = true; //they dead
            Vector3 dropLocation = enemyPos; //set this for a itemdrop location
            Destroy(gameObject); //destroy enemy
            LootPicker(dropLocation); //pick and drop the item from lootpool in unity
            XPGiver(); //call the xp give
        }
    }

    public IEnumerator Attack() //attack
    {
        attacked = true; //flip to prevent multi attacks
        swingCount++; //increase swing count
        enemyAttack.weaponUsed = true; //show weapon being used
        equipedWeapon.SetActive(true); //turn on the weapon to be used
        yield return new WaitForSeconds(0.1f); //wait for a moment to flash the weapon
        equipedWeapon.SetActive(false); //turn off the weapon
        yield return new WaitForSeconds(1f); //wait another second to prevent quick attacks
        enemyAttack.weaponUsed = false; //turn the attack flag off to allow next attack
        attacked = false; //set attack to false to allow next attack
    }

    public IEnumerator SpinAttack()
    {
        //yield return new WaitForSeconds(enemyParams.BossAttackPause);
        spinAttacking = true; //set spinng attack to true to lock into this attack
        enemyAttack.weaponUsed = true; //show weapon being used
        equipedWeapon.SetActive(true);//turn on weapon

        // Start the spinning rotation
        float totalRotation = 360f; // Total degrees to rotate
        float duration = .5f; // Duration in seconds
        float rotationSpeed = totalRotation / duration; // Degrees per second
        float rotationAmount = 0f; //how far into the rotation enemy is

        // Perform the 360-degree rotation
        while (rotationAmount < totalRotation) //if rotation is less than wanted
        {
            float rotationStep = rotationSpeed * Time.deltaTime; // Calculate the rotation step
            transform.Rotate(Vector3.up, rotationStep, Space.World); //rotate the object
            rotationAmount += rotationStep; //increase current rotation by step

            // Prevent overshooting
            if (rotationAmount > totalRotation) //if rotation is greater than wanted
            {
                float overshoot = rotationAmount - totalRotation; //amount wanted
                transform.Rotate(Vector3.up, -overshoot, Space.World); // Correct overshoot
                rotationAmount = totalRotation; //set 
            }

            yield return null; // Wait for the next frame
        }

        // Ensure facing the player directly
        Vector3 direction = (playerTransform.position - transform.position).normalized; //grab direction to player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); //grab rotation to player
        transform.rotation = lookRotation; //set the rotation 

        yield return new WaitForSeconds(.1f); // Adjust this duration as needed

        equipedWeapon.SetActive(false); //turn off the weapon
        spinAttacking = false; //flip spinning flag for next spin
    }

    public void LookAtPlayer() //looks at player
    {
        if (enemyParams.type != ScriptableEnemies.Type.Ranged && !spinAttacking) // Anything but ranged and not spin attacking
        {
            if (playerInRange && meleeAttackRange) //check range and melee range bools
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized; //grab direction to player
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); //grab rotation to player
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //rotate the enemy
            }
        }

        if (enemyParams.type == ScriptableEnemies.Type.Ranged) //if ranged
        {
            if (playerInRange)// if the player is in range
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized; //grab direction to player
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); //grab rotation to player
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //rotate the enemy
            }
        }
    }
    public void LootPicker(Vector3 dropLocation) //picks the loot
    {
        ScriptableItems chosenItem; //create the scriptable holder
        int randomItem = Random.Range(0, enemyParams.lootPool.Length); //pick the loot from the loot pool and assign it to chosenItem
        chosenItem = enemyParams.lootPool[randomItem]; //assign from the loot pool in unity in the enemy scriptable
        Debug.Log("Chosen Item: " + chosenItem.itemName);
        lootDropper(chosenItem, dropLocation); //call loot dropper to instantiate
    }
    public void lootDropper(ScriptableItems chosenItem, Vector3 dropLocation) //drops random chosen loot
    {
        GameObject droppingItem; //create the gameobject holder
        droppingItem = chosenItem.loot; //assign it to the passed in Scriptable
        if (chosenItem.itemName == "Coin") //if its a coin
        {
            for (int i = 0; i < enemyParams.Level * 3; i++) //frop 3 coins per enemy level
            {
                int dropLocationX = Random.Range(0, 1); //calc random X
                int dropLocationZ = Random.Range(0, 1); //calc random z
                float dropLocationY = Random.Range(0, .2F); //calc random y
                Vector3 RandomVectorLocation = new Vector3(dropLocation.x + dropLocationX, dropLocation.y + dropLocationY, dropLocation.z + dropLocationZ); //randomized drop location
                Instantiate(droppingItem, RandomVectorLocation /*dropLocation*/, transform.rotation); //drop it
            }
        }
        else //anything other than a coin
        {
            int dropLocationX = Random.Range(0, 1); //random X
            int dropLocationZ = Random.Range(0, 2); //ranfom y
            Vector3 RandomVectorLocation = new Vector3(dropLocation.x + dropLocationX, dropLocation.y + 0, dropLocation.z + dropLocationZ);//randomized drop location
            Instantiate(droppingItem, RandomVectorLocation, transform.rotation); //drop it

        }
        if (Key != null) //if a key is assigned, for floor bosses only
        {
            int dropLocationX = Random.Range(0, 1); //random x
            int dropLocationZ = Random.Range(0, 2); //random y
            Vector3 RandomVectorLocation = new Vector3(dropLocation.x + dropLocationX, dropLocation.y + 0, dropLocation.z + dropLocationZ);//randomized drop location
            Instantiate(Key, RandomVectorLocation, transform.rotation);//drop it
        }

    }
    public void XPGiver() //Gives XP to player
    {
        float xpDrop = enemyParams.XpDrop; //from the enemy scriptable assigned in unity
        PlayerManager.Instance.playerXP += xpDrop;//add xp
    }

    IEnumerator Shoot() //shoot
    {
        yield return new WaitForSeconds(enemyParams.ShootRate); //rounds per second
        GameObject round = Instantiate(enemyParams.objectToShoot, ShootPos.position, ShootPos.rotation); //create the object assigned in the enemy scriptable
        Rigidbody rb = round.GetComponent<Rigidbody>(); //rigid body 
        rb.velocity = playerDir * enemyParams.bulletSpeed; //give rigidbody velocity from enemy parameters
        Destroy(round, enemyParams.DestroyTime); //destroy if no impact after scriptable time
        shooting = false; //flip shooting flag to be able to shoot again
    }
}