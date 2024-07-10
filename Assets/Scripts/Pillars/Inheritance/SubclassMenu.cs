using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubclassMenu : MonoBehaviour
{
    public GameObject subclass;
    public void OpenSubclassMenu()
    {
        subclass.SetActive(true);
    }

}
