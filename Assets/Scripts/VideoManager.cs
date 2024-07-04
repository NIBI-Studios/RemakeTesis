using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private VideoTimeScrubControl playerControl;
    public List<Question> questions;
    private float tolerance = 0.1f;
    private double lastPlayerTime = 0;

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
                    question.hasBeenAsked = true;
                    Debug.Log(question.text);
                    if (question.imageAnswers.Count > 0)
                    {
                        foreach (var answer in question.imageAnswers)
                        {
                            Debug.Log(answer);
                        }
                    }
                    else
                    {
                        foreach (var answer in question.answers)
                        {
                            Debug.Log(answer.text);
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
}

[Serializable]
public class Question
{
    public string text;
    public float timeOfQuestion;
    public List<Answer> answers;
    public List<ImageAnswer> imageAnswers;
    [NonSerialized] public bool hasBeenAsked;
}

[Serializable]
public class Answer
{
    public string text;
    public bool isCorrect;
}
[Serializable]
public class ImageAnswer
{
    public Sprite image;
    public bool isCorrect;
}