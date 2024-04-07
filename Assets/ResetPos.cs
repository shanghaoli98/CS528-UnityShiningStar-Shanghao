using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resset()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
    }
}
