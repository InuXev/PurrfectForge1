using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTorches : MonoBehaviour
{
    [SerializeField] GameObject BossFires;

    void Update()
    {
        if(PlayerManager.Instance.HasBossKey)
        {
            BossFires.SetActive(true);
        }
        else
        {
            BossFires.SetActive(false);
        }
    }
}
