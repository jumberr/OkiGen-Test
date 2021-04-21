using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDestruction : MonoBehaviour
{
    public delegate void OnDestruction();
    public event OnDestruction OnJointDestruction;
    
    private void OnJointBreak(float breakForce)
    {
        OnJointDestruction?.Invoke();
    }
}
