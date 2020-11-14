using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    //public GameObject child;

    public Transform devices;

    void ChangeParentRandom()
    {
        /*
        for (int i=0; i=0; i++)
        {
            
        }
        foreach (var item in collection)
        {
            
        }
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {   
            ChangeParentRandom();
        }

        //transform.position = transform.position + new Vector3(0.1f, 0, 0);
    }
}
