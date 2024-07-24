using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    public Inventory inventory;

    void Start() 
    {
        inventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStatus enemy = other.GetComponent<EnemyStatus>();
            enemy.hit = true;
            enemy.health -= 1;
            
            bool fireDamage = false;
            bool iceDamage = false;
            float burnTimer = 0;
            float freezeTimer = 0;
            int burnDamage = 0;

            if (inventory.torches.Length > 0)
            {
                Torch firstTorch = inventory.torches[0];
                if (firstTorch != null)
                {
                    fireDamage = firstTorch.fireDamage;
                    iceDamage = firstTorch.iceDamage;
                    burnTimer += firstTorch.burnTimer;
                    freezeTimer += firstTorch.freezeTimer;
                    burnDamage += firstTorch.burnDamage;
                }
            }
            
            if (inventory.torches.Length > 1)
            {
                Torch secondTorch = inventory.torches[1];
                if (secondTorch != null)
                {
                    fireDamage = fireDamage || secondTorch.fireDamage;
                    iceDamage = iceDamage || secondTorch.iceDamage;
                    burnTimer += secondTorch.burnTimer;
                    freezeTimer += secondTorch.freezeTimer;
                    burnDamage += secondTorch.burnDamage;
                }
            }

            if (fireDamage)
            {
                enemy.Burn(burnTimer, burnDamage);
            }

            if (iceDamage)
            {
                enemy.Freeze(freezeTimer);
            }

            if (enemy.health <= 0)
            {
                enemy.Die();
            }
        }
        gameObject.SetActive(false);
    }
}
