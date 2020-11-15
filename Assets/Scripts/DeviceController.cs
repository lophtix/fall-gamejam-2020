using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceController : MonoBehaviour {
    private GameObject player;

    public List<Connection> NearbyConnections { get; }

    /// <summary>
    /// Gets or sets the attached player <see cref="GameObject"/> of this <see cref="DeviceController"/> and updates its NearbyDevices property(!).
    /// </summary>
    public GameObject Player {
        get {
            return player;
        }
        set {
            if (value == null) {
                player = null;
                PlayerController = null;
            }
            else if (player == null) {
                player = value;
                PlayerController = player.GetComponent<PlayerController>(); // optimize!
                if (PlayerController != null) {
                    PlayerController.nearbyConnections = NearbyConnections;
                }
                else {
                    // If the GameObject passed was not a "player GameObject" with a PlayerController, reset player field to null.
                    player = null;
                }
            }
        }
    }

    public PlayerController PlayerController { get; private set; }

    public DeviceController() {
        NearbyConnections = new List<Connection>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(NearbyConnections.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
