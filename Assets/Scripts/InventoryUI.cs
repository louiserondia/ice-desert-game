using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    private bool _isVisible = true;
    void Start()
    {
        ToggleInventoryUI();
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnClick(btn));
        }
    }

    public void UpdateItemAmount(string name, int amount)
    {
        Transform amountTransform = GameObject.Find("LayoutGroupInventory/button_" + name + "/amount").transform;
        TMPro.TextMeshProUGUI text = amountTransform.GetComponent<TMPro.TextMeshProUGUI>();
        if (text != null)
            text.text = amount.ToString();
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
            ToggleInventoryUI();
    }

    void ToggleInventoryUI()
    {
        _isVisible = !_isVisible;
        CanvasGroup layoutGroup = GameObject.Find("LayoutGroupInventory").GetComponent<CanvasGroup>();
        layoutGroup.alpha = _isVisible ? 1f : 0f;
        layoutGroup.interactable = _isVisible;
        layoutGroup.blocksRaycasts = _isVisible;
    }

    void OnClick(Button button)
    {
        string name = button.name.Split('_')[1];

        if (name == "wood")
        {
            GameObject.Find("Player").GetComponent<PlayerInteract>().BurnWood();
        }
    }
}
