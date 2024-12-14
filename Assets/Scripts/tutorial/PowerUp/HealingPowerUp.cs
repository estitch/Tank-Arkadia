using UnityEngine;

public class HealingPowerUp : PowerUpBase
{
    [Header("Healing Settings")]
    public float healAmount = 20f;

    [Header("Quiz Manager")]
    public GameObject quizPanel; // Panel de preguntas
    private TankHealth currentTankHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tank")) return; // Solo interactúa con tanques

        TankHealth tankHealth = other.GetComponent<TankHealth>();

        if (tankHealth != null)
        {
            currentTankHealth = tankHealth;

            if (quizPanel != null)
            {
                quizPanel.SetActive(true);

                QuizManager quizManager = quizPanel.GetComponent<QuizManager>();
                QuizLoader quizLoader = quizPanel.GetComponent<QuizLoader>();

                if (quizManager != null && quizLoader != null)
                {
                    quizManager.OnQuestionAnswered -= HandleQuizResult; // Evita duplicación de eventos
                    quizManager.OnQuestionAnswered += HandleQuizResult;
                    quizLoader.LoadRandomQuestion();
                }
                else
                {
                    Debug.LogWarning("QuizManager o QuizLoader no están asignados.");
                }
            }
            else
            {
                Debug.LogWarning("El panel de preguntas no está asignado.");
            }
        }
    }

    private void HandleQuizResult(bool isCorrect)
    {
        if (isCorrect && currentTankHealth != null)
        {
            ApplyEffect(currentTankHealth, null, null);
        }

        QuizManager quizManager = quizPanel.GetComponent<QuizManager>();
        if (quizManager != null)
        {
            quizManager.OnQuestionAnswered -= HandleQuizResult;
        }

        // Desactiva el panel y el power-up
        quizPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    protected override void ApplyEffect(TankHealth health, TankMovement movement, TankShooting shooting)
    {
        if (health != null)
        {
            health.Heal(healAmount);
            Debug.Log($"Healed for {healAmount}. Current Health: {health.GetCurrentHealth()}");
        }
    }
}
