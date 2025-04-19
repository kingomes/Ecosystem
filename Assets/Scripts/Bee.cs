using System.Collections;
using UnityEngine;

public class Bee : MonoBehaviour
{
    private int health;
    private int hunger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 1;
        hunger = 100;
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

    public int GetHunger()
    {
        return this.hunger;
    }

    public void Sting(Bear target)
    {
        this.health -= 1;
        int damageGiven = Random.Range(5, 15);
        target.TakeDamage(damageGiven);
    }

    public IEnumerator LoseHunger(float duration)
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            this.hunger--;

            if (this.hunger <= 0)
            {
                this.hunger = 0;
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

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
