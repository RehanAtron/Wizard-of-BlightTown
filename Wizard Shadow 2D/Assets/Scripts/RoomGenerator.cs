using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RuleTile tile;
    [SerializeField] private Tile groundTile;
    [Space]
    private RoomManager roomManager;
    public int totalRooms, distanceBetween;
    private int width, height;
    public Vector2Int roomPosition;
    public HashSet<Vector2Int> rooms = new HashSet<Vector2Int>{
        Vector2Int.zero
    };
    [Space]
    [SerializeField] private GameObject door,enemy;
    public Transform doorParent;
    public List<Door> previousDoors = new List<Door>();
    public List<Door> nextDoors = new List<Door>();
    Dictionary<Vector2, int> Doors =  new Dictionary<Vector2, int>();
    [Space]
    public List<GameObject[]> enemies = new List<GameObject[]>();
    void Start()
    {
        roomManager = GetComponent<RoomManager>();
        width = 11;
        height = 11;
        RoomGeneration();
    }
    void RoomGeneration()
    {
        for (int i = 0; i < totalRooms; i++)
        {
            // adds room to be placed into the list
            rooms.Add(roomPosition);
            
            GameObject[] enemiesPerRoom = new GameObject[Random.Range(1,5)];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tilemap.SetTile(new Vector3Int(roomPosition.x - width/2 + x, roomPosition.y - height/2 + y, 0),tile);
                }
            }
            

            if (i == totalRooms-1)
            {
                break;
            }

            List<Vector2> vector2s = new List<Vector2>() {
                    Vector2.up,
                    Vector2.down,
                    Vector2.left,
                    Vector2.right,
                    new Vector2 (1,1),
                    new Vector2 (-1,1),
                    new Vector2 (1,-1),
                    new Vector2 (-1,-1)
                };
            System.Random random = new System.Random();
            if (i > 0)
            {
                    for (int j = 0; j < enemiesPerRoom.Length; j++)
                {
                    Vector2 offset = vector2s[random.Next(vector2s.Count)];
                    Vector2 enemySpawnPosition = roomPosition + (offset * Random.Range(0,(width/2)-2));

                    while (Vector2.Distance(enemySpawnPosition, previousDoors[previousDoors.Count-1].position) < 5)
                    {
                        offset = vector2s[random.Next(vector2s.Count)];
                        enemySpawnPosition = roomPosition + (offset * Random.Range(0,(width/2)-2));
                    }
                    
                    GameObject specific = Instantiate(enemy, enemySpawnPosition, Quaternion.identity);
                    enemiesPerRoom[j] = specific;
                }
                enemies.Add(enemiesPerRoom);
            }
            
            int room = Random.Range(-2,2);
            while (rooms.Contains(newRoomPosition(room)))
            {
                room = Random.Range(-2,2);
            }
            
            // places next door
            if (room < 0)
            {
                GameObject thisDoor = Instantiate(door, doorPosition(room,-width,-height), Quaternion.identity, doorParent);
                nextDoors.Add(new Door(doorPosition(room,-width,-height), thisDoor,room));
            }
            else
            {
                GameObject thisDoor = Instantiate(door, doorPosition(room,-width,-height), Quaternion.Euler(0,0,90), doorParent);
                nextDoors.Add(new Door(doorPosition(room,-width,-height), thisDoor,room));
            }
            Doors.Add(doorPosition(room,-width,-height),room);

            // next room details
            roomPosition = newRoomPosition(room);
            width = Random.Range(6,10) * 2 + 1;
            height = width;
            // places previous door
            if (room < 0)
            {
                GameObject thisDoor = Instantiate(door, doorPosition(room,width,height), Quaternion.identity, doorParent);
                previousDoors.Add(new Door(doorPosition(room,width,height),thisDoor,room));
            }
            else
            {
                GameObject thisDoor = Instantiate(door, doorPosition(room,width,height), Quaternion.Euler(0,0,90), doorParent);
                previousDoors.Add(new Door(doorPosition(room,width,height),thisDoor,room));
            }
            Doors.Add(doorPosition(room,width,height),room);
        }
        DeleteDoorOverlap(Doors);
        Debug.Log("Rooms Generated");
        roomManager.RoomMaker();
    }
    
    Vector2Int newRoomPosition (int r) =>
        r switch 
        {
            -2 => roomPosition + new Vector2Int(-distanceBetween,0),
            -1  => roomPosition + new Vector2Int(distanceBetween,0),
            1  => roomPosition + new Vector2Int(0,-distanceBetween),
            0  => roomPosition + new Vector2Int(0,distanceBetween),
            >1 => roomPosition,
            <-2 => roomPosition
        };

    Vector2 doorPosition (int d, int width, int height) =>
        d switch
        {
            -2 => roomPosition + new Vector2(width/2,0),
            -1  => roomPosition + new Vector2(-width/2,0),
            1  => roomPosition + new Vector2(0,height/2),
            0  => roomPosition + new Vector2(0,-height/2),
            >1 => Vector2.zero,
            <-2 => Vector2.zero 
        };

    void DeleteDoorOverlap(Dictionary<Vector2,int> doors)
    {
        foreach (KeyValuePair<Vector2,int> door in doors)
        {
            Vector3Int doorPos = new Vector3Int(Mathf.RoundToInt(door.Key.x), Mathf.RoundToInt(door.Key.y), 0);
            if (door.Value == -2 || door.Value == -1)
            {
                tilemap.SetTile(doorPos + Vector3Int.up, null);
                tilemap.SetTile(doorPos + Vector3Int.down, null);
            }
            else
            {
                tilemap.SetTile(doorPos + Vector3Int.left, null);
                tilemap.SetTile(doorPos + Vector3Int.right, null);
            }   
        }
    }
}

[System.Serializable]
public class Door
{
    public Vector3 position;
    public GameObject door;
    public int orientation;
    public Door() {

    }

    public Door (Vector3 position, GameObject Door, int direction) {
        this.position = position;
        door = Door;
        orientation = direction;
    }

    public Door(Vector3 position)
    {
        this.position = position;
    }
}