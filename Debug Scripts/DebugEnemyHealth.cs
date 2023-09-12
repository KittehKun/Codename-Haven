
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DebugEnemyHealth : UdonSharpBehaviour
{
    public int MaxHealth{get; set;}
    public int CurrentHealth {get; set;}
    void Start()
    {
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
        Debug.Log($"Enemy health is {CurrentHealth}");
        if(CurrentHealth <= 0)
        {
            Debug.Log("Enemy has died.");
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"Destroying GameObject {this.transform.gameObject}");
        Destroy(this.transform.gameObject);
    }
}
