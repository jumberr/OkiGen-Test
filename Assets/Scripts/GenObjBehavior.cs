using Handlers;
using UnityEngine;

public class GenObjBehavior : MonoBehaviour
{
    public delegate void OnChangeValues();

    public static event OnChangeValues OnAddCoins;

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Coin"))
        {
            if (other.gameObject.GetComponent<PlayerHandler>())
            {
                OnAddCoins?.Invoke();
                gameObject.SetActive(false);
            }
        }
        else if (gameObject.CompareTag("Cube"))
        {
            if (other.gameObject.TryGetComponent<PlayerCubes>(out var cubes))
            {
                cubes.AddCube();
                gameObject.SetActive(false);
            }
        }
        else if (gameObject.CompareTag("Obstacle"))
        {
            if (other.gameObject.TryGetComponent<CapsuleCollider>(out var playerCollider))
            {
                Debug.Log("END!");
                Time.timeScale = 0f;
            }

            // else if (other.gameObject.TryGetComponent<PlayerCubes>(out var playerCubes))
            // {
            //     playerCubes.DeleteCube();
            // }
        }
    }
}