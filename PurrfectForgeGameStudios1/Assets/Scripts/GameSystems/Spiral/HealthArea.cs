using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthArea : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collided object is the player
        {
            HealHit dmg = other.gameObject.GetComponent<HealHit>();
            dmg.takeHeal(); // Call the takeDamage function from the IDamage component
        }
    }

}
