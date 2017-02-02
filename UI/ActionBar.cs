using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour {

    public List<KeyCode> keys;
    public List<GameObject> buttons;
    public GameObject activeButton;

    private Dictionary<KeyCode, GameObject> buttonsDict = new Dictionary<KeyCode, GameObject>();

    private void Awake()
    {
        if (keys.Count != buttons.Count)
        {
            Debug.LogError("Error: Keys and Buttons not same length ActionBar.CS : " + gameObject.name);
            return;
        }

        for (int i = 0; i < keys.Count; i++)
            buttonsDict.Add(keys[i], buttons[i]);
    }

	void Update ()
    {
        foreach (KeyValuePair<KeyCode, GameObject> pair in buttonsDict)
            if (Input.GetKeyDown(pair.Key))
                OnKeyPress(pair.Value);

        disableInactive();

        if (Input.GetKeyDown(KeyCode.Q) && activeButton)
            DropHandler.Drop(activeButton.GetComponentInChildren<ItemStack>(), ObjectHelper.getParentGameObject(activeButton, "Player"));
    }

    private void OnKeyPress(GameObject button)
    {
        ItemStack stack = button.GetComponentInChildren<ItemStack>();
        GameObject player = ObjectHelper.getParentGameObject(gameObject, "Player");
        if (stack && player && stack.getFirstItem())
           ItemActivateHandler.OnActivate(stack.getFirstItem(), player);

        ActionbarButton actionButton = button.GetComponent<ActionbarButton>();
        actionButton.OnActivate();
        activeButton = button;

        DeactiveButtons(button);
    }

    private void DeactiveButtons(GameObject button)
    {
        foreach (GameObject btn in buttons)
            if (btn != button)
                btn.GetComponent<ActionbarButton>().OnReset();
    }

    private void disableInactive()
    {
        foreach (GameObject btn in buttons)
        {
            ActionbarButton aBtn = btn.GetComponent<ActionbarButton>();
            ItemStack stack = btn.GetComponentInChildren<ItemStack>();
            if (aBtn && stack && !stack.isUsed())
                aBtn.OnReset();
        }
    }
}
