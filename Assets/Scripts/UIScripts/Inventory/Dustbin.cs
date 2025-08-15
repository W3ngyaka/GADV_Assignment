using UnityEngine;
using UnityEngine.EventSystems;

public class Dustbin : MonoBehaviour, IDropHandler
{
    // UI EventSystem callback: fired when a draggable object is dropped on this Dustbin
    public void OnDrop(PointerEventData eventData)
    {
        // The GameObject currently being dragged by the EventSystem
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            // The dropped object should carry an InventoryItem component
            InventoryItem item = droppedObject.GetComponent<InventoryItem>();
            if (item != null)
            {
                // Remove from inventory (if there's an active PlayerInventory in the scene)
                PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();
                if (inventory != null)
                {
                    // This assumes you have a RemoveLetter method in PlayerInventory
                    // If not, you'll need to add it (kept as original note)
                    inventory.RemoveLetter(item.letter);
                }

                // Destroy the dropped letter's GameObject after removal
                Destroy(droppedObject);

                // Log the deletion for debugging
                Debug.Log($"Deleted letter: {item.letter}");
            }
        }
    }
}
