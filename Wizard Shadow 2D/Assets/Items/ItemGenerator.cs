using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private List<Torch> torches;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject descriptionPanel;
    private int index;

    private const string fireRate = "Increase fire rate by";
    private const string bullets = "Increase bullet number by";
    void Start()
    {
        SelectTorchForCurrent();
        inventory = FindObjectOfType<Inventory>();
        descriptionPanel = FindObjectOfType<InventoryHandlingGUI>().descriptionPanel;
    }

    void OnTriggerStay2D(Collider2D other) 
    {
        descriptionPanel.SetActive(true);
        PanelManager();
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                inventory.torches[0] = torches[index];
                Destroy(gameObject);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.torches[1] = torches[index];
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        descriptionPanel.SetActive(false);
    }
    void SelectTorchForCurrent()
    {
        index = Random.Range(0,torches.Count);
        Torch currentTorch = torches[index];
        GetComponent<SpriteRenderer>().sprite = currentTorch.sprite;
    }
    void PanelManager()
    {
        Image spriteImage = descriptionPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        Text text = descriptionPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        spriteImage.sprite = torches[index].sprite;
        text.text = fireRate + bullets;
    }
}
