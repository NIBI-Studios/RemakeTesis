using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;
    public void SetAnswer(Answer answer)
    {
        answerText.text = answer.text;
    }
}
