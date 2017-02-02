using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public static GameObject announcementObj;
    private static readonly UIHandler instance = new UIHandler(); 

    private UIHandler() { } //Prevent instantiation
    private void Awake()
    {
        announcementObj = GameObject.FindGameObjectWithTag("AnnouncementObject");
        announcementObj.SetActive(false);
    }

    public static UIHandler Instance
    {
        get
        {
            return instance;
        }
    }

    public void announce(string message, Sprite icon = null)
    {
        if (announcementObj == null) return;


        if (icon != null)
            ObjectHelper.getChildGameObject(announcementObj, "Image").GetComponent<Image>().sprite = icon;

        ObjectHelper.getChildGameObject(announcementObj, "Text").GetComponent<Text>().text = message;

        announcementObj.SetActive(true);
        announcementObj.GetComponent<FadeInOut>().fade(1, 1.5f, 1);
    }

   
}
