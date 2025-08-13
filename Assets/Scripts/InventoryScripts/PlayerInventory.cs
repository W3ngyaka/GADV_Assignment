using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory UI")]
    public GameObject inventoryPanel; 
    public List<GameObject> inventorySlots; 

    private List<char> collectedLetters = new List<char>();

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    public void AddLetter(char letter, Sprite sprite)
    {
        collectedLetters.Add(letter);

        // Find first empty slot
        foreach (GameObject slot in inventorySlots) // Changed to GameObject
        {
            if (slot.transform.childCount == 0) // Access transform for child count
            {
                CreateLetterInSlot(letter, sprite, slot.transform); // Pass the transform
                return;
            }
        }
        Debug.LogWarning("All slots are full!");
    }

    void CreateLetterInSlot(char letter, Sprite sprite, Transform slot)
    {
        // Create new UI Image directly
        GameObject letterObj = new GameObject($"Letter_{letter}");
        letterObj.transform.SetParent(slot, false);

        // Add and configure Image component
        Image image = letterObj.AddComponent<Image>();
        image.sprite = sprite;
        image.rectTransform.sizeDelta = Vector2.one * 50f;
        image.rectTransform.anchoredPosition = Vector2.zero;

        InventoryItem item = letterObj.AddComponent<InventoryItem>();
        item.image = image;
        item.letter = letter;

        Debug.Log($"Added {letter} to slot {inventorySlots.IndexOf(slot.gameObject)}"); // Added .gameObject
    }

    public List<char> GetInventory()
    {
        return new List<char>(collectedLetters);
    }
}