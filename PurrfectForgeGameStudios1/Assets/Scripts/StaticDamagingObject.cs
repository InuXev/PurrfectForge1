using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDamagingObject : MonoBehaviour
{
    [SerializeField] float Damage;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collided object is the player
        {
            PDamage dmg = other.gameObject.GetComponent<PDamage>();
            dmg.takeDamage(Damage); // Call the takeDamage function from the IDamage component
        }
    }
}
