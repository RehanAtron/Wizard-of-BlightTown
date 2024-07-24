using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private RoomGenerator roomGenerator;
    public int index;
    bool movesToNext;
    public bool DoorsContains (Vector3 position, List<Door> doors) {
        foreach (Door door in doors) {
            if (door.position == position) {
                return true;
            }
        }

        return false;
    }
    void Start()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();
        FindIndex(transform.position);
    }
    void Update() 
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {   
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement.teleportCooldown <= 0)
            {
                playerMovement.teleportCooldown = 1;
                StartCoroutine(playerTeleport(other));
            }
        }
    }

    
    void FindIndex(Vector3 position)
{
    index = roomGenerator.previousDoors.FindIndex(door => Vector3.Distance(door.position, position) < 0.01f);
    if (index != -1)
    {
        movesToNext = false;
    }
    else
    {
        index = roomGenerator.nextDoors.FindIndex(door => Vector3.Distance(door.position, position) < 0.01f);
        if (index != -1)
        {
            movesToNext = true;
        }
    }
}

    IEnumerator playerTeleport(Collider2D other)
    {
        yield return null;
        if (other.CompareTag("Player") && movesToNext)
        {
            other.transform.position = roomGenerator.previousDoors[index].position + offset(roomGenerator.previousDoors[index].orientation);
        }
        else
        {
            other.transform.position = roomGenerator.nextDoors[index].position - offset(roomGenerator.nextDoors[index].orientation);
        }
        yield return null;
    }

    Vector3 offset (int r) =>
        r switch 
        {
            -2 =>  Vector3.left,
            -1  =>  Vector3.right,
            1  =>  Vector3.down,
            0  =>  Vector3.up,
            >1 => Vector3.zero,
            <-2 => Vector3.zero 
        }; 
}
