using UnityEngine;

public class Finish : MonoBehaviour
{
    private Canvas canvas;
    private CanvasHandler canvasHandler;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        canvasHandler = canvas.GetComponent<CanvasHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerCubes>(out var playerCubes)) return;
        canvasHandler.NextLevelButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
