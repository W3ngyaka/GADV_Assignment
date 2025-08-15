using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public GameObject gameMenu;

    [Header("Inventory UI")]
    public GameObject inventoryPanel;
    public List<GameObject> inventorySlots;

    private List<char> collectedLetters = new List<char>();

    // --- Unity lifecycle: allowed to keep one-liners inline (per your rule) ---
    void Start()
    {
        // Hide inventory and game menu on load (simple one-liners kept inline)
        inventoryPanel.SetActive(false);
        gameMenu.SetActive(false);
    }

    // --- Unity lifecycle: extract non-trivial logic into helpers ---
    void Update()
    {
        // Handle input that toggles inventory/menu each frame
        HandleInventoryToggleInput();
        HandleResumeInput();
    }

    // Toggle the inventory panel on E key press
    private void HandleInventoryToggleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    // Call ResumeLevel() when Escape is pressed
    private void HandleResumeInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeLevel();
        }
    }

    // Toggle the game menu visibility
    public void ResumeLevel()
    {
        gameMenu.SetActive(!gameMenu.activeSelf);
    }

    // Add a letter with its sprite into the first empty inventory slot
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

    // Instantiate a UI Image for the letter and attach an InventoryItem component
    void CreateLetterInSlot(char letter, Sprite sprite, Transform slot)
    {
        // Create new UI GameObject directly under the slot
        GameObject letterObj = new GameObject($"Letter_{letter}");
        letterObj.transform.SetParent(slot, false);

        // Add and configure Image component
        Image image = letterObj.AddComponent<Image>();
        image.sprite = sprite;
        image.rectTransform.sizeDelta = Vector2.one * 50f;
        image.rectTransform.anchoredPosition = Vector2.zero;

        // Store metadata for drag/drop and inventory logic
        InventoryItem item = letterObj.AddComponent<InventoryItem>();
        item.image = image;
        item.letter = letter;

        Debug.Log($"Added {letter} to slot {inventorySlots.IndexOf(slot.gameObject)}"); // Added .gameObject
    }

    // Remove a single instance of a letter from the list and its UI representation
    public void RemoveLetter(char letter)
    {
        // Remove from collected letters list
        collectedLetters.Remove(letter);

        // Optional: Find and destroy the visual representation
        foreach (GameObject slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                InventoryItem item = slot.transform.GetChild(0).GetComponent<InventoryItem>();
                if (item != null && item.letter == letter)
                {
                    Destroy(item.gameObject);
                    break;
                }
            }
        }
    }

    // Return a copy of the current collected letters
    public List<char> GetInventory()
    {
        return new List<char>(collectedLetters);
    }
}
