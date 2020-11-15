using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class DeviceManager : MonoBehaviour {
    public GameObject DevicePrefab;
    public GameObject DeviceConnectionPrefab;
    public GameObject PlayerPrefab;
    public int NumberOfDevices = 10;
    [Range(1, 4)]
    public int NumberOfPlayers = 1;
    public int LevelWidth = 20, LevelHeight = 20;
    public List<GameObject> Devices { get; }
    public List<Connection> Connections { get; }
    public List<Vector3> MapPositions { get; set; }
                /* Player, Position */
    public List<(GameObject, Vector3)> NewMapPositions { get; }

    private const float TRANSMISSION_DISTANCE = 10;

    DeviceManager() {
        Devices = new List<GameObject>();
        Connections = new List<Connection>();
        // set in MapManager: MapPositions = transform.parent.GetComponent<MapManager>().MapPositions;
        NewMapPositions = new List<(GameObject, Vector3)>();
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
            GameObject spawnDevice = Instantiate(DevicePrefab, spawnDevicePos, Quaternion.identity, transform);
            Devices.Add(spawnDevice);

            DeviceController spawnDeviceController = spawnDevice.GetComponent<DeviceController>();
            spawnDeviceController.Player = Instantiate(PlayerPrefab, spawnDevice.transform);
        }

        // Fill Devices with GameObjects on random positions
        for (int i = 0; i < NumberOfDevices - NumberOfPlayers; i++) {
            float x = Random.Range(0f, LevelWidth);
            float z = Random.Range(0f, LevelHeight);
            var devicePos = new Vector3(x, 0, z);
            GameObject device = Instantiate(DevicePrefab, devicePos, Quaternion.identity, transform);
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
                    foundDuplicate = connection.Device1 == device2 && connection.Device2 == device1;
                    if (foundDuplicate) {
                        break;
                    }
                }

                if (foundDuplicate) {
                    continue;
                }

                DeviceController device1Controller = device1.GetComponent<DeviceController>(),
                    device2Controller = device2.GetComponent<DeviceController>();
                GameObject deviceConnection = Instantiate(DeviceConnectionPrefab, transform);
                Renderer deviceConnectionRenderer = deviceConnection.GetComponent<Renderer>();
                deviceConnectionRenderer.enabled = false;
                float distance = Vector3.Distance(device1.transform.position, device2.transform.position);
                var newConnection = new Connection(device1, device1Controller, device2, device2Controller, deviceConnection, deviceConnectionRenderer, distance);
                Connections.Add(newConnection);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        foreach (Connection connection in Connections) {
            if (connection.Distance <= TRANSMISSION_DISTANCE) {
                // ELONGATE the DeviceConnection
                connection.DeviceConnectionRenderer.transform.localScale = new Vector3(
                    x: connection.DeviceConnectionRenderer.transform.localScale.x,
                    y: Vector3.Distance(connection.Device1.transform.position, connection.Device2.transform.position) / 2,
                    z: connection.DeviceConnectionRenderer.transform.localScale.z);
                // Place the DeviceConnection inbetween Device1 and Device2
                connection.DeviceConnectionRenderer.transform.position =
                    (connection.Device1.transform.position + connection.Device2.transform.position) / 2;
                // Rotate the DeviceConnection towards each Device
                float angleOfDevice1RelativeToDevice2 = 180 / Mathf.PI * Mathf.Atan2(connection.Device1.transform.position.x - connection.Device2.transform.position.x, connection.Device1.transform.position.z - connection.Device2.transform.position.z);
                connection.DeviceConnectionRenderer.transform.rotation = Quaternion.Euler(90f, angleOfDevice1RelativeToDevice2, 0f);
                connection.DeviceConnectionRenderer.enabled = true;
            }
        }
    }

    // Called each physics update
    void FixedUpdate() {
        UpdateConnections();
    }
}
