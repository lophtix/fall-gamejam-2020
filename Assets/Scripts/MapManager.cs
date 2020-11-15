using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public GameObject DeviceManagerPrefab;
    public GameObject RoadStraightPrefab;
    public GameObject RoadFourWayPrefab;
    public List<Vector3> MapPositions;

    private List<string[]> maps;

    public MapManager() {
        maps = new List<string[]>();
        foreach (string filePath in Directory.GetFiles("Assets/Maps")) {
            maps.Add(File.ReadAllLines(filePath));
        }

        MapPositions = new List<Vector3>();
    }

    // Start is called before the first frame update
    void Start() {
        GameObject devices = Instantiate(DeviceManagerPrefab, transform);
        DeviceManager deviceManager = devices.GetComponent<DeviceManager>();
        deviceManager.MapPositions = MapPositions;

        for (int z = 0; z < maps[0].Length; z++) {
            for (int x = 0; x < maps[0][z].Length; x++) {
                char tile = maps[0][z][x];
                float roadWidth = RoadStraightPrefab.transform.localScale.x / 50,
                    roadDepth = RoadStraightPrefab.transform.localScale.z / 50;
                Vector3 position = new Vector3(x * roadWidth, 0, z * roadDepth);
                if (tile == '|') {
                    Instantiate(RoadStraightPrefab, position, Quaternion.AngleAxis(270, Vector3.right), transform);
                }
                else if (tile == '-') {
                    Instantiate(RoadStraightPrefab, position, Quaternion.Euler(270, 90, 0), transform);
                }
                else if (tile == '+') {
                    Instantiate(RoadFourWayPrefab, position, Quaternion.AngleAxis(270, Vector3.right), transform);
                }
                else {
                    continue;
                }

                MapPositions.Add(position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
