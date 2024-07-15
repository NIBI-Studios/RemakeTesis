using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EncapsulationGameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameSceneObject;
    [SerializeField] private GameObject tutorialObject;
    [SerializeField] private GameObject challenge1;
    [SerializeField] private GameObject challenge2;
    [SerializeField] private GameObject challenge3;
    [SerializeField] private GameObject challenge4;
    [SerializeField] private GameObject goal1;
    [SerializeField] private GameObject goal2;
    [SerializeField] private GameObject goal3;
    [SerializeField] private GameObject goal4;
    [SerializeField] private GameObject error1;
    [SerializeField] private GameObject error2;
    [SerializeField] private GameObject error3;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;
    private int correctAnswers = 0;
    public void StartGame()
    {
        gameSceneObject.SetActive(true);
        tutorialObject.SetActive(false);
    }
    public void OnChosenCorrect(GameObject table)
    {
        table.SetActive(false);
        correctAnswers = Mathf.Clamp(++correctAnswers, 0, 2);
    }
    public void OnChosenIncorrect()
    {
        if (error2.activeInHierarchy)
        {
            error3.SetActive(true);
        }
        if (error1.activeInHierarchy)
        {
            error2.SetActive(true);
        }
        else
        {
            error1.SetActive(true);
        }
        if (error3.activeInHierarchy)
        {
            challenge1.SetActive(false);
            challenge2.SetActive(false);
            challenge3.SetActive(false);
            challenge4.SetActive(false);
            loseScreen.SetActive(true);
        }
    }
    public void GoToChallenge2()
    {
        if (correctAnswers == 2)
        {
            correctAnswers = 0;
            challenge1.SetActive(false);
            goal1.SetActive(true);
            challenge2.SetActive(true);
        }
    }
    public void GoToChallenge3()
    {
        if (correctAnswers == 2)
        {
            correctAnswers = 0;
            challenge2.SetActive(false);
            goal2.SetActive(true);
            challenge3.SetActive(true);
        }
    }
    public void GoToChallenge4()
    {
        if (correctAnswers == 2)
        {
            correctAnswers = 0;
            challenge3.SetActive(false);
            goal3.SetActive(true);
            challenge4.SetActive(true);
        }
    }
    public void Win()
    {
        if (correctAnswers == 2)
        {
            correctAnswers = 0;
            challenge4.SetActive(false);
            goal4.SetActive(true);
            winScreen.SetActive(true);
            StartCoroutine(nameof(CheckGameCompleted));
        }
        else
        {
            Debug.Log($"Faltan responder {2 - correctAnswers} cosos");
        }
    }
    private IEnumerator CheckGameCompleted()
    {
        var json = $"{{\"encapsulationGame\":\"True\"}}";
        using UnityWebRequest request = UnityWebRequest.Put($"{Constants.BASE_URI}/progress/{User.UserId}", json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
    }
}
