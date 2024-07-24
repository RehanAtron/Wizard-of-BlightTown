using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private RoomGenerator roomGenerator;
    private Dictionary<int, List<GameObject>> roomEnemies = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> roomDoors = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        roomGenerator = GetComponent<RoomGenerator>();
        RoomMaker();
    }

    void Update()
    {
        RoomLock();
    }

    public void RoomMaker()
    {
        for (int i = 0; i < roomGenerator.totalRooms - 2; i++)
        {
            List<GameObject> doorsPerRoom = new List<GameObject>
            {
                roomGenerator.previousDoors[i].door,
                roomGenerator.nextDoors[i + 1].door
            };

            // Initialize roomEnemies and roomDoors
            if (!roomEnemies.ContainsKey(i))
            {
                roomEnemies.Add(i, new List<GameObject>(roomGenerator.enemies[i]));
            }

            if (!roomDoors.ContainsKey(i))
            {
                roomDoors.Add(i, doorsPerRoom);
            }
        }
    }

    void RoomLock()
    {
        foreach (var room in roomEnemies)
        {
            int roomId = room.Key;
            List<GameObject> enemiesInRoom = room.Value;
            List<GameObject> doorsInRoom = roomDoors[roomId];

            bool enemiesExist = enemiesInRoom.Exists(enemy => enemy != null);

            foreach (var door in doorsInRoom)
            {
                if (door.TryGetComponent<Collider2D>(out var collider))
                {
                    collider.isTrigger = !enemiesExist;
                    if (collider.isTrigger)
                    {
                        door.GetComponent<SpriteRenderer>().sprite = null;
                    }
                }
            }
        }
    }
}
