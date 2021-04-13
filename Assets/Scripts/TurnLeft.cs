using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TurnLeft : MonoBehaviour
{
    [SerializeField] private GameObject turnRight;
    [SerializeField] private float duration = 2f;
    private Vector3 turn = Vector3.zero;
    private BoxCollider boxCollider;
    private const float ROTATE_VALUE = -90;
    private void Awake()
    {
        //DOTween.Init(false, true);
        boxCollider = turnRight.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Movement>(out var player) is false) return;

        turnRight.SetActive(false);
        var center = boxCollider.center;
        Debug.Log($"Center: {center}");
        var playerPosition = other.transform.position;
        var pos = new Vector3(center.x, playerPosition.y, center.z);
        Debug.Log($"POS1: {pos}");
        var desiredPos = transform.TransformPoint(pos);
        Debug.Log($"POS2: {pos}");
        var rot = other.transform.rotation.eulerAngles;  //prevRot
        turn.y += ROTATE_VALUE + rot.y;
        if (turn.y > 360)
        {
            turn.y -= 360;
        }
        else if (turn.y < -360)
        {
            turn.y += 360;
        }
        other.transform.DOLocalMove(desiredPos, duration);
        other.transform.DORotate(turn, duration);

        player.OnRotate(ROTATE_VALUE);
    }

}