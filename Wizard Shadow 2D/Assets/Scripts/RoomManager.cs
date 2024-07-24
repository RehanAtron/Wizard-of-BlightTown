using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private RoomGenerator roomGenerator;
    private Dictionary<GameObject[],List<GameObject>> enemyRoomConnection = new Dictionary<GameObject[], List<GameObject>>();
    public int roomnumber;
    void Start()
    {
        roomGenerator = GetComponent<RoomGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        RoomLock();
    }
    public void RoomMaker()
    {
        for (int i = 0; i < roomGenerator.totalRooms - 2; i++)
        {
            List<GameObject> DoorsPerRoom = new List<GameObject>
            {
                roomGenerator.previousDoors[i].door,
                roomGenerator.nextDoors[i+1].door
            };
            enemyRoomConnection.Add(roomGenerator.enemies[i],DoorsPerRoom);
        }
    }
    void RoomLock()
    {
        foreach (var entry in enemyRoomConnection)
        {
            GameObject[] enemiesInRoom = entry.Key;
            List<GameObject> doorsInRoom = entry.Value;

            bool enemiesExist = false;

            // Check if any enemy in the room is still active
            foreach (var enemy in enemiesInRoom)
            {
                if (enemy != null)
                {
                    enemiesExist = true;
                    break;
                }
            }

            // Set the trigger state of doors
            foreach (var door in doorsInRoom)
            {
                if (door.TryGetComponent<Collider2D>(out var collider))
                {
                    collider.isTrigger = enemiesExist ? false : true;
                }
            }
        }
    }
}
