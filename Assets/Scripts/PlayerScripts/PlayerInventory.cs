using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory UI")]
    public Transform inventoryPanel; // Parent container for inventory slots

    private List<char> collectedLetters = new List<char>();

    void Start()
    {
        // Hide inventory panel on game start
        inventoryPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        // Toggle the inventory panel with E key
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
        }
    }

    public void AddLetter(char letter, Sprite sprite)
    {
        collectedLetters.Add(letter);

        // Create new inventory slot
        GameObject slot = new GameObject($"Letter_{letter}", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        slot.transform.SetParent(inventoryPanel, false);

        Image image = slot.GetComponent<Image>();
        image.sprite = sprite;

        Debug.Log($"Collected: {letter}");
    }

    public List<char> GetInventory()
    {
        return new List<char>(collectedLetters);
    }
}
