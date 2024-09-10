using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpObject : MonoBehaviour
{
    [SerializeField] Transform StartPOS;
    [SerializeField] Transform EndPOS;
    private Transform flipHolder;
    public float timeToEnd;

    public float timeGoneBy = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = StartPOS.position;
    }

    // Update is called once per frame
    void Update()
    {
        timeGoneBy += Time.deltaTime;

        float t = timeGoneBy / timeToEnd;
        transform.position = Vector3.Lerp(StartPOS.position, EndPOS.position, t);

        if(t >= 1)
        {
            transform.position = EndPOS.position;
            timeGoneBy = 0F;
            flipHolder= StartPOS;
            StartPOS = EndPOS;
            EndPOS = flipHolder;
        }
    }
}
