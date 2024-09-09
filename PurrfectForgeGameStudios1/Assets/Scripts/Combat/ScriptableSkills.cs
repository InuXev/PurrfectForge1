using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
//using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

[CreateAssetMenu]

public class ScriptableSkill : ScriptableObject
{
    // Start is called before the first frame update
    public enum Element { Fire, Ice, Lightning }
    public Element element;
    public string SkillName;
    public float SkillLevel;
    public float SkillCost;
    public float Damage;
    public string Description;
    public GameObject objectToShoot;
    public float bulletSpeed;
    public float DestroyTime;
    public Sprite SkillSlotImage;
    bool casting = false;


    public void SkillBehavior(Transform shootPos)
    {
        if(!casting)
        {
            casting = true;
            GameObject round = Instantiate(objectToShoot, shootPos.position, shootPos.rotation); //create the object assigned in the enemy scriptable
            Rigidbody rb = round.GetComponent<Rigidbody>(); //rigid body 
            rb.velocity = shootPos.forward * bulletSpeed; //give rigidbody velocity from enemy parameters
            Destroy(round, DestroyTime); //destroy if no impact after scriptable time
            casting = false; //flip shooting flag to be able to shoot again
        }
    }
}
