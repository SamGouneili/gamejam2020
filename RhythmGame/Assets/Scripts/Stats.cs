using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Stats : MonoBehaviour
{

	public GameObject perfectRate;
	public GameObject hitRate;
    // Start is called before the first frame update
    void Start()
    {
        perfectRate.GetComponent<Text>().text = "Perfect Hit Rate: 100%";
        perfectRate.GetComponent<Text>().text = "Hit Rate: 100%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
