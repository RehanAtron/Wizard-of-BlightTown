using UnityEngine;
using UnityEngine.UI;

public class InventoryHandlingGUI : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public GameObject descriptionPanel;
    public GameObject deathPanel;
    void Start() 
    {

    }
    void Update() 
    {
        Death();
        if (Inventory.Instance.torches[0] != null)
        {
            image1.sprite = Inventory.Instance.torches[0].sprite;
            image1.color = Color.white;
        }
        else
        {
            image1.color = new Color(0,0,0,0);
        }
        if (Inventory.Instance.torches[1] != null)
        {
            image2.sprite = Inventory.Instance.torches[1].sprite;
            image2.color = Color.white;
        }
        else
        {
            image2.color = new Color(0,0,0,0);
        }
    }

    public void Death()
    {
        if (Inventory.Instance.health <= 0)
        {
            deathPanel.SetActive(true);
            if (deathPanel.transform.localScale.x < 1)
            {
                deathPanel.transform.localScale = Vector2.Lerp(deathPanel.transform.localScale, new Vector3(1,1,1), Time.unscaledDeltaTime * 2);
            }
            Inventory.Instance.dead = true;
            Time.timeScale = 0;
        }
    }
}
