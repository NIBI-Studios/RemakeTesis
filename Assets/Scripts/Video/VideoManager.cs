using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
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
    public List<VideoQuestion> questions;
    private readonly float tolerance = 0.1f;
    private double lastPlayerTime = 0;
    private bool isRunning;
    private void Start()
    {
        player.loopPointReached += (player) =>
        {
            if (!isRunning)
            {
                StartCoroutine(nameof(CheckActivity));
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
                        foreach (var answer in question.answers)
                        {
                            GameObject instantiated = Instantiate(answerPrefab, answersParent);
                            instantiated.GetComponentInChildren<TextMeshProUGUI>().text = answer.text;
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

    private IEnumerator CheckActivity()
    {
        isRunning = true;
        yield return null;
        isRunning = false;
    }
}