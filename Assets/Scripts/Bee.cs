using UnityEngine;

public class Bee : MonoBehaviour
{
    private int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 1;
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

    public void Sting(Bear target)
    {
        this.health -= 1;
        int damageGiven = Random.Range(5, 15);
        target.TakeDamage(damageGiven);
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
