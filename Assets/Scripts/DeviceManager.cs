﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour {
    public GameObject DevicePrefab;
    public GameObject PlayerPrefab;
    public int NumberOfDevices = 10;
    [Range(1, 4)]
    public int NumberOfPlayers = 1;
    public int LevelWidth = 10, LevelHeight = 10;
    public List<GameObject> Devices { get; }
    public List<Connection> Connections { get; }

    private const float TRANSMISSION_DISTANCE = 1;

    DeviceManager() {
        Devices = new List<GameObject>();
        Connections = new List<Connection>();
    }

    void UpdateConnections() {
        foreach (Connection connection in Connections) {
            connection.Device1Controller.NearbyConnections.Clear();
            connection.Device2Controller.NearbyConnections.Clear();
        }

        foreach (Connection connection in Connections) {
            float newDistance = Vector3.Distance(connection.Device1.transform.position, connection.Device2.transform.position);
            if (newDistance <= TRANSMISSION_DISTANCE) {
                connection.Device1Controller.NearbyConnections.Add(connection);
                connection.Device2Controller.NearbyConnections.Add(connection);
            }

            connection.Distance = newDistance;
        }
    }

    // Start is called before the first frame update
    void Start() {
        // Fill the first NumberOfPlayers objects in Devices with devices for spawning players
        for (int i = 0; i < NumberOfPlayers; i++) {
            int x = i % 2 * LevelWidth,
                z = Mathf.FloorToInt(i / 2f) * LevelHeight;
            var spawnDevicePos = new Vector3(x, 0, z);
            GameObject spawnDevice = Instantiate(DevicePrefab, spawnDevicePos, Quaternion.identity);
            Devices.Add(spawnDevice);

            DeviceController spawnDeviceController = spawnDevice.GetComponent<DeviceController>();
            spawnDeviceController.Player = Instantiate(PlayerPrefab, spawnDevice.transform);
        }

        // Fill Devices with GameObjects on random positions
        for (int i = 0; i < NumberOfDevices - NumberOfPlayers; i++) {
            float x = Random.Range(0f, LevelWidth);
            float z = Random.Range(0f, LevelHeight);
            var devicePos = new Vector3(x, 0, z);
            GameObject device = Instantiate(DevicePrefab, devicePos, Quaternion.identity);
            Devices.Add(device);
        }

        // Fill Connections with objects, however ignore duplicates such as 1 - 2 - dist & 2 - 1 - dist
        for (int i = 0; i < Devices.Count; i++) {
            for (int j = 0; j < Devices.Count; j++) {
                if (i == j) {
                    continue;
                }

                GameObject device1 = Devices[i], device2 = Devices[j];
                bool foundDuplicate = false;
                foreach (Connection connection in Connections) {
                    foundDuplicate = connection.Device1 == device2 || connection.Device2 == device1;
                    if (foundDuplicate) {
                        break;
                    }
                }

                if (foundDuplicate) {
                    continue;
                }

                DeviceController device1Controller = device1.GetComponent<DeviceController>(),
                    device2Controller = device2.GetComponent<DeviceController>();
                float distance = Vector3.Distance(device1.transform.position, device2.transform.position);
                Connection newConnection = new Connection(device1, device2, device1Controller, device2Controller, distance);
                Connections.Add(newConnection);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called each physics update
    void FixedUpdate() {
        UpdateConnections();
    }
}
