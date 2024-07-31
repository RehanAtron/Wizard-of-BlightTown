using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public Torch[] torches;
    public float fireRate;
    public int bulletNumber,damage;
    public int level;
    public int health = 5;
    public bool dead;
        void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Update() 
    {
        if (torches[0] != null || torches[1] != null)
        {
            fireRate = (torches[0]?.fireRate ?? 0) + (torches[1]?.fireRate ?? 0);
            bulletNumber = (torches[0]?.bulletNumber ?? 0) + (torches[1]?.bulletNumber ?? 0);
            damage = (torches[0]?.damage ?? 0) + (torches[1]?.damage ?? 0);
        }
        else
        {
            fireRate = 0;
            bulletNumber = 0;
            damage = 0;
        }
    }
}
