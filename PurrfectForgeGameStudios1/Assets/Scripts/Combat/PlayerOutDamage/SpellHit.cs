using UnityEngine;

public class SpellHit : MonoBehaviour
{
    [SerializeField] ScriptableSkill skill;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Check if the collided object is the player
        {
            float Damage = skill.Damage * skill.SkillLevel;
            EDamage dmg = other.gameObject.GetComponent<EDamage>();
            dmg.takeEDamage(Damage); // Call the takeDamage function from the IDamage component
        }
        Destroy(gameObject);
    }
}
