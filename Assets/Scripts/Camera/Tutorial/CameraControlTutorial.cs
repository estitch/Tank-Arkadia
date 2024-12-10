using UnityEngine;

public class CameraControlTutorial : MonoBehaviour
{
    public float m_DampTime = 0.2f;                // Tiempo de suavizado para el movimiento de la cámara.
    public float m_ScreenEdgeBuffer = 4f;           // Buffer para evitar que los objetos estén demasiado cerca del borde de la cámara.
    public float m_MinSize = 11f;                  // Tamaño mínimo del zoom de la cámara.

    [SerializeField] private Transform[] m_Targets; // Los objetos que la cámara debe seguir. Se asignan en el Inspector.

    private Camera m_Camera;                        // La cámara que se controlará.
    private float m_ZoomSpeed;                      // Velocidad para el cambio de tamaño.
    private Vector3 m_MoveVelocity;                 // Velocidad de movimiento de la cámara.
    private Vector3 m_DesiredPosition;              // La posición que la cámara desea alcanzar.

    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();  // Asignar la cámara si no está asignada.
    }

    private void FixedUpdate()
    {
        if (m_Targets != null && m_Targets.Length > 0)
        {
            Move();    // Mover la cámara.
            Zoom();    // Ajustar el zoom de la cámara.
        }
    }

    // Mover la cámara suavemente hacia la posición promedio de los objetivos.
    private void Move()
    {
        FindAveragePosition();  // Calcular la posición promedio de los objetivos.
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);  // Mover la cámara suavemente.
    }

    // Calcular la posición promedio de todos los objetivos activos.
    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Recorremos todos los objetivos activos y calculamos la posición promedio.
        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (m_Targets[i] != null && m_Targets[i].gameObject.activeSelf)
            {
                averagePos += m_Targets[i].position;
                numTargets++;
            }
        }

        // Si hay objetivos activos, calcular la posición promedio.
        if (numTargets > 0)
            averagePos /= numTargets;

        // Mantener la misma altura en Y para evitar movimiento vertical innecesario.
        averagePos.y = transform.position.y;

        m_DesiredPosition = averagePos;
    }

    // Ajustar el zoom de la cámara en función de la distancia a los objetivos.
    private void Zoom()
    {
        float requiredSize = FindRequiredSize();  // Calcular el tamaño requerido para la cámara.
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);  // Cambiar el tamaño suavemente.
    }

    // Calcular el tamaño necesario para mostrar todos los objetivos en pantalla.
    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);  // Convertir la posición deseada a espacio local.

        float size = 0f;

        // Calcular el tamaño necesario para encajar todos los objetivos visibles.
        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (m_Targets[i] != null && m_Targets[i].gameObject.activeSelf)
            {
                Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);  // Convertir posición del objetivo a local.

                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;  // Calcular la distancia del objetivo a la posición deseada.

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));  // Asegurarse de que se vea toda la altura de los objetivos.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);  // Asegurarse de que se vea toda la anchura de los objetivos.
            }
        }

        // Añadir buffer para que los objetos no queden demasiado cerca de los bordes.
        size += m_ScreenEdgeBuffer;

        // Asegurarse de que el tamaño no sea menor que el tamaño mínimo.
        size = Mathf.Max(size, m_MinSize);

        return size;
    }

    // Establecer la posición y el tamaño inicial de la cámara.
    public void SetStartPositionAndSize()
    {
        FindAveragePosition();  // Calcular la posición inicial.
        transform.position = m_DesiredPosition;  // Establecer la posición de la cámara.
        m_Camera.orthographicSize = FindRequiredSize();  // Establecer el tamaño inicial de la cámara.
    }
}
