using System;

[Serializable]
public class Quizes
{
    public Quiz[] quizzes;
}
[Serializable]
public class Quiz
{
    public string id;
    public string name;
    public bool isActive;
    public Question[] questions;
    public Course course;
}
[Serializable]
public class Course
{
    public string id;
    public string name;
    public bool isActive;
}
[Serializable]
public class Question
{
    public string id;
    public string text;
    public Answer[] answers;
}
[Serializable]
public class Answer
{
    public string id;
    public string text;
    public bool isCorrect;
}