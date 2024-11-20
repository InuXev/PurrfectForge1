using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Check if the collided object is the player
        {
            float AttackMod = PlayerManager.Instance.Attack * .1f;
            EDamage dmg = other.gameObject.GetComponent<EDamage>();
            dmg.takeEDamage(itemData.AttPow + AttackMod); // Call the takeDamage function from the IDamage component
        }
    }
}
