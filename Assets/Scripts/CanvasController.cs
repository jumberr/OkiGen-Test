using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private Transform player;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Image cursor;
    private float duration = 2f;

    public bool Tapped { get; set; }

    public Button RestartButton => restartButton;
    public Button NextLevelButton => nextLevelButton;

    private Player coins;
    private void Start()
    {
        coins = player.GetComponent<Player>();
        StartCoroutine(ChangeCursorPos());
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        while (true)
        {
            diamondText.text = coins.Coins.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private IEnumerator ChangeCursorPos()
    {
        var prevPos = cursor.transform.position;
        var y = -30f;
        while (!Tapped)
        {
            var pos = prevPos;
            pos.x *= -1f;
            pos.y = y;
            cursor.transform.DOLocalMove(pos, duration);
            prevPos = pos;
            yield return new WaitForSeconds(duration);
        }
    }

    public void OffCursor()
    {
        cursor.gameObject.SetActive(false);
    }

    public void Restart() => SceneManager.LoadScene(0);
    public void LoadNextLevel() => SceneManager.LoadScene(0);

}
