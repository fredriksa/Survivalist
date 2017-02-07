using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   [SerializeField]
    private float mMaxHealth = 100;
    [SerializeField]
    private float mHealth = 0;

    public float health { get { return mHealth; } }
    public float maxHealth { get { return mMaxHealth; } }

    public void heal(float healthAmount)
    {
        mHealth += healthAmount;
        Mathf.Clamp(mHealth, 0f, mHealth);
    }
}
