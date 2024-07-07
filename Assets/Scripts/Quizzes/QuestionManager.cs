using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Transform answersParent;
    [SerializeField] private GameObject answerPrefab;
    public void SetQuestion(Question question)
    {
        questionText.text = question.text;
        foreach (Answer answer in question.answers)
        {
            GameObject instantiated = Instantiate(answerPrefab, answersParent);
            instantiated.name = question.text;
            instantiated.GetComponent<AnswerManager>().SetAnswer(answer);
            instantiated.GetComponent<Button>().onClick.AddListener(() =>
            {
                instantiated.GetComponent<Image>().color = new Color(121, 130, 207);
                if (answer.isCorrect)
                {

                }
            });
        }
    }
}
