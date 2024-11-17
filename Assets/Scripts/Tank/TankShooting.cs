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
            // El slider debe tener un valor por defecto de la fuerza mínima de lanzamiento.
            m_AimSlider.value = m_MinLaunchForce;

            // Si se ha excedido la fuerza máxima y el proyectil aún no ha sido disparado...
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                // Usa la fuerza máxima y dispara el proyectil.
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }
            // Si el botón de disparo acaba de empezar a ser presionado...
            else if (Input.GetButtonDown(m_FireButton))
            {
                // Resetea la bandera de disparo y la fuerza de lanzamiento.
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                // Cambia el clip al de carga y empieza a reproducirlo.
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
            }
            // Si el botón de disparo está siendo mantenido y el proyectil aún no ha sido disparado...
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                // Incrementa la fuerza de lanzamiento y actualiza el slider.
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

                m_AimSlider.value = m_CurrentLaunchForce;
            }
            // Si el botón de disparo se suelta y el proyectil aún no ha sido disparado...
            else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
            {
                // Lanza el proyectil.
                Fire();
            }
        }
        else
        { 
            // Calcular la posición futura del objetivo (predicción)
            Vector3 predictedPosition = PredictTargetPosition();

            // Asegúrate de que el proyectil se dispare solo cuando esté dentro de un rango adecuado
            Vector3 direction = predictedPosition - this.transform.position;
            if (direction.magnitude <= 25f)
            {
                fireTimer += Time.deltaTime;

                // Si ha pasado el tiempo suficiente para disparar de nuevo, ejecuta el disparo
                if (fireTimer >= fireCooldown)
                {
                    Fire();            // Dispara
                    fireTimer = 0f;    // Reinicia el temporizador
                }
            }
        }
    }

    private void Fire()
    {
        // Marca que el proyectil ha sido disparado.
        m_Fired = true;

        // Crea una instancia del proyectil y guarda una referencia a su rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Establece la velocidad del proyectil para que se mueva hacia la posición predicha.
        Vector3 direction = goal.position - m_FireTransform.position;
        shellInstance.velocity = m_CurrentLaunchForce * direction.normalized;

        // Cambia el clip a la reproducción del disparo y empieza a reproducirlo.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        // Resetear la fuerza de lanzamiento como precaución.
        m_CurrentLaunchForce = m_MinLaunchForce;
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
