using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    [SerializeField] float Damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Check if the collided object is the player
        {
            EDamage dmg = other.gameObject.GetComponent<EDamage>();
            dmg.takeEDamage(Damage); // Call the takeDamage function from the IDamage component
        }
    }
}
