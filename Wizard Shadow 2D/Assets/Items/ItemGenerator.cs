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
    bool Showcase;

    private string fireRate = "Increase fire rate by";
    private string bullets = "Increase bullet number by";
    void Start()
    {
        SelectTorchForCurrent();
        inventory = FindObjectOfType<Inventory>();
        descriptionPanel = FindObjectOfType<InventoryHandlingGUI>().descriptionPanel;
    }

    void Update()
    {
        if (Showcase)
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
    void OnTriggerEnter2D(Collider2D other) 
    {
        descriptionPanel.SetActive(true);
        PanelManager(torches[index]);
        if (other.CompareTag("Player"))
        {
            Showcase = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        descriptionPanel.SetActive(false);
        Showcase = false;
    }
    void SelectTorchForCurrent()
    {
        index = Random.Range(0,torches.Count);
        Torch currentTorch = torches[index];
        GetComponent<SpriteRenderer>().sprite = currentTorch.sprite;
    }
    void PanelManager(Torch torch)
    {
        Image spriteImage = descriptionPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        Text text = descriptionPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        spriteImage.sprite = torches[index].sprite;
        text.text = Description(torches[index]);
    }

    string Description(Torch torch)
    {
        string d = "";
        if (torch.damage > 0)
        {
            d += $"Increases damage by {torch.damage} \n";
        }
        if (torch.fireRate > 0)
        {
            d += $"{fireRate} {torch.fireRate}% \n";
        }
        else if (torch.fireRate < 0)
        {
            d +=$"Decreases fire rate by {-torch.fireRate} \n";
        }
        if (torch.bulletNumber > 0)
        {
            d += $"{bullets} {torch.bulletNumber} \n";
        }
        if (torch.fireDamage)
        {
            d += $"Deals Fire damage for {torch.burnDamage} \n";
        }
        if (torch.iceDamage)
        {
            d += $"Freezes enemies for {torch.freezeTimer} seconds\n";
        }
        return d;
    }
}
