using System.Collections;
using UnityEngine;

public class Bear : MonoBehaviour
{
    private int health;
    private int hunger;
    
    [SerializeField] BearHUD bearHUD;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 300;
        hunger = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.health <= 0)
        {
            Die();
        }
        StartCoroutine(bearHUD.UpdateHealth());
        StartCoroutine(bearHUD.UpdateHunger());
    }

    // Getter methods
    public int GetHealth()
    {
        return this.health;
    }

    public int GetHunger()
    {
        return this.hunger;
    }

    public void TakeDamage(int damageTaken)
    {
        if (this.gameObject != null) {
            this.health -= damageTaken;
        }
    }

    public void Heal(int healthGained)
    {
        if (this.gameObject != null) {
            this.health += healthGained;
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator LoseHunger(float duration)
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            this.hunger--;

            if (this.hunger <= 0)
            {
                TakeDamage(1);
            }
        }
    }

    public void GainHunger(int hungerGained)
    {
        if (this.gameObject != null)
        {
            this.hunger += hungerGained;
        }
    }
}
