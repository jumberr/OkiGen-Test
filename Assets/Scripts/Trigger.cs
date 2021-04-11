using UnityEngine;

public class Trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Movement>() != null)
        {
            // todo try to delete blocks
            Debug.Log($"Invoked from: {name}, {transform.position}, {other.name}, {other.transform.position}");
        }
    }
}