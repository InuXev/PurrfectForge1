using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObjectScript : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(10f, 0f * Time.deltaTime, 0f, Space.Self);
    }

}
