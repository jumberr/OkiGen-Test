using UnityEngine;

public class ObstaclesBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerCubes>(out var playerCubes)) return;
        playerCubes.DeleteCube(transform.position);
    }
}