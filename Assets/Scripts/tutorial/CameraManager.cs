using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform m_Target;               // El objetivo que la c�mara seguir� (en este caso, el tanque).
    public float m_FollowSpeed = 5f;         // Velocidad de seguimiento de la c�mara.
    public float m_DefaultSize = 5f;         // Tama�o por defecto de la c�mara (zoom).
    public float m_MaxSize = 15f;            // Tama�o m�ximo del zoom.
    public float m_SizeChangeSpeed = 2f;     // Velocidad de cambio del tama�o de la c�mara.

    private Camera m_Camera;                 // Referencia a la c�mara.
    private float m_CurrentSize;             // Tama�o actual de la c�mara.

    private void Start()
    {
        if (m_Camera == null)
            m_Camera = Camera.main;          // Si no se asigna, usaremos la c�mara principal.

        if (m_Target == null)
        {
            Debug.LogError("�El objetivo (Tanque) no est� asignado! Por favor asigna un objetivo.");
            return;
        }

        m_CurrentSize = m_DefaultSize;        // Inicializamos el tama�o de la c�mara.
        m_Camera.orthographicSize = m_CurrentSize; // Asignar tama�o inicial de la c�mara.
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
        // La posici�n del tanque (objeto) que queremos seguir.
        Vector3 targetPosition = m_Target.position;

        // Asegurarnos de que la c�mara solo se mueva en el plano XY (sin afectar el eje Z).
        targetPosition.z = transform.position.z;

        // Suavizar el movimiento de la c�mara hacia el objetivo (tanque).
        transform.position = Vector3.Lerp(transform.position, targetPosition, m_FollowSpeed * Time.deltaTime);

        // Mostrar en consola la posici�n del objetivo y de la c�mara (solo para depuraci�n).
        Debug.Log("Posici�n del tanque: " + targetPosition + ", Posici�n de la c�mara: " + transform.position);
    }

    public void SetCameraSize(float size)
    {
        // Cambiar el tama�o de la c�mara de forma suave con l�mites.
        m_CurrentSize = Mathf.Clamp(size, m_DefaultSize, m_MaxSize);
        StartCoroutine(SmoothChangeCameraSize(m_CurrentSize));
    }

    public void ResetCameraSize()
    {
        // Restaurar el tama�o predeterminado de la c�mara.
        SetCameraSize(m_DefaultSize);
    }

    private System.Collections.IEnumerator SmoothChangeCameraSize(float targetSize)
    {
        // Cambiar el tama�o de la c�mara suavemente.
        while (!Mathf.Approximately(m_Camera.orthographicSize, targetSize))
        {
            m_Camera.orthographicSize = Mathf.MoveTowards(m_Camera.orthographicSize, targetSize, m_SizeChangeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
