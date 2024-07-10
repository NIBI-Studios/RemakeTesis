using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public string parentClass;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(parentClass))
        {
            InheritanceGameManager.Instance.Correct();
            Destroy(this);
        }
        else
        {
            InheritanceGameManager.Instance.Incorrect();
        }
    }
}
