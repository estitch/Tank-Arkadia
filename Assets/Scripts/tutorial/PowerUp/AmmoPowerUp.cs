using UnityEngine;

public class AmmoPowerUp : PowerUpBase
{
    [Header("Ammo Settings")]
    public int ammoIncrease = 5;
    [Header("Quiz Manager")]
    public GameObject quizPanel; // Panel de preguntas
    private TankHealth currentTankHealth;

    private void OnTriggerEnter(Collider other)
    {
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
        if (shooting != null)
        {
            shooting.AddMaxBullets(ammoIncrease);
            Debug.Log($"Ammo max increased by {ammoIncrease}. Current Ammo: {shooting.GetCurrentBullets()}");

            //// Actualiza solo la sección de velocidad del HUD.
            //if (hudManager != null)
            //{
            //    hudManager.UpdateShooting(shooting);
            //}
        }
    }
}
