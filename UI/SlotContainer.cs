using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotContainer : MonoBehaviour {
    public List<GameObject> slots = new List<GameObject>();

    public bool distributeItem(Item item, int amount = 1)
    {
        if (!hasSpaceFor(item, amount)) return false;

        amount -= addToExistingSlots(item, amount);
        amount -= addToEmptySlots(item, amount);

        return true;
    }

    public bool removeItem(Item item, int amount = 1)
    {
        if (getItemCount(item) < amount) return false;

        foreach (GameObject slot in slots)
        {
            ItemStack stack = slot.GetComponent<ItemStack>();
            if (!stack) continue;

            if (stack.isUsed() && stack.contains(item.getEntry()))
            {
                int itemsToRemove = calculateItemsToRemove(stack, amount);
                stack.removeFirst(itemsToRemove);
                amount -= itemsToRemove;

                if (amount == 0)
                    return true;
            }
        }

        return true;
    }

    public int getItemCount(Item item)
    {
        int found = 0;

        foreach (GameObject slot in slots)
        {
            ItemStack stack = slot.GetComponent<ItemStack>();
            if (stack && stack.contains(item.getEntry()))
                found += stack.getUsedSpace();
        }

        return found;
    }

    public int getUsedSpace()
    {
        int usedSpace = 0;

        foreach (GameObject slot in slots)
        {
            ItemStack stack = slot.GetComponent<ItemStack>();
            if (!stack) continue;

            usedSpace += stack.getUsedSpace();
        }

        return usedSpace;
    }

    public int getFreeSpace()
    {
        int freeSpace = 0;

        foreach (GameObject slot in slots)
        {
            ItemStack stack = slot.GetComponent<ItemStack>();
            if (!stack) continue;

            freeSpace += stack.getFreeSpace();
        }

        return freeSpace;
    }

    public int getTotalSpace()
    {
        int totalSpace = 0;

        foreach (GameObject slot in slots)
        {
            ItemStack stack = slot.GetComponent<ItemStack>();
            if (!stack) continue;

            totalSpace += stack.getSpace();
        }

        return totalSpace;
    }

    private bool hasSpaceFor(Item item, int amount = 1)
    {
        int foundSpace = 0;
        foreach (GameObject slot in slots)
        {
            ItemStack stack = slot.GetComponent<ItemStack>();
            bool isFree = !stack.isUsed();
            bool containsItemButHasSpace = (stack.contains(item.getEntry()) && stack.getFreeSpace() > 0);

            if (isFree || containsItemButHasSpace)
                foundSpace += stack.getFreeSpace();
        }

        return amount <= foundSpace;
    }

    private int addToExistingSlots(Item item, int amount = 1)
    {
        int added = 0;
        foreach (GameObject slot in slots)
        {
            ItemStack stack = slot.GetComponent<ItemStack>();

            if (stack.contains(item.getEntry()) && stack.hasSpace(1))
            {
                int nItems = calculateItemsToPush(stack, amount);
                stack.push(item, nItems);
                added += nItems;

                if (added == amount)
                    return added;
            }
        }

        return added;
    }

    private int addToEmptySlots(Item item, int amount = 1)
    {
        int added = 0;
        foreach (GameObject slot in slots)
        {
            ItemStack stack = slot.GetComponent<ItemStack>();

            if (!stack.isUsed())
            {
                int nItems = calculateItemsToPush(stack, amount);
                stack.push(item, nItems);
                added += nItems;

                if (added == amount)
                    return added;
            }
        }

        return added;
    }

    private int calculateItemsToPush(ItemStack stack, int requestedAmount = 1)
    {
        int itemsToPush = requestedAmount;

        if (requestedAmount >= stack.getFreeSpace())
            itemsToPush = stack.getFreeSpace();

        return itemsToPush;
    }

    private int calculateItemsToRemove(ItemStack stack, int requestedAmount = 1)
    {
        int itemsToRemove = requestedAmount;

        if (requestedAmount >= stack.getUsedSpace())
            itemsToRemove = stack.getUsedSpace();

        return itemsToRemove;
    }
}
