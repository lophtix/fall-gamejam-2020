using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection {
    public GameObject Device1 { get; }
    public DeviceController Device1Controller { get; }
    public GameObject Device2 { get; }
    public DeviceController Device2Controller { get; }
    public GameObject DeviceConnection { get; }
    public Renderer DeviceConnectionRenderer { get; }
    public float Distance { get; set; }

    public Connection(GameObject device1, DeviceController device1Controller, GameObject device2, DeviceController device2Controller, GameObject deviceConnection, Renderer deviceConnectionRenderer, float distance) {
        Device1 = device1;
        Device1Controller = device1Controller;
        Device2 = device2;
        Device2Controller = device2Controller;
        DeviceConnection = deviceConnection;
        DeviceConnectionRenderer = deviceConnectionRenderer;
        Distance = distance;
    }
}
