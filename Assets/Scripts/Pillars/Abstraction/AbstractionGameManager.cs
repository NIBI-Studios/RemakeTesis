using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AbstractionGameManager : MonoBehaviour
{
    [SerializeField] private GameObject life1;
    [SerializeField] private GameObject life2;
    [SerializeField] private GameObject life3;
    [SerializeField] private GameObject challange1;
    [SerializeField] private GameObject challange2;
    [SerializeField] private GameObject challange3;
    [SerializeField] private GameObject challange4;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject winCanvas;
    private int currentCorrect = 0;
    private static AbstractionGameManager _instance;
    public static AbstractionGameManager Instance
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
        currentCorrect += 1;
        if (currentCorrect == 4)
        {
            if (challange4.activeInHierarchy)
            {
                winCanvas.SetActive(true);
                challange4.SetActive(false);
                currentCorrect = 0;
                StartCoroutine(nameof(CheckGameCompleted));
            }
            if (challange3.activeInHierarchy)
            {
                challange4.SetActive(true);
                challange3.SetActive(false);
                currentCorrect = 0;
            }
            if (challange2.activeInHierarchy)
            {
                challange3.SetActive(true);
                challange2.SetActive(false);
                currentCorrect = 0;
            }
            if (challange1.activeInHierarchy)
            {
                challange2.SetActive(true);
                challange1.SetActive(false);
                currentCorrect = 0;
            }
        }
    }
    public void Incorrect()
    {
        if (!life2.activeInHierarchy)
        {
            life3.SetActive(false);
            loseCanvas.SetActive(true);
            Destroy(this);
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
        var json = $"{{\"abstractionGame\":\"True\"}}";
        using UnityWebRequest request = UnityWebRequest.Put($"{Constants.BASE_URI}progress/{User.UserId}", json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
    }
}