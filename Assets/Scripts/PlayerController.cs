using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    //public GameObject child;

    //public GameObject deviceManager;
    //public GameObject target;
    public GameObject selectionArrow;
    Renderer arrowRenderer;
    public List<Connection> nearbyConnections;
    List<GameObject> nearbyDevices;

    // vvv incorrect as shite vvv
    //List<GameObject> nearbyDevices = deviceManager.nearbyDevices;
    //public List<Connection> nearbyDevices = new List<Connection>();

    GameObject FindTarget(Vector2 joystickVector)
    {
        float dx;
        float dz;
        GameObject device = null;
        GameObject closestDevice = null;
        Vector2 deviceVector;

        float angleDiff;
        float smallestDiff = 360f;

        foreach (Connection connection in nearbyConnections)
        {   
            device = connection.Device2;        // Might need fix
            /*
            if(connection.Device1 == transform.parent)
            {
                device = connection.Device2;
            }
            else
            {
                device = connection.Device1;
            }
            */

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
        selectionArrow.transform.position = target.transform.position + new Vector3(0, 2, 0);
        //selectionArrow.GetComponent<Renderer>().enabled = true;
        arrowRenderer.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        arrowRenderer = selectionArrow.GetComponent<Renderer>();
        Debug.Log("runnin'");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        //Debug.Log(horizontalInput.ToString() + ", " + verticalInput.ToString());

        if((horizontalInput != 0) || (verticalInput != 0))
        {
            Vector2 joystickVector = new Vector2(horizontalInput, verticalInput);
            GameObject target = FindTarget(joystickVector);
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
