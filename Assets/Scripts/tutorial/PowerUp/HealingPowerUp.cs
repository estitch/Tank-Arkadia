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
        Debug.Log("power up salud");
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
                    quizManager.OnQuestionAnswered += HandleQuizResult;
                    quizLoader.LoadRandomQuestion();
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

        // Ocultar o desactivar el power-up después de responder la pregunta
        gameObject.SetActive(false); // Desactiva el objeto del power-up
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
