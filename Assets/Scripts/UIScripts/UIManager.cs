using UnityEngine;

public class UIManager : MonoBehaviour
{
    // === Singleton Instance ===
    // Allows other scripts to quickly check UI state by calling UIManager.Instance
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject inventoryPanel; // Reference to the inventory UI panel
    public GameObject gameMenu;       // Reference to the game menu (pause/settings/etc.)

    // --- Unity lifecycle ---
    void Awake()
    {
        // Assign the singleton instance so this class can be accessed globally
        Instance = this;
    }

    /// <summary>
    /// Returns true if either the inventory panel or the game menu is currently open.
    /// Used to block player actions (like attacking) while menus are active.
    /// </summary>
    public bool IsAnyMenuOpen()
    {
        return inventoryPanel.activeSelf || gameMenu.activeSelf;
    }
}
