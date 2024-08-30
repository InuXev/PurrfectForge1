using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedDamage : MonoBehaviour
{
    [SerializeField] float Damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
        {
            PDamage dmg = collision.gameObject.GetComponent<PDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(Damage); // Call the takeDamage function from the PDamage component
            }
        }
        Destroy(gameObject);
    }
}
