using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    //public GameObject child;

    public GameObject deviceManager;
    public GameObject target;
    public GameObject selectionArrow;
    Renderer arrowRenderer;

    // vvv incorrect as shite vvv
    //List<GameObject> nearbyDevices = deviceManager.nearbyDevices;
    public List<Connection> nearbyDevices = new List<Connection>();

    GameObject FindTarget(Vector2 joystickVector, List<GameObject> nearbyDevices)
    {
        float dx;
        float dz;
        GameObject closestDevice = null;
        Vector2 deviceVector;

        float deviceAngle;
        float angleDiff;
        float smallestDiff = 360f;

        foreach (GameObject device in nearbyDevices)
        {
            dx = device.transform.position.x - transform.position.x;
            dz = device.transform.position.z - transform.position.z;
            deviceVector = new Vector2(dx, dz);
            
            angleDiff = Vector2.Angle(joystickVector, deviceVector);

            if(angleDiff < smallestDiff)
            {
                closestDevice = device;
                smallestDiff = angleDiff;
            }
        }
        
        return closestDevice;
    }


    void HighlightTarget(GameObject target)
    {
        selectionArrow.transform.SetParent(target.transform, false);
        //selectionArrow.GetComponent<Renderer>().enabled = true;
        arrowRenderer.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        arrowRenderer = selectionArrow.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Debug.Log(horizontalInput.ToString() + ", " + verticalInput.ToString());
        //Debug.Log(verticalInput);

        if((horizontalInput != 0) || (verticalInput != 0))
        {
            Vector2 joystickVector = new Vector2(horizontalInput, verticalInput);
            //GameObject target = FindTarget(joystickVector, nearbyDevices);
            //DrawLineToTarget();
            HighlightTarget(target);
        }
        else
        {
            //selectionArrow.GetComponent<Renderer>().enabled = false;
            arrowRenderer.enabled = false;
        }
    }
}
