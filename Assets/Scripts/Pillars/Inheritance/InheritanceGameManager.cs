using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
            StartCoroutine(nameof(CheckGameCompleted));
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
    private IEnumerator CheckGameCompleted()
    {
        var json = $"{{\"inheritanceGame\":\"True\"}}";
        using UnityWebRequest request = UnityWebRequest.Put($"{Constants.BASE_URI}/progress/{User.UserId}", json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
    }
}
