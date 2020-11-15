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

    Connection GetTargetConnection(Vector2 joystickVector)
    {
        float dx;
        float dz;
        GameObject device = null;
        GameObject closestDevice = null;
        Connection closestConnection = null;
        Vector2 deviceVector;

        float angleDiff;
        float smallestDiff = 360f;
        //Debug.Log(nearbyConnections.ToString());
        //Debug.Log("New loop");
        foreach (Connection connection in nearbyConnections)
        {   
            //Debug.Log("Device1: " + connection.Device1.transform.position.ToString() + " Device2: " + connection.Device2.transform.position.ToString());
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
                closestConnection = connection;
                smallestDiff = angleDiff;
            }
        }
        return closestConnection;
    }


    void HighlightTarget(GameObject target)
    {
        selectionArrow.transform.position = target.transform.position + new Vector3(0, 2, 0);
        //selectionArrow.GetComponent<Renderer>().enabled = true;
        arrowRenderer.enabled = true;
    }

    void MoveToDevice(DeviceController targetDeviceController, DeviceController myDeviceController)
    {
        Debug.Log("Jumpin'");
        if(nearbyConnections.Count > 0)
        {
            // Set new device's player 
            targetDeviceController.Player = myDeviceController.Player;
            // Unset current device's player
            myDeviceController.Player = null;
        }
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
        Connection targetConnection;
        GameObject targetDevice;
        DeviceController targetDeviceController;

        GameObject myDevice;
        DeviceController myDeviceController;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        //Debug.Log(horizontalInput.ToString() + ", " + verticalInput.ToString());

        if((horizontalInput != 0) || (verticalInput != 0))
        {
            Vector2 joystickVector = new Vector2(horizontalInput, verticalInput);
            targetConnection = GetTargetConnection(joystickVector);
            //Debug.Log(targetConnection.Device1.name);
            //Debug.Log(transform.parent.name);
            if(targetConnection.Device1 == transform.parent)
            {
                Debug.Log("true");
                targetDevice = targetConnection.Device2;
                targetDeviceController = targetConnection.Device2Controller;

                myDevice = targetConnection.Device1;
                myDeviceController = targetConnection.Device1Controller;
            }
            else
            {
                targetDevice = targetConnection.Device1;
                targetDeviceController = targetConnection.Device1Controller;

                myDevice = targetConnection.Device2;
                myDeviceController = targetConnection.Device2Controller;
            }

            //DrawLineToTarget();
            HighlightTarget(myDevice);
            if(Input.GetButtonDown("Jump"))
            {
                //MoveToDevice(targetDeviceController, myDeviceController);
                MoveToDevice(myDeviceController, targetDeviceController);
            }
        }
        else
        {
            //selectionArrow.GetComponent<Renderer>().enabled = false;
            arrowRenderer.enabled = false;
        }
    }
}
