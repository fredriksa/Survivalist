using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HoverButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Sprite hoverSprite = null;

    private Sprite originalSprite;
    private Image imageComponent;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        originalSprite = imageComponent.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hoverSprite) return;

        imageComponent.sprite = hoverSprite;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!hoverSprite) return;

        imageComponent.sprite = originalSprite;
    }
}
