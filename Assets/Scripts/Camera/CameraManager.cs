using UnityEngine;

public class CameraManagers : MonoBehaviour
{
    public Camera mainCamera;  // La cámara principal
    public Transform defaultCameraPosition;  // Posición alternativa de la cámara (por ejemplo, una vista superior)
    private Transform currentTarget;  // El objetivo actual de la cámara

    private void Update()
    {
        if (currentTarget == null)
        {
            // Si no hay objetivo, mueve la cámara a la posición predeterminada
            mainCamera.transform.position = defaultCameraPosition.position;
            mainCamera.transform.rotation = defaultCameraPosition.rotation;
        }
    }

    public void SetCameraTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    // Llamar a este método cuando el tanque especial muere
    public void OnSpecialTankDeath()
    {
        // Desvincula la cámara del tanque especial
        SetCameraTarget(null);  // La cámara ya no sigue al tanque especial.
    }
}
