using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingArea : MonoBehaviour, IDropHandler
{
    // List of valid word ? item recipes (configure in Inspector)
    public List<WordRecipe> recipes;

    // Current letters present as children under this CraftingArea
    public List<char> currentLetters = new List<char>();

    // Where to spawn the crafted item (child of this transform recommended)
    public Transform spawnPoint;

    // Holds the last crafted instance so we can replace/clear it correctly
    private GameObject _lastCraftedItem;

    // --- Unity lifecycle (tick-like): keep logic inline as requested ---
    void Update()
    {
        // Keep currentLetters in sync with child InventoryItems each frame
        SyncLettersWithChildren();
    }

    // Remove a specific letter from the current list and clear last crafted item
    public void RemoveLetter(char letter)
    {
        currentLetters.Remove(letter);

        // If there is a previously crafted item displayed, remove it
        if (_lastCraftedItem != null)
        {
            Destroy(_lastCraftedItem);
            _lastCraftedItem = null;
        }
    }

    // EventSystem drop handler: called when an InventoryItem is dropped onto this area
    public void OnDrop(PointerEventData eventData)
    {
        // Read the InventoryItem from the object being dragged
        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item == null) return;

        // Reparent the dropped letter under this crafting area
        item.parentAfterDrag = transform;

        // After a drop, check if the current letters match any recipe
        CheckForRecipe();
    }

    // Walk through child objects to build a letter list and re-check only when changed
    private void SyncLettersWithChildren()
    {
        List<char> newLetters = new List<char>();

        // Collect letters from all child InventoryItems
        foreach (Transform child in transform)
        {
            InventoryItem item = child.GetComponent<InventoryItem>();
            if (item != null)
            {
                newLetters.Add(item.letter);
            }
        }

        // Only update + evaluate when the set/order has changed
        if (!ListsEqual(currentLetters, newLetters))
        {
            currentLetters = newLetters;
            CheckForRecipe();
        }
    }

    // Compare two char lists for equal length and element-by-element equality
    private bool ListsEqual(List<char> a, List<char> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    // Build the current word and test it against all known recipes
    private void CheckForRecipe()
    {
        string currentWord = new string(currentLetters.ToArray());

        foreach (WordRecipe recipe in recipes)
        {
            if (currentWord == recipe.requiredWord)
            {
                CraftItem(recipe);
                return;
            }
        }
    }

    // Instantiate the recipe's result prefab at the spawn point and wire up click handling
    private void CraftItem(WordRecipe recipe)
    {
        Debug.Log($"Crafted: {recipe.requiredWord}!");

        if (spawnPoint != null && recipe.resultPrefab != null)
        {
            // Replace the previous crafted item if one exists
            if (_lastCraftedItem != null)
            {
                Destroy(_lastCraftedItem);
            }

            // Create the crafted item as a child of the spawnPoint
            _lastCraftedItem = Instantiate(
                recipe.resultPrefab,
                spawnPoint.position,
                Quaternion.identity,
                spawnPoint
            );

            // Add click behavior so the item can be used by the player
            AddClickHandler(_lastCraftedItem);
        }
        else
        {
            Debug.LogError("SpawnPoint or resultPrefab is not assigned!");
        }
    }

    // Ensure collider + EventTrigger exist, then register a click callback
    private void AddClickHandler(GameObject craftedItem)
    {
        // Ensure the item has a collider (required for pointer events in many setups)
        if (craftedItem.GetComponent<Collider>() == null)
        {
            craftedItem.AddComponent<BoxCollider>();
        }

        // Add or get EventTrigger for pointer click handling
        EventTrigger trigger = craftedItem.GetComponent<EventTrigger>() ?? craftedItem.AddComponent<EventTrigger>();

        // Clear existing triggers to avoid duplicate subscriptions
        trigger.triggers.Clear();

        // Create a click entry and bind to OnCraftedItemClick
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((data) => OnCraftedItemClick(craftedItem));

        trigger.triggers.Add(entry);
    }

    // When the crafted item is clicked, apply its effect and clear letters/items
    private void OnCraftedItemClick(GameObject craftedItem)
    {
        // Try to use the item on the player before cleanup
        ItemUse itemUse = craftedItem.GetComponent<ItemUse>();
        if (itemUse != null)
        {
            // Use the item on the player (found by tag here)
            itemUse.Use(GameObject.FindGameObjectWithTag("Player")); // Or your player reference
        }

        // Clear all letter GameObjects from the crafting area
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Remove the crafted item instance and reset state
        Destroy(craftedItem);
        currentLetters.Clear();
        _lastCraftedItem = null;

        Debug.Log("Item used and letters cleared!");
    }
}
