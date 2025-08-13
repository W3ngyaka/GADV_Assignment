using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public char letter;

    [HideInInspector] public Transform parentAfterDrag;

    private CraftingArea lastCraftingArea;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        lastCraftingArea = parentAfterDrag.GetComponent<CraftingArea>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;

        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");
        image.raycastTarget = true;

        if (transform.parent == transform.root)
        {
            ReturnToParent();
        }

        if (lastCraftingArea != null && parentAfterDrag != lastCraftingArea.transform)
        {
            lastCraftingArea.RemoveLetter(letter);
        }

    }

    private void ReturnToParent()
    {
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero; // Snap back to slot center
    }
}

