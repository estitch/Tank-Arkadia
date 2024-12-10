using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform m_Target;               // El objetivo que la cámara seguirá (en este caso, el tanque).
    public float m_FollowSpeed = 5f;         // Velocidad de seguimiento de la cámara.
    public float m_DefaultSize = 5f;         // Tamaño por defecto de la cámara (zoom).
    public float m_MaxSize = 15f;            // Tamaño máximo del zoom.
    public float m_SizeChangeSpeed = 2f;     // Velocidad de cambio del tamaño de la cámara.

    private Camera m_Camera;                 // Referencia a la cámara.
    private float m_CurrentSize;             // Tamaño actual de la cámara.

    private void Start()
    {
        if (m_Camera == null)
            m_Camera = Camera.main;          // Si no se asigna, usaremos la cámara principal.

        if (m_Target == null)
        {
            Debug.LogError("¡El objetivo (Tanque) no está asignado! Por favor asigna un objetivo.");
            return;
        }

        m_CurrentSize = m_DefaultSize;        // Inicializamos el tamaño de la cámara.
        m_Camera.orthographicSize = m_CurrentSize; // Asignar tamaño inicial de la cámara.
    }

    private void LateUpdate()
    {
        if (m_Target != null)
        {
            FollowTarget();                  // Seguir al tanque.
        }
    }

    private void FollowTarget()
    {
        // La posición del tanque (objeto) que queremos seguir.
        Vector3 targetPosition = m_Target.position;

        // Asegurarnos de que la cámara solo se mueva en el plano XY (sin afectar el eje Z).
        targetPosition.z = transform.position.z;

        // Suavizar el movimiento de la cámara hacia el objetivo (tanque).
        transform.position = Vector3.Lerp(transform.position, targetPosition, m_FollowSpeed * Time.deltaTime);

        // Mostrar en consola la posición del objetivo y de la cámara (solo para depuración).
        Debug.Log("Posición del tanque: " + targetPosition + ", Posición de la cámara: " + transform.position);
    }

    public void SetCameraSize(float size)
    {
        // Cambiar el tamaño de la cámara de forma suave con límites.
        m_CurrentSize = Mathf.Clamp(size, m_DefaultSize, m_MaxSize);
        StartCoroutine(SmoothChangeCameraSize(m_CurrentSize));
    }

    public void ResetCameraSize()
    {
        // Restaurar el tamaño predeterminado de la cámara.
        SetCameraSize(m_DefaultSize);
    }

    private System.Collections.IEnumerator SmoothChangeCameraSize(float targetSize)
    {
        // Cambiar el tamaño de la cámara suavemente.
        while (!Mathf.Approximately(m_Camera.orthographicSize, targetSize))
        {
            m_Camera.orthographicSize = Mathf.MoveTowards(m_Camera.orthographicSize, targetSize, m_SizeChangeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
