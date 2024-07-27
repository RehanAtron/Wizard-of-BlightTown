using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    bool isInTrigger;
    void Update()
    {
        if (isInTrigger && Input.GetKey(KeyCode.E))
        {
            Inventory.Instance.level += 1;
            SceneManager.LoadScene(Inventory.Instance.level);
        }
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        isInTrigger = true;
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        isInTrigger = false;
    }
}
