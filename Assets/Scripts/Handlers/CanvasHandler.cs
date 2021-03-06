using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Handlers
{
    public class CanvasHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI diamondText;
        [SerializeField] private Transform player;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Image cursor;
        private float duration = 2f;

        private bool tapped;

        public Button RestartButton => restartButton;
        public Button NextLevelButton => nextLevelButton;

        private PlayerHandler coins;
        private void Start()
        {
            coins = player.GetComponent<PlayerHandler>();
            StartCoroutine(ChangeCursorPos());
            StartCoroutine(ShowText());
        }

        private IEnumerator ShowText()
        {
            while (true)
            {
                diamondText.text = coins.GetCoins().ToString();
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        private IEnumerator ChangeCursorPos()
        {
            var prevPos = cursor.transform.position;
            var y = -30f;
            while (!tapped)
            {
                var pos = prevPos;
                pos.x *= -1f;
                pos.y = y;
                cursor.transform.DOLocalMove(pos, duration);
                prevPos = pos;
                yield return new WaitForSeconds(duration);
            }
        }

        private void OffCursor()
        {
            cursor.gameObject.SetActive(false);
        }

        public void GetStartingInput()
        {
            tapped = !tapped;
            OffCursor();
        }

        public void Restart() => SceneManager.LoadScene(0);
        public void LoadNextLevel() => SceneManager.LoadScene(0);

    }
}
