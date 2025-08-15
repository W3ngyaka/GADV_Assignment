using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject inventoryPanel;
    public GameObject gameMenu;

    [Header("Game Over / Win")]
    public GameObject losePanel;
    public GameObject winPanel;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Make sure ALL panels are hidden at game start
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (gameMenu != null) gameMenu.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    public bool IsAnyMenuOpen()
    {
        return (inventoryPanel != null && inventoryPanel.activeSelf)
            || (gameMenu != null && gameMenu.activeSelf)
            || (losePanel != null && losePanel.activeSelf)
            || (winPanel != null && winPanel.activeSelf);
    }

    public void ShowLoseMenu()
    {
        if (losePanel != null)
            losePanel.SetActive(true);
    }

    public void ShowWinMenu()
    {
        if (winPanel != null)
            winPanel.SetActive(true);
    }
}
