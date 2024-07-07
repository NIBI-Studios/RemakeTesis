using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform questionsParent;
    [SerializeField] private GameObject questionPrefab;
    private List<GameObject> instantiatedPrefabs = new List<GameObject>();
    private int currentIndex = 0;
    private bool isFetchingData = false;
    public void SetQuiz(Quiz quiz)
    {
        title.text = quiz.name;
        if (!isFetchingData)
        {
            StartCoroutine(FetchQuestions(quiz));
        }
    }

    private IEnumerator FetchQuestions(Quiz quiz)
    {
        isFetchingData = true;
        using UnityWebRequest request = UnityWebRequest.Get($"{Constants.BASE_URI}/quiz/{quiz.id}");
        yield return request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
        quiz.questions = JsonUtility.FromJson<Quiz>(request.downloadHandler.text).questions;
        int i = 0;
        foreach (Question question in quiz.questions)
        {
            GameObject instantiated = Instantiate(questionPrefab, questionsParent);
            instantiated.name = question.text;
            instantiated.GetComponent<QuestionManager>().SetQuestion(question);
            instantiatedPrefabs.Add(instantiated);
            Button nextButton = instantiated.transform.FindInChildren("NextButton").GetComponent<Button>();
            Button backButton = instantiated.transform.FindInChildren("PreviousButton").GetComponent<Button>();
            int index = i;
            nextButton.onClick.AddListener(() => ShowNextQuestion(index));
            backButton.onClick.AddListener(() => ShowPreviousQuestion(index));
            instantiated.SetActive(false);
            if (instantiatedPrefabs.Count > 0)
            {
                instantiatedPrefabs[currentIndex].SetActive(true);
            }
            i++;
        }
        isFetchingData = false;
    }

    void ShowNextQuestion(int current)
    {
        instantiatedPrefabs[current].SetActive(false);
        currentIndex = (current + 1) % instantiatedPrefabs.Count;
        instantiatedPrefabs[currentIndex].SetActive(true);
    }

    void ShowPreviousQuestion(int current)
    {
        instantiatedPrefabs[current].SetActive(false);
        currentIndex = (current - 1 + instantiatedPrefabs.Count) % instantiatedPrefabs.Count;
        instantiatedPrefabs[currentIndex].SetActive(true);
    }

    private void OnDisable()
    {
        foreach (Transform child in questionsParent)
        {
            Destroy(child.gameObject);
        }
    }
}
