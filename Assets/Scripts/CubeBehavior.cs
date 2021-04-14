using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerCubes>(out var cubes)) return;
        cubes.AddCube(transform.position);
        gameObject.SetActive(false);
    }
}
