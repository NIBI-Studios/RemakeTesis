using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform questionsParent;
    [SerializeField] private GameObject questionPrefab;
    private readonly List<GameObject> instantiatedPrefabs = new();
    private int currentIndex = 0;
    private bool isFetchingData = false;
    private int correctAnswers;
    private int totalQuestions;
    private string quizId;
    private readonly Dictionary<int, bool> answeredQuestions = new();
    private bool isGrading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetQuiz(Quiz quiz)
    {
        title.text = quiz.name;
        quizId = quiz.id;
        if (!isFetchingData)
        {
            StartCoroutine(FetchQuestions(quiz));
        }
    }

    private IEnumerator FetchQuestions(Quiz quiz)
    {
        isFetchingData = true;
        totalQuestions = 0;
        using UnityWebRequest request = UnityWebRequest.Get($"{Constants.BASE_URI}/quiz/{quiz.id}");
        yield return request.SendWebRequest();
        quiz.questions = JsonUtility.FromJson<Quiz>(request.downloadHandler.text).questions;
        int i = 0;
        foreach (Question question in quiz.questions)
        {
            int index = i;
            GameObject instantiated = Instantiate(questionPrefab, questionsParent);
            instantiated.name = question.text;
            instantiated.GetComponent<QuestionManager>().SetQuestion(question, index);
            instantiatedPrefabs.Add(instantiated);
            Button nextButton = instantiated.transform.FindInChildren("NextButton").GetComponent<Button>();
            Button backButton = instantiated.transform.FindInChildren("PreviousButton").GetComponent<Button>();
            nextButton.onClick.AddListener(() => ShowNextQuestion(index));
            backButton.onClick.AddListener(() => ShowPreviousQuestion(index));
            instantiated.SetActive(false);
            if (instantiatedPrefabs.Count > 0)
            {
                instantiatedPrefabs[currentIndex].SetActive(true);
            }
            i++;
        }
        totalQuestions = i;
        isFetchingData = false;
    }

    public void AnswerQuestion(int questionIndex, bool isCorrect)
    {
        if (answeredQuestions.ContainsKey(questionIndex))
        {
            answeredQuestions[questionIndex] = isCorrect;
            return;
        }
        answeredQuestions.Add(questionIndex, isCorrect);
    }
    public void GetGrade()
    {
        correctAnswers = 0;
        foreach (bool isCorrect in answeredQuestions.Values)
        {
            if (isCorrect)
            {
                correctAnswers++;
            }
        }
        float grade = correctAnswers / (float)totalQuestions * 10f;
        if (!isGrading)
        {
            StartCoroutine(GradeQuiz(grade));
        }
    }

    IEnumerator GradeQuiz(float grade)
    {
        isGrading = true;
        string json = $"{{\"studentId\":\"{User.UserId}\",\"quizId\":\"{quizId}\",\"grade\":{grade}}}";
        using UnityWebRequest webRequest = UnityWebRequest.Post($"{Constants.BASE_URI}/student/grade", json, "application/json");
        yield return webRequest.SendWebRequest();
        isGrading = false;
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
