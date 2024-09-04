using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] GameObject trapDart;
    [SerializeField] Transform[] dartHoles;
    public int amountToShoot;
    public float destroyTime;
    private bool activated;
    private float moveSpeed = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !activated)
        {
            StartCoroutine(TrapTriggerTime());
        }
    }
    IEnumerator TrapTriggerTime()
    {
        GameObject sameHoleTwiceCheck = null;
        for (int i = 0; i < amountToShoot; i++)
        {
            int randomDartHoleInt = Random.Range(0, dartHoles.Length);
            if (dartHoles[i] == sameHoleTwiceCheck)
            {
                randomDartHoleInt = Random.Range(0, dartHoles.Length);
            }
            Transform shootHere = dartHoles[randomDartHoleInt];
            yield return new WaitForSeconds(.2F);
            GameObject dart = Instantiate(trapDart, shootHere.position, shootHere.rotation);
            sameHoleTwiceCheck = dart;
            Rigidbody rb = dart.GetComponent<Rigidbody>();
            rb.velocity = Vector3.forward * moveSpeed; //give rigidbody velocity from enemy parameters
            Destroy(dart, destroyTime);
        }
        activated = false;
    }
}
