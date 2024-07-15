using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private VideoTimeScrubControl playerControl;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Transform imageAnswersParent;
    [SerializeField] private Transform answersParent;
    [SerializeField] private GameObject imageAnswerPrefab;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private GameObject continueToExamplesButton;
    [SerializeField] private GameObject examples;
    [SerializeField] private string pillar;
    public List<VideoQuestion> questions;
    private readonly float tolerance = 0.1f;
    private double lastPlayerTime = 0;
    private bool isUpdatingTheory;
    private bool isUpdatingExcercise;
    private readonly List<Color> _answerColors = new() {
        new Color(0, 1, 1, 1),
        new Color(1, 1, 0, 1),
        new Color(0.5f, 1, 0.5f, 1),
        new Color(1, 0.5f, 0.5f, 1)
    };
    private void Start()
    {
        player.loopPointReached += (player) =>
        {
            continueToExamplesButton.SetActive(true);
            continueToExamplesButton.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                if (!isUpdatingExcercise)
                {
                    StartCoroutine(nameof(CheckExcercise));
                }
                continueToExamplesButton.SetActive(false);
                examples.SetActive(true);
            });
            if (!isUpdatingTheory)
            {
                StartCoroutine(nameof(CheckTheory));
            }
        };
    }
    private void Update()
    {
        if (player.isPlaying)
        {
            if (player.time < lastPlayerTime)
            {
                ResetQuestions();
            }
            lastPlayerTime = player.time;
            foreach (var question in questions)
            {
                if (!question.hasBeenAsked && Mathf.Abs((float)player.time - question.timeOfQuestion) <= tolerance)
                {
                    questionPanel.SetActive(true);
                    question.hasBeenAsked = true;
                    questionText.text = question.text;
                    if (question.imageAnswers.Count > 0)
                    {
                        imageAnswersParent.gameObject.SetActive(true);
                        foreach (var answer in question.imageAnswers)
                        {
                            GameObject instantiated = Instantiate(imageAnswerPrefab, imageAnswersParent);
                            instantiated.GetComponent<Image>().sprite = answer.image;
                            instantiated.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                if (answer.isCorrect)
                                {
                                    questionPanel.SetActive(false);
                                    foreach (Transform answer in imageAnswersParent)
                                    {
                                        Destroy(answer.gameObject);
                                    }
                                    imageAnswersParent.gameObject.SetActive(false);
                                    playerControl.VideoPlay();
                                }
                                else
                                {
                                    Debug.Log("Respuesta equivocada");
                                }
                            });
                        }
                    }
                    else
                    {
                        answersParent.gameObject.SetActive(true);
                        Shuffle(question.answers);
                        int i = 0;
                        foreach (var answer in question.answers)
                        {
                            GameObject instantiated = Instantiate(answerPrefab, answersParent);
                            instantiated.GetComponentInChildren<TextMeshProUGUI>().text = answer.text;
                            instantiated.GetComponent<Image>().color = _answerColors[i++];
                            instantiated.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                if (answer.isCorrect)
                                {
                                    questionPanel.SetActive(false);
                                    foreach (Transform answer in answersParent)
                                    {
                                        Destroy(answer.gameObject);
                                    }
                                    answersParent.gameObject.SetActive(false);
                                    playerControl.VideoPlay();
                                }
                                else
                                {
                                    Debug.Log("Respuesta equivocada");
                                }
                            });
                        }
                    }
                    playerControl.VideoStop();
                    break;
                }
            }
        }
    }

    private void ResetQuestions()
    {
        foreach (var question in questions)
        {
            question.hasBeenAsked = false;
        }
    }

    private IEnumerator CheckTheory()
    {
        isUpdatingTheory = true;
        var json = $"{{\"{pillar}Theory\":\"True\"}}";
        using UnityWebRequest request = UnityWebRequest.Put($"{Constants.BASE_URI}/progress/{User.UserId}", json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        isUpdatingTheory = false;
    }

    private IEnumerator CheckExcercise()
    {
        isUpdatingExcercise = true;
        var json = $"{{\"{pillar}Excercise\":\"True\"}}";
        using UnityWebRequest request = UnityWebRequest.Put($"{Constants.BASE_URI}/progress/{User.UserId}", json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        isUpdatingExcercise = false;
    }

    private void Shuffle<T>(List<T> list)
    {
        System.Random rng = new();
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}