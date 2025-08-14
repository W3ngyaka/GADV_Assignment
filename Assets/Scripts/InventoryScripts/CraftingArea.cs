using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingArea : MonoBehaviour, IDropHandler
{
    public List<WordRecipe> recipes;
    public List<char> currentLetters = new List<char>();
    public Transform spawnPoint;

    private GameObject _lastCraftedItem;

    void Update()
    {
        // Constantly sync letters with children
        SyncLettersWithChildren();
    }

    public void RemoveLetter(char letter)
    {
        currentLetters.Remove(letter);
        if (_lastCraftedItem != null)
        {
            Destroy(_lastCraftedItem);
            _lastCraftedItem = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item == null) return;

        item.parentAfterDrag = transform;
        CheckForRecipe();
    }

    private void SyncLettersWithChildren()
    {
        List<char> newLetters = new List<char>();
        foreach (Transform child in transform)
        {
            InventoryItem item = child.GetComponent<InventoryItem>();
            if (item != null)
            {
                newLetters.Add(item.letter);
            }
        }

        // Only update if changed to prevent unnecessary checks
        if (!ListsEqual(currentLetters, newLetters))
        {
            currentLetters = newLetters;
            CheckForRecipe();
        }
    }

    private bool ListsEqual(List<char> a, List<char> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

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

    // ... [Rest of the original methods remain exactly the same] ...
    private void CraftItem(WordRecipe recipe)
    {
        Debug.Log($"Crafted: {recipe.requiredWord}!");

        if (spawnPoint != null && recipe.resultPrefab != null)
        {
            if (_lastCraftedItem != null)
            {
                Destroy(_lastCraftedItem);
            }

            _lastCraftedItem = Instantiate(
                recipe.resultPrefab,
                spawnPoint.position,
                Quaternion.identity,
                spawnPoint
            );

            AddClickHandler(_lastCraftedItem);
        }
        else
        {
            Debug.LogError("SpawnPoint or resultPrefab is not assigned!");
        }
    }

    private void AddClickHandler(GameObject craftedItem)
    {
        // Ensure the item has a collider
        if (craftedItem.GetComponent<Collider>() == null)
        {
            craftedItem.AddComponent<BoxCollider>();
        }

        // Add or get EventTrigger
        EventTrigger trigger = craftedItem.GetComponent<EventTrigger>() ?? craftedItem.AddComponent<EventTrigger>();

        // Clear existing triggers to avoid duplicates
        trigger.triggers.Clear();

        // Add new click handler
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((data) => OnCraftedItemClick(craftedItem));
        trigger.triggers.Add(entry);
    }

    private void OnCraftedItemClick(GameObject craftedItem)
    {
        // Get the ItemUse component and execute effect before destruction
        ItemUse itemUse = craftedItem.GetComponent<ItemUse>();
        if (itemUse != null)
        {
            // Use the item on the player before cleaning up
            itemUse.Use(GameObject.FindGameObjectWithTag("Player")); // Or your player reference
        }

        // Clean up letters and crafted item
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Destroy(craftedItem);
        currentLetters.Clear();
        _lastCraftedItem = null;

        Debug.Log("Item used and letters cleared!");
    }
}