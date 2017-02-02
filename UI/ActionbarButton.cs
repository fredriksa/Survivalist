using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionbarButton : MonoBehaviour {

    public Sprite defaultImage;
    public Sprite activeImage;

    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        defaultImage = img.sprite;
    }

    public void OnActivate()
    {
        img.sprite = activeImage;
    }

    public void OnReset()
    {
        img.sprite = defaultImage;
    }
}
