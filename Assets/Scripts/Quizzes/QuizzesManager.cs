using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class QuizzesManager : MonoBehaviour
{
    [SerializeField] private GameObject quizItemPrefab;
    [SerializeField] private Transform quizesParent;
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
                    instantiated.GetComponentInChildren<TextMeshProUGUI>().text = quiz.name;
                }
            }
        }
        isFetchingQuizzes = false;
    }
}
