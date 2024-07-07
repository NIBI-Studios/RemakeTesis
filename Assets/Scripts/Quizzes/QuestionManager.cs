using System;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    public void SetQuestion(Question question)
    {
        questionText.text = question.text;
    }
}
