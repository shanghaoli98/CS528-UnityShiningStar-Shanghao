using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPos : MonoBehaviour
{


    public Text distanceText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceText.text = Vector3.Distance(transform.position, Vector3.zero).ToString();
    }

    public void Resset()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;

        FindObjectOfType<StarDataManager>().year = 0;
        FindObjectOfType<StarDataManager>().yearText.text = "0 years";
    }
}
