using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritanceExcerciseManager : MonoBehaviour
{
    public void HideSubclassMenus(Transform subclassParent)
    {
        foreach (Transform transform in subclassParent)
        {
            transform.gameObject.SetActive(false);
        }
    }
    public void ActiveCanvas(GameObject go)
    {
        go.SetActive(true);
    }
}
