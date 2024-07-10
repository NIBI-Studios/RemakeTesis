using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritanceGameManager : MonoBehaviour
{
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject life1;
    [SerializeField] private GameObject life2;
    [SerializeField] private GameObject life3;
    [SerializeField] private int maxPoints;
    private int currentPoints;
    private static InheritanceGameManager _instance;
    public static InheritanceGameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Start()
    {

        if (_instance == null)
        {
            _instance = this;
        }
        if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void Correct()
    {
        currentPoints += 1;
        if (currentPoints == maxPoints)
        {
            winCanvas.SetActive(true);
        }
    }
    public void Incorrect()
    {
        if (!life3.activeInHierarchy)
        {
            loseCanvas.SetActive(true);
            Destroy(this);
        }
        if (!life2.activeInHierarchy)
        {
            life3.SetActive(false);
        }
        if (!life1.activeInHierarchy)
        {
            life2.SetActive(false);
        }
        if (life1.activeInHierarchy)
        {
            life1.SetActive(false);
        }
    }
}
