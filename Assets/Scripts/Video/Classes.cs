using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VideoQuestion
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
