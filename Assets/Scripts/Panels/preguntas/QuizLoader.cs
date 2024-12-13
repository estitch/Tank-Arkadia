using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionData
{
    public string question;
    public string[] options;
    public string answer;
}

[System.Serializable]
public class QuestionDatabase
{
    public List<QuestionData> questions;
}

public class QuizLoader : MonoBehaviour
{
    public TextAsset questionsJson;
    public QuizManager quizManager;

    private QuestionDatabase questionDatabase;

    private void Awake()
    {
        LoadQuestionsFromJson();
    }

    private void LoadQuestionsFromJson()
    {
        if (questionsJson != null)
        {
            questionDatabase = JsonUtility.FromJson<QuestionDatabase>(questionsJson.text);
            Debug.Log("data cargada");
        }
        else
        {
            Debug.LogError("Archivo JSON de preguntas no asignado.");
        }
    }

    public void LoadRandomQuestion()
    {
        if (questionDatabase != null && questionDatabase.questions.Count > 0)
        {
            int randomIndex = Random.Range(0, questionDatabase.questions.Count);
            QuestionData selectedQuestion = questionDatabase.questions[randomIndex];
            quizManager.DisplayQuestion(selectedQuestion.question, selectedQuestion.options, selectedQuestion.answer);
        }
        else
        {
            Debug.LogError("No hay preguntas disponibles.");
        }
    }
}
