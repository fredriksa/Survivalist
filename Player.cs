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
                healthText.text = health.getHealth().ToString() + " / " + health.getMaxHealth();

            if (healthBarScaler)
            {
                healthBarScaler.value = health.getHealth();
                healthBarScaler.maxValue = health.getMaxHealth();
            }
        }
	}
}
