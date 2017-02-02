using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public static GameObject announcementObj;
    public static GameObject eventAnnouncementObj;

    private static readonly UIHandler instance = new UIHandler(); 

    private UIHandler() { } //Prevent instantiation
    private void Awake()
    {
        announcementObj = GameObject.FindGameObjectWithTag("AnnouncementObject");
        announcementObj.SetActive(false);

        eventAnnouncementObj = GameObject.FindGameObjectWithTag("EventAnnouncementObject");
        eventAnnouncementObj.SetActive(false);
    }

    public static UIHandler Instance
    {
        get
        {
            return instance;
        }
    }

    public void announceEvent(string message, Sprite icon = null)
    {
        announceFor(eventAnnouncementObj, message, icon);
    }

    public void announce(string message, Sprite icon = null)
    {
        announceFor(announcementObj, message, icon);
    }

    private void announceFor(GameObject obj, string message, Sprite icon = null)
    {
        if (obj == null) return;

        if (icon != null)
            ObjectHelper.getChildGameObject(obj, "Image").GetComponent<Image>().sprite = icon;

        ObjectHelper.getChildGameObject(obj, "Text").GetComponent<Text>().text = message;

        obj.SetActive(true);
        obj.GetComponent<FadeInOut>().fade(1, 1.5f, 1);
    }
}
