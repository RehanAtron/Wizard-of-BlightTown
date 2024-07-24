using UnityEngine;
using UnityEngine.UI;

public class InventoryHandlingGUI : MonoBehaviour
{
    public Image image1;
    public Image image2;

    public Inventory inventory;
    public GameObject descriptionPanel;
    void Start() 
    {
        inventory = FindObjectOfType<Inventory>();
    }
    void Update() 
    {
        if (inventory.torches[0] != null)
        {
            image1.sprite = inventory.torches[0].sprite;
        }
        if (inventory.torches[1] != null)
        {
            image2.sprite = inventory.torches[1].sprite;
        }
    }
}
