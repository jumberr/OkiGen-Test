using UnityEngine;

public class Finish : MonoBehaviour
{
    private Canvas canvas;
    private CanvasController canvasController;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        canvasController = canvas.GetComponent<CanvasController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerCubes>(out var playerCubes)) return;
        canvasController.NextLevelButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
