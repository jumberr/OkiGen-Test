using System;
using System.Collections;
using System.Collections.Generic;
using Handlers;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerCubes : MonoBehaviour
{
    [SerializeField] private Transform playerGfx;
    [SerializeField] private Transform playerCubesParent;
    [SerializeField] private Transform exCubesParent;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject firstCube;
    
    //[SerializeField] private CharacterController characterController;
    [SerializeField] private CanvasHandler canvasHandler; 
    private readonly List<Rigidbody> listRigidbody = new List<Rigidbody>();
    private readonly List<GameObject> playerCubes = new List<GameObject>();
    //private int amountOfCubes = 1;
    private float heightCube = 0.5f;
    //private float duration = 2f;
    //private float prevDelPosX;
    //private bool isChangedDel;
    //private float eps = 0.001f;
    private void Awake()
    {
        playerCubes.Add(firstCube);
        listRigidbody.Add(firstCube.GetComponent<Rigidbody>());
    }

    // public void AddCube(Vector3 pos)
    // {
    //     Add();
    // }
    // public void DeleteCube(Vector3 pos)
    // {
    //     if (amountOfCubes <= 1)
    //     {
    //         canvasHandler.RestartButton.gameObject.SetActive(true);
    //         Time.timeScale = 0;
    //     }
    //     else
    //     {
    //         if (!isChangedDel)
    //         {
    //             isChangedDel = !isChangedDel;
    //             StartCoroutine(ChangeValueDel());
    //             prevDelPosX = pos.x;
    //             Del();
    //         }
    //         else
    //         {
    //             if (!(Math.Abs(pos.x - prevDelPosX) < eps)) return;
    //             Del();
    //         }
    //     }
    // }

    // private void Del()
    // {
    //     ChangePositionByY(playerGfx, -heightCube);
    //     ChangePositionByY(playerCubesParent, -heightCube);
    //     ChangeCCHeightAndCenter(characterController, -heightCube);
    //     // off cubes under player
    //     playerCubes[playerCubes.Count - 1].SetActive(false);
    //     playerCubes.RemoveAt(playerCubes.Count - 1);
    //     amountOfCubes -= 1;
    //     Debug.Log("amountOfCubes -= 1;");
    // }

    // private void Add()
    // {
    //     ChangePositionByY(playerGfx, heightCube);
    //     ChangePositionByY(playerCubesParent, heightCube);
    //     ChangeCCHeightAndCenter(characterController, heightCube);
    //
    //     // change cube position and spawn cubes
    //     var position = playerCubes[playerCubes.Count - 1].transform.position;
    //     var cubePos = new Vector3(position.x, position.y - heightCube, position.z);
    //     var cube = Instantiate(cubePrefab, cubePos, Quaternion.identity, playerCubesParent);
    //     playerCubes.Add(cube);
    //
    //     Debug.Log("amountOfCubes += 1;");
    //     amountOfCubes += 1;
    // }
    //
    // private IEnumerator ChangeValueDel()
    // {
    //     yield return new WaitForSecondsRealtime(duration);
    //     isChangedDel = false;
    // }

    public void AddCube()
    {
        ChangePositionByY(playerGfx, heightCube);
        ChangePositionByY(playerCubesParent, heightCube);
        
        // change cube position and spawn cubes
        var position = playerCubes[playerCubes.Count - 1].transform.position;
        var cubePos = new Vector3(position.x, position.y - heightCube, position.z);
        var cube = Instantiate(cubePrefab, cubePos, Quaternion.identity, playerCubesParent);
        playerCubes.Add(cube);

        cube.GetComponent<JointDestruction>().OnJointDestruction += DeleteCube;
        
        //FixedJoint fixedJoint = gameObject.
        var fixedJoint = cube.GetComponent<FixedJoint>();
        var currBody = cube.GetComponent<Rigidbody>();
        fixedJoint.connectedBody = listRigidbody[listRigidbody.Count - 1]; // previous body
        listRigidbody.Add(currBody);
        
        //prevCubeRigidbody = currBody;
        Debug.Log("amountOfCubes += 1;");
    }

    private void DeleteCube()
    {
        
        //playerCubes[playerCubes.Count - 1].SetActive(false);
        
        playerCubes[playerCubes.Count - 1].transform.parent = exCubesParent;
        playerCubes.RemoveAt(playerCubes.Count - 1);
        listRigidbody.RemoveAt(listRigidbody.Count - 1);
        //ChangePositionByY(playerGfx, -heightCube);
        //ChangePositionByY(playerCubesParent, -heightCube);
        Debug.Log("amountOfCubes -= 1;");
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
