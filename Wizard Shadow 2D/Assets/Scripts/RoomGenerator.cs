using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap, accesoryTilemap;
    [SerializeField] private RuleTile tile;
    [SerializeField] private Tile[] accesories;
    [Space]
    private RoomManager roomManager;
    public int totalRooms, distanceBetween;
    private int width, height;
    public Vector2Int roomPosition;
    public HashSet<Vector2Int> rooms = new HashSet<Vector2Int>{
        Vector2Int.zero
    };
    [Space]
    [SerializeField] private GameObject door,spider,still, bigSpider, huge, human;
    [SerializeField] private GameObject levelEnd;
    [SerializeField] private GameObject Boss;
    public Transform doorParent;
    public List<Door> previousDoors = new List<Door>();
    public List<Door> nextDoors = new List<Door>();
    Dictionary<Vector2, int> Doors =  new Dictionary<Vector2, int>();
    [Space]
    public List<GameObject[]> enemies = new List<GameObject[]>();
    GameObject bossClone,player;
    void Start()
    {
        roomManager = GetComponent<RoomManager>();
        player = GameObject.FindWithTag("Player");
        width = 9;
        height = 9;
        RoomGeneration();
    }
    void Update() 
    {
        if (bossClone != null)
        {
        if (Vector2.Distance(bossClone.transform.position,player.transform.position) < 5)
        {
            player.GetComponent<Light2D>().pointLightInnerRadius = 10;
            player.GetComponent<Light2D>().pointLightOuterRadius = 10;
            bossClone.SetActive(true);
        }}    
    }
    void RoomGeneration()
    {
        for (int i = 0; i < totalRooms; i++)
        {
            // adds room to be placed into the list
            rooms.Add(roomPosition);
            
            // room generation
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tilemap.SetTile(new Vector3Int(roomPosition.x - width/2 + x, roomPosition.y - height/2 + y, 0),tile);
                    int randomz = Random.Range(0,33);
                    if (x > 2 && y >= 2 && x < width-3 && y < height-3 && randomz == 10 && i > 0 && i < totalRooms-1)
                    {
                        accesoryTilemap.SetTile(new Vector3Int(roomPosition.x - width/2 + x, roomPosition.y - height/2 + y, 0),accesories[Random.Range(0,3)]); 
                    }
                }
            }
            

            if (i == totalRooms-1)
            {
                if (Inventory.Instance.level == 6)
                {
                    GameObject[] enemiesPerRoom = new GameObject[1];
                    bossClone = Instantiate(Boss, roomPosition + Vector2.zero, Quaternion.identity);
                    enemiesPerRoom[0] = bossClone;
                    enemies.Add(enemiesPerRoom);
                }
                else{
                Instantiate(levelEnd,new Vector2(0,height/4) + roomPosition,Quaternion.identity);}
                break;
            }

            // Monster Spawn
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
                int stillsPerRoom = 0;
                int spidersPerRoom = 0;
                int bigSpiderperRoom = 0;
                int hugePerRoom = 0;
                int humanPerRoom = 0;
                GameObject[] enemiesPerRoom;
                int randomS = Random.Range(0,2);
                if (width > 13 || height > 13)
                {
                    spidersPerRoom = Random.Range(1, 4);
                    stillsPerRoom = Random.Range(1, 4);
                    if (randomS == 0 && Inventory.Instance.level > 2)
                    {
                        hugePerRoom = 1;
                    }
                    else if (randomS == 1)
                    {
                        bigSpiderperRoom = 1;
                    }
                    if (Inventory.Instance.level > 4)
                    {
                        humanPerRoom = Random.Range(0,3);
                    }
                }
                else
                {
                    spidersPerRoom = Random.Range(1, 3);
                    if (randomS == 1)
                    {
                        bigSpiderperRoom = Random.Range(0,2);
                        spidersPerRoom = 1;
                    }
                    stillsPerRoom = 0;
                    hugePerRoom = 0;
                }
                enemiesPerRoom = new GameObject[spidersPerRoom + stillsPerRoom + bigSpiderperRoom + hugePerRoom];
                for (int j = 0; j < spidersPerRoom; j++)
                {
                    Vector2 offset;
                    Vector2 enemySpawnPosition;

                    do {
                        offset = vector2s[random.Next(vector2s.Count)];
                        enemySpawnPosition = roomPosition + (offset * Random.Range(0,(width/2)-2));
                    } while (Vector2.Distance(enemySpawnPosition, previousDoors[previousDoors.Count-1].position) < 5 && Mathf.Abs(enemySpawnPosition.x) < width/2 && Mathf.Abs(enemySpawnPosition.y) < height/2 );
                    
                    GameObject specific = Instantiate(spider, enemySpawnPosition, Quaternion.identity);
                    enemiesPerRoom[j] = specific;
                }
                if (bigSpiderperRoom > 0)
                {
                    GameObject specific = Instantiate(bigSpider,roomPosition + Vector2.zero, Quaternion.identity);
                    enemiesPerRoom[spidersPerRoom] = specific;
                }
                if (hugePerRoom > 0)
                {
                    GameObject specific = Instantiate(huge,roomPosition + Vector2.zero, Quaternion.identity);
                    enemiesPerRoom[spidersPerRoom] = specific;
                }
                if (width > 13 || height > 13)
                {
                    HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
                    Vector2 baseOffset = new Vector2((width - 2) / 2, 3);
                    
                    for (int k = 0; k < stillsPerRoom; k++)
                    {
                        Vector3 enemySpawnPosition; Vector2 offset;
                        do {
                            int r = Random.Range(4,8);
                            offset = baseOffset * vector2s[r];
                            enemySpawnPosition = roomPosition + offset;
                        } while (occupiedPositions.Contains(enemySpawnPosition));

                        Quaternion rotation = offset.x > 0 ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, 270);
                        GameObject specific = Instantiate(still, enemySpawnPosition, rotation);
                        enemiesPerRoom[spidersPerRoom + bigSpiderperRoom + k] = specific;

                        occupiedPositions.Add(enemySpawnPosition);
                    }
                }
                if (humanPerRoom > 0)
                {
                    HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
                    Vector3 enemySpawnPosition; Vector2 offset;
                    Vector2 baseOffset = new Vector2(Mathf.Sqrt((width * width)+(width * width))/4,Mathf.Sqrt((width * width)+(width * width))/4);
                    for (int k = 0; k < humanPerRoom; k++)
                    {
                        do {
                            int r = Random.Range(4,8);
                            offset = baseOffset * vector2s[r];
                            enemySpawnPosition = roomPosition + offset;
                        } while (occupiedPositions.Contains(enemySpawnPosition));

                        Instantiate(human, enemySpawnPosition, Quaternion.identity);

                        occupiedPositions.Add(enemySpawnPosition);
                    }
                }
                enemies.Add(enemiesPerRoom);
            }
            
            int room = Random.Range(-2,2);
            while (rooms.Contains(newRoomPosition(room)))
            {
                room = Random.Range(-2,2);
            }
            
            // places next door
            GameObject thisDoor = Instantiate(door, doorPosition(room,-width,-height), doorRotation(room), doorParent);
            nextDoors.Add(new Door(doorPosition(room,-width,-height), thisDoor,room));
            Doors.Add(doorPosition(room,-width,-height),room);

            // next room details
            roomPosition = newRoomPosition(room);
            if (Inventory.Instance.level == 6 && i == totalRooms-2)
            {
                width = 21;
                height = 21;
            }
            else
            {
                width = Random.Range(5,10) * 2 + 1;
                height = Random.Range(5,10) * 2 + 1;
            }
            // places previous door
            GameObject thatDoor = Instantiate(door, doorPosition(room,width,height), doorCOPYRotation(room), doorParent);
            previousDoors.Add(new Door(doorPosition(room,width,height),thatDoor,room));
            Doors.Add(doorPosition(room,width,height),room);
        }
        DeleteDoorOverlap(Doors);
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

    Quaternion doorRotation (int r) =>
        r switch
        {
            -2 => Quaternion.Euler(0,0,0),
            -1  => Quaternion.Euler(0,0,180),
            0  => Quaternion.Euler(0,0,270),
            1  => Quaternion.Euler(0,0,90),
            _ => Quaternion.identity
        };

    Quaternion doorCOPYRotation (int r) =>
        r switch
        {
            -1 => Quaternion.Euler(0,0,0),
            -2  => Quaternion.Euler(0,0,180),
            1  => Quaternion.Euler(0,0,270),
            0  => Quaternion.Euler(0,0,90),
            _ => Quaternion.identity
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