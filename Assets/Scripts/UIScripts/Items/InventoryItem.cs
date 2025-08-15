using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Visual for this inventory letter (assigned when created)
    public Image image;

    // The character this item represents
    public char letter;

    // Parent transform to return to after dragging (set at drag start)
    [HideInInspector] public Transform parentAfterDrag;

    // Tracks the crafting area we came from (if any) to manage letter removal
    private CraftingArea lastCraftingArea;

    // --- Drag lifecycle (event-style): kept inline as requested ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");

        // Remember current parent (slot or container) for potential return
        parentAfterDrag = transform.parent;

        // Move to root so it renders above other UI while dragging
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // Disable raycast so underlying drop targets receive events
        image.raycastTarget = false;

        // If dragged out of a CraftingArea, remember it so we can undo letter state if needed
        lastCraftingArea = parentAfterDrag.GetComponent<CraftingArea>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");

        // Follow the mouse position while dragging
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");

        // Re-enable raycast so the item can be interacted with again
        image.raycastTarget = true;

        // If no new parent accepted the drop, snap back to original slot
        if (transform.parent == transform.root)
        {
            ReturnToParent();
        }

        // If we started in a CraftingArea but ended elsewhere, inform it to remove the letter
        if (lastCraftingArea != null && parentAfterDrag != lastCraftingArea.transform)
        {
            lastCraftingArea.RemoveLetter(letter);
        }
    }

    // --- Helpers (non-lifecycle) ---

    // Return to the original parent slot and center the item
    private void ReturnToParent()
    {
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero; // Snap back to slot center
    }
}
