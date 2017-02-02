using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float health = 0;

    public float getHealth() { return health; }
    public float getMaxHealth() { return maxHealth; }

    public void modHealth(float mod) { health -= mod; }

    public void heal(float healthAmount)
    {
        if (health + healthAmount > maxHealth)
            health = maxHealth;
        else
            health += healthAmount;
    }
}
