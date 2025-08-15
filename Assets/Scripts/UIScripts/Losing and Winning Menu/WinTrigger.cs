using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private bool isShown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isShown) return;

        if (collision.CompareTag("Player"))
        {
            isShown = true;
            UIManager.Instance.ShowWinMenu();
            Debug.Log("Win screen activated");
        }
    }
}
