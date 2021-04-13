using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCubes : MonoBehaviour
{
    [SerializeField] private Transform playerGfx;
    [SerializeField] private Transform playerCubesParent;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject firstCube;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CanvasController canvasController;
    private List<GameObject> playerCubes = new List<GameObject>();
    private int amountOfCubes = 1;
    private float heightCube = 0.5f;

    private void Awake()
    {
        playerCubes.Add(firstCube);
    }

    public void AddCube()
    {
        ChangePositionByY(playerGfx, heightCube);
        ChangePositionByY(playerCubesParent, heightCube);
        ChangeCCHeightAndCenter(characterController, heightCube);

        // change cube position and spawn cubes
        var position = playerCubes[playerCubes.Count - 1].transform.position;
        var cubePos = new Vector3(position.x,position.y - heightCube,position.z);
        var cube = Instantiate(cubePrefab, cubePos, Quaternion.identity, playerCubesParent);
        playerCubes.Add(cube);
        
        Debug.Log("amountOfCubes += 1;");
        amountOfCubes += 1;
    }
    public void DeleteCube()
    {
        if (amountOfCubes <= 1)
        {
            canvasController.RestartButton.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            ChangePositionByY(playerGfx, -heightCube);
            ChangePositionByY(playerCubesParent, -heightCube);
            ChangeCCHeightAndCenter(characterController, -heightCube);
            // off cubes under player
            playerCubes[playerCubes.Count - 1].SetActive(false);
            playerCubes.RemoveAt(playerCubes.Count - 1);
            amountOfCubes -= 1;
            Debug.Log("amountOfCubes -= 1;");
        }

    }

    private void ChangePositionByY(Transform target, float valueY)
    {
        var pos = target.position;
        pos = new Vector3(pos.x, pos.y += valueY, pos.z);
        target.position = pos;
    }

    private void ChangeCCHeightAndCenter(CharacterController controller, float valueY)
    {
        controller.height += valueY; // change CC height
        var center = controller.center;
        center = new Vector3(center.x, center.y + valueY/2, center.z); // change CC center Y
        controller.center = center;
    }
}
