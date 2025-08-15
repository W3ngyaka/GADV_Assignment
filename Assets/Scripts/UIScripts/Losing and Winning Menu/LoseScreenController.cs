using UnityEngine;

public class LoseScreenController : MonoBehaviour
{
    public PlayerHealth player;

    private bool isShown = false;

    void Update()
    {
        if (!isShown && (player == null || player.gameObject == null))
        {
            isShown = true;
            UIManager.Instance.ShowLoseMenu();
            Debug.Log("Lose screen activated");
        }
    }
}
