using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ItemStack : MonoBehaviour {
    public int maxSize;
    public int itemEntry = -1;
    [SerializeField]
    public List<Item> items;

    private Sprite originalIcon;
    private int originalMaxSize;

    void Start()
    {
        originalIcon = GetComponent<Image>().sprite;
        originalMaxSize = maxSize;
    }

    void Update()
    {

        Image image = GetComponent<Image>();
        if (isUsed() && image != null)
        {
            if (image != items[0].icon)
            {
                image.sprite = items[0].icon;
            }

            //Update item stack text
            GameObject textObj = ObjectHelper.getChildGameObject(gameObject, "Text");
            Text text = textObj.GetComponent<Text>();
            if (text)
                text.text = items.Count.ToString();
        } 

        if (!isUsed() && image != null)
        {
            if (image)
            {
                image.sprite = originalIcon;
            }

            //Remove item stack text
            GameObject textObj = ObjectHelper.getChildGameObject(gameObject, "Text");
            Text text = textObj.GetComponent<Text>();
            if (text)
                text.text = "";
        }
    }

    public void resetComponent()
    {
        items = new List<Item>();
        maxSize = originalMaxSize;
        GetComponent<Image>().sprite = originalIcon;
        itemEntry = -1;
        ObjectHelper.getChildGameObject(gameObject, "Text").GetComponent<Text>().text = "";
    }

    public bool push(Item item, int amount = 1)
    {
        if (!hasSpace(amount))
            return false;

        for (int i = 0; i < amount; i++)
        {
            useFor(item);
            if (!canPush(item))
                return false;

            items.Add(item);
        }

        return true;
    }

    //Kanske inte funkar som tänkt, kanske inte shiftar index..
    public List<Item> removeFirst(int amount = 1)
    {
        if (amount < 1) return null;
        List<Item> removedItems = new List<Item>();

        for (int i = 0; i < amount; i++)
        {
            if (items.Count < 1) continue;

            removedItems.Add(items[0]);
            items.RemoveAt(0);
        }

        afterRemove();
        return removedItems;
    }

    public bool remove(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            afterRemove();
            return true;
        }

        afterRemove();
        return false;
    }

    public bool contains(int entry)
    {
        return itemEntry == entry;
    }

    public bool hasSpace(int amount = 1)
    {
        return items.Count + amount <= maxSize;
    }

    public bool isUsed()
    {
        return itemEntry != -1;
    }

    public int getFreeSpace()
    {
        return maxSize - items.Count;
    }

    public int getUsedSpace()
    {
        return items.Count;
    }

    public int getSpace()
    {
        return maxSize;
    }

    private void free()
    {
        itemEntry = -1;
    }

    private void useFor(Item item)
    {
        if (itemEntry == -1)
            itemEntry = item.getEntry();
    }

    private bool canPush(Item item)
    {
        if (!contains(item.getEntry()))
            return false;

        if (!hasSpace(1))
            return false;

        return true;
    }

    private void afterRemove()
    {
        if (items.Count == 0)
            free();
    }
}

