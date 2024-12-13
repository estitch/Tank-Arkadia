using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI questionText; // Muestra la pregunta
    public List<Button> answerButtons;  // Botones para respuestas
    public TextMeshProUGUI resultText; // Muestra "Correcto" o "Incorrecto"

    public delegate void QuestionAnsweredHandler(bool isCorrect);
    public event QuestionAnsweredHandler OnQuestionAnswered;

    private bool isPaused;

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Detiene el tiempo del juego
        gameObject.SetActive(true); // Asegura que el panel esté activo
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        gameObject.SetActive(false); // Cierra el panel
    }

    public void DisplayQuestion(string question, string[] options, string correctAnswer)
    {
        PauseGame(); // Pausa el juego cuando se muestra una nueva pregunta

        questionText.text = question;

        for (int i = 0; i < answerButtons.Count; i++)
        {
            if (i < options.Length)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
                answerButtons[i].gameObject.SetActive(true);

                int localIndex = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(options[localIndex], correctAnswer));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        resultText.text = ""; // Limpia mensajes previos
    }

    private void CheckAnswer(string selectedOption, string correctAnswer)
    {
        bool isCorrect = selectedOption == correctAnswer;

        resultText.text = isCorrect ? "Correcto" : "Incorrecto";

        OnQuestionAnswered?.Invoke(isCorrect);

        Invoke(nameof(ResumeGame), 0f); // Espera 1 segundo antes de reanudar el juego
    }
}
