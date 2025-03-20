using UnityEngine;

public class Bear : MonoBehaviour
{
    private int health;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 300;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.health <= 0)
        {
            Die();
        }
    }

    public int GetHealth()
    {
        return this.health;
    }

    public void TakeDamage(int damageTaken)
    {
        this.health -= damageTaken;
    }

    public void Heal(int healthGained)
    {
        this.health += healthGained;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
