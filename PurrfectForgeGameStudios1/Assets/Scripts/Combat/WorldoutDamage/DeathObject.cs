using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathObject : MonoBehaviour
{
    [SerializeField] float Damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collided object is the player
        {
            PDamage dmg = other.gameObject.GetComponent<PDamage>();
            dmg.takeDamage(Damage); // Call the takeDamage function from the IDamage component
        }
    }
}
