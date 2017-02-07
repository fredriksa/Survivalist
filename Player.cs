using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public Text healthText = null;
    public BarPct healthBarScaler = null;

	void Update ()
    {
		if (healthText)
        {
            Health health = GetComponent<Health>();

            if (healthText)
                healthText.text = health.health.ToString() + " / " + health.maxHealth;

            if (healthBarScaler)
            {
                healthBarScaler.value = health.health;
                healthBarScaler.maxValue = health.maxHealth;
            }
        }
	}
}
