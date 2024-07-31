using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCheck : MonoBehaviour
{
    [SerializeField] GameObject[] hearts = new GameObject[5];
    void Start()
    {
        CheckHealth();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }
    void CheckHealth()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Inventory.Instance.health > i)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }
}
