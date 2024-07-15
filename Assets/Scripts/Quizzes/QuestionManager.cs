using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Transform answersParent;
    [SerializeField] private GameObject answerPrefab;
    private GameObject currentSelectedAnswer;
    public void SetQuestion(Question question, int index)
    {
        questionText.text = question.text;
        foreach (Answer answer in question.answers)
        {
            GameObject instantiated = Instantiate(answerPrefab, answersParent);
            instantiated.name = question.text;
            instantiated.GetComponent<AnswerManager>().SetAnswer(answer);
            instantiated.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (currentSelectedAnswer != null)
                {
                    currentSelectedAnswer.GetComponent<Image>().color = Color.white;
                }
                instantiated.GetComponent<Image>().color = new Color(121f / 255f, 130f / 255f, 207f / 255f);
                currentSelectedAnswer = instantiated;
                if (answer.isCorrect)
                {
                    QuizManager.Instance.AnswerQuestion(index, answer.isCorrect);
                }
            });
        }
    }
}
