using UnityEngine;
using UnityEngine.EventSystems;

public class DeathOverlay : MonoBehaviour
{
    [SerializeField]
    private Button _respawnButton;

    private void Start()
    {
        _respawnButton.Click += Respawn;
    }

    private void Respawn()
    {
        PlayerController.Instance.Respawn();
        gameObject.SetActive(false);
    }
}
