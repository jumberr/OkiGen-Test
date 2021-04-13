using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private CanvasController canvasController;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerCubes>(out var playerCubes)) return;
        canvasController.RestartButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
