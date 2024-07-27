using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public GameObject[] items;
    void Update()
    {
        bool hasNull = items.Any(item => !item);
        if (hasNull)
        {
            foreach (var item in items)
            {
                if (item != null)
                Destroy(item);
            }
            Inventory.Instance.level += 1;
            SceneManager.LoadScene(Inventory.Instance.level);
        }
    }

}
