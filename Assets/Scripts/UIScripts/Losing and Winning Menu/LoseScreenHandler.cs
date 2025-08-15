using UnityEngine;


public class LoseScreenController : MonoBehaviour
{
    [Header("References")]
    public GameObject losePanel;     // Assign your LosePanel prefab/UI here
    public PlayerHealth player;      // Drag in the Player object (with PlayerHealth)

    private bool isShown = false;

    void Start()
    {
        // Make sure the lose panel starts hidden
        if (losePanel != null)
            losePanel.SetActive(false);
    }

    void Update()
    {
        // Check if player is gone or has died
        if (!isShown && (player == null || player.gameObject == null))
        {
            ShowLoseScreen();
        }
    }

    private void ShowLoseScreen()
    {
        isShown = true;

        if (losePanel != null)
            losePanel.SetActive(true);

        // Optionally pause the game (comment out if you prefer not to freeze)
        // Time.timeScale = 0f;
    }
}