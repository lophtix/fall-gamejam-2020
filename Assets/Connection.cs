using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection {
    public GameObject Device1 { get; }
    public GameObject Device2 { get; }
    public DeviceController Device1Controller { get; }
    public DeviceController Device2Controller { get; }
    public float Distance { get; set; }
     
    public Connection(GameObject device1, GameObject device2, DeviceController device1Controller, DeviceController device2Controller, float distance) {
        Device1 = device1;
        Device2 = device2;
        Device1Controller = device1Controller;
        Device2Controller = device2Controller;
        Distance = distance;
    }
}
