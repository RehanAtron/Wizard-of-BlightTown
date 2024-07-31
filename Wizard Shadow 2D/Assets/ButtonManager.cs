using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void Play()
    {
        Inventory.Instance.level = 2;
        Inventory.Instance.health = 5;
        Inventory.Instance.dead = false;
        Inventory.Instance.torches = new Torch[2];
        SceneManager.LoadScene("Level1");
    }
    public void Retry()
    {
        Inventory.Instance.level = 1;
        Inventory.Instance.health = 5;
        Inventory.Instance.dead = false;
        Inventory.Instance.torches = new Torch[2];
        SceneManager.LoadScene("Shop0");
    }
    public void MainMenu()
    {
        Inventory.Instance.level = 0;
        Inventory.Instance.health = 5;
        Inventory.Instance.dead = false;
        Inventory.Instance.torches = new Torch[2];
        SceneManager.LoadScene("MainMenu");
    }
}
