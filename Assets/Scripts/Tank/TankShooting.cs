using System;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Usado para identificar los diferentes jugadores.
    public Rigidbody m_Shell;                   // Prefab de la shell.
    public Transform m_FireTransform;           // Un hijo del tanque donde se generan las shells.
    public Slider m_AimSlider;                  // Un hijo del tanque que muestra la fuerza de lanzamiento actual.
    public AudioSource m_ShootingAudio;         // Referencia al audio usado para reproducir el audio de disparo.
    public AudioClip m_ChargingClip;            // Audio que se reproduce cuando el disparo se está cargando.
    public AudioClip m_FireClip;                // Audio que se reproduce cuando el disparo es realizado.
    public float m_MinLaunchForce = 50f;        // La fuerza dada al proyectil si el botón de disparo no está presionado.
    public float m_MaxLaunchForce = 80f;        // La fuerza dada al proyectil si el botón de disparo está presionado durante el tiempo máximo.
    public float m_MaxChargeTime = 0.75f;       // El tiempo que el proyectil puede cargarse antes de ser disparado con la máxima fuerza.

    private string m_FireButton;                // El botón de entrada usado para lanzar shells.
    private float m_CurrentLaunchForce;         // La fuerza que se dará al proyectil cuando se libere el botón de disparo.
    private float m_ChargeSpeed;                // Qué tan rápido aumenta la fuerza de lanzamiento, basado en el tiempo máximo de carga.
    private bool m_Fired;                       // Si el proyectil ya ha sido lanzado con esta presión de botón.

    public Transform goal;                     // El objetivo a seguir (por ejemplo, otro tanque).
    private float fireCooldown = 3f; 
    private float fireTimer = 0f;

    // Variables para el cálculo de la predicción.
    public float predictionTime = 50f;           // El tiempo en el futuro que se predice la posición del objetivo.
    public float targetSpeed = 10f;              // La velocidad del objetivo (puede ser ajustable según el juego).

    // datos extra para el disparo
    public int currentBullets = 10;       // Número inicial de balas.
    public int maxBullets = 20;           // Capacidad máxima de balas.
    private string bulletType = "Normal";  // Tipo actual de balas.
    private void OnEnable()
    {
        // Cuando el tanque se enciende, resetea la fuerza de lanzamiento y la UI
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }

    private void Start()
    {
        // El eje de disparo se basa en el número de jugador.
        m_FireButton = "Fire" + m_PlayerNumber;

        // La velocidad de carga es el rango de las fuerzas posibles dividido por el tiempo máximo de carga.
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
        if (m_PlayerNumber <= 2)
        {
            // Lógica de disparo manual para jugadores controlados
            m_AimSlider.value = m_MinLaunchForce;

            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired && currentBullets > 0)
            {
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }
            else if (Input.GetButtonDown(m_FireButton))
            {
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
            }
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            else if (Input.GetButtonUp(m_FireButton) && !m_Fired && currentBullets > 0)
            {
                Fire();
            }
        }
        else
        {
            // Lógica para tanques controlados por IA
            if (goal != null)
            {
                Vector3 direction = goal.position - this.transform.position;

                if (direction.magnitude <= 25f)
                {
                    fireTimer += Time.deltaTime;

                    if (fireTimer >= fireCooldown && currentBullets > 0)
                    {
                        Fire();
                        fireTimer = 0f;
                    }
                }
            }
        }
    }


    private void Fire()
    {

        if (currentBullets <= 0)
        {
            Debug.Log("Sin balas para disparar.");
            return;
        }

        // Set the fired flag so only Fire is only called once.
        m_Fired = true;

        // Create an instance of the shell and store a reference to its rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        if (goal != null && m_PlayerNumber > 2)
        {
            Vector3 predictiveDirection = (goal.position - this.transform.position).normalized;
            shellInstance.velocity = m_CurrentLaunchForce * predictiveDirection;
            Debug.Log("llego: ");
        }
        else
        {
            // Disparo directo sin predicción
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
        }

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        // Reset the launch force. This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_MaxLaunchForce;

        currentBullets--;

    }
    public void AddBullets(int amount)
    {
        currentBullets = Mathf.Clamp(currentBullets + amount, 0, maxBullets);
    }

    public void SetMaxBullets(int newMax)
    {
        maxBullets = newMax;
        currentBullets = Mathf.Clamp(currentBullets, 0, maxBullets);
    }

    public int GetCurrentBullets()
    {
        return currentBullets;
    }

    public int GetMaxBullets()
    {
        return maxBullets;
    }

    public string GetBulletType()
    {
        return bulletType;
    }

    public void SetBulletType(string newType)
    {
        bulletType = newType;
    }

    // Método para predecir la posición futura del objetivo basado en su velocidad y dirección.
    private Vector3 PredictTargetPosition()
    {
        Vector3 targetVelocity = goal.GetComponent<Rigidbody>().velocity;  // Obtener la velocidad del objetivo (tanque enemigo).
        Vector3 targetDirection = targetVelocity.normalized;               // Dirección de la velocidad.

        // Calcula la posición futura del objetivo considerando su velocidad y un tiempo predicho.
        Vector3 predictedPosition = goal.position + targetVelocity * predictionTime;

        return predictedPosition;
    }
}
