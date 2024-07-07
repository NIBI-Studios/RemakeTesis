using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuizzesManager : MonoBehaviour
{
    [SerializeField] private GameObject quizItemPrefab;
    [SerializeField] private Transform quizesParent;
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private GameObject quizzesCanvas;
    private bool isFetchingQuizzes;
    private void Start()
    {
        if (!isFetchingQuizzes)
        {
            StartCoroutine(nameof(FetchQuizzes));
        }
    }
    private IEnumerator FetchQuizzes()
    {
        isFetchingQuizzes = true;
        using UnityWebRequest request = UnityWebRequest.Get($"{Constants.BASE_URI}/quiz/byCourse/{User.CourseId}");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.uri);
            Debug.Log(request.responseCode);
        }
        else
        {
            var text = $"{{\"quizzes\":{request.downloadHandler.text}}}";
            Quizes quizes = JsonUtility.FromJson<Quizes>(text);
            foreach (var quiz in quizes.quizzes)
            {
                if (quiz.isActive)
                {
                    GameObject instantiated = Instantiate(quizItemPrefab, quizesParent);
                    instantiated.transform.Find("QuizName").GetComponentInChildren<TextMeshProUGUI>().text = quiz.name;
                    using UnityWebRequest gradeRequest = UnityWebRequest.Get($"{Constants.BASE_URI}/student/grade/{User.UserId}/{quiz.id}");
                    yield return gradeRequest.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(request.uri);
                        Debug.Log(request.responseCode);
                    }
                    else
                    {
                        instantiated.transform.Find("QuizGrade").GetComponentInChildren<TextMeshProUGUI>().text = $"{gradeRequest.downloadHandler.text}/10";
                    }
                    instantiated.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        quizPanel.SetActive(true);
                        quizPanel.transform.FindInChildren("SendQuizButton").GetComponent<Button>().onClick.AddListener(() =>
                        {

                        });
                        quizPanel.GetComponent<QuizManager>().SetQuiz(quiz);
                        quizzesCanvas.SetActive(false);
                    });
                }
            }
        }
        isFetchingQuizzes = false;
    }
}
