using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VideoQuestion
{
    public string text;
    public float timeOfQuestion;
    public List<VideoAnswer> answers;
    public List<VideoImageAnswer> imageAnswers;
    [NonSerialized] public bool hasBeenAsked;
}

[Serializable]
public class VideoAnswer
{
    public string text;
    public bool isCorrect;
}
[Serializable]
public class VideoImageAnswer
{
    public Sprite image;
    public bool isCorrect;
}
