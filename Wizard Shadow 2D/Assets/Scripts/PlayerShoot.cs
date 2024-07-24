using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectile,player;
    public Transform projectileTransform;
    public float OgfireRate, bulletSpeed, rotZ;
    [SerializeField] private float fireRate;
    private Vector3 mousePos;
    private Inventory inventory;
    void Start()
    {
        fireRate = OgfireRate;
        inventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        Rotation();
    }
    void FixedUpdate() 
    {
        Shoot();
    }

    void Rotation()
    {
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width 
         && Input.mousePosition.y  >= 0 && Input.mousePosition.y <= Screen.height)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
        }

        Vector3 rotation = mousePos - transform.position;
        rotZ = Mathf.Atan2(rotation.y,rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }
    void Shoot()
    {
        fireRate -= Time.deltaTime;
        if (Input.GetMouseButton(0) && fireRate <= 0)
        {
            int totalBullets = 1 + inventory.bulletNumber;
            GameObject[] bullets = new GameObject[totalBullets];
            float offsetIncrement = 0.1f;
            List<int> offsets = GenerateOffsets(totalBullets);

            for (int i = 0; i < totalBullets; i++)
            {
                GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
                bullet.SetActive(true);
                if (bullet != null) 
                {
                    float offset = offsets[i] * offsetIncrement;
                    Vector3 offsetVector = new Vector3(0, offset, 0);
                    

                    Quaternion rotation = Quaternion.Euler(0, 0, rotZ);
                    Vector3 spawnPosition = projectileTransform.position + rotation * offsetVector;

                    bullet.transform.position = spawnPosition;
                    bullet.transform.rotation = rotation;
                }
                bullets[i] = bullet;
            }
            foreach (var bullet in bullets)
            {
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector3 direction = mousePos - transform.position;
                    rb.velocity = new Vector2(direction.x,direction.y).normalized * bulletSpeed;

                    Vector3 rotation = transform.position - mousePos;
                    float rotZ = Mathf.Atan2(rotation.y,rotation.x) * Mathf.Rad2Deg;
                    bullet.transform.rotation = Quaternion.Euler(0,0,rotZ);
                }
            }
            FireRateCalculation();
        }
    }
    void FireRateCalculation()
    {
        fireRate = OgfireRate * (1 - inventory.fireRate/100);
    }

    List<int> GenerateOffsets(int totalBullets)
    {
        List<int> offsets = new List<int>();

        int half = totalBullets / 2;
        for (int i = -half; i <= half; i++)
        {
            if (totalBullets % 2 == 0 && i == 0)
                continue;
            offsets.Add(i);
        }

        return offsets;
    }
}

