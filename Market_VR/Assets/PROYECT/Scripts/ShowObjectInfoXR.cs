using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowObjectInfoXR : MonoBehaviour
{
    [Header("Panel de informacion")]
    public GameObject infoPanel;  // Asigna en el inspector

    public Transform userCamera;
    public float approachDistance = 0.7f; // Cuánto se acerca el objeto
    public float moveSpeed = 1.5f;        // Velocidad de movimiento hacia el usuario
    public float heightOffset = -0.3f;      // Ajuste vertical (negativo = más bajo)
    public float additionalRotationY = 30f; // Rotación extra sobre Y para que se gire más

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private bool hasMoved = false;
    private Vector3 targetPosition;
    private bool isMoving = false;



    void Start()
    {
        // Obtiene el componente interactable del mismo objeto
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        // Se suscribe al evento cuando el usuario �selecciona� (clic o trigger)
        if (interactable != null)
            interactable.selectEntered.AddListener(OnSelect);
    }

    private void OnDestroy()
    {
        // Por limpieza, eliminar listener cuando se destruya
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnSelect);
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        if (!hasMoved)
        {
            CalculateTargetPosition();
            isMoving = true;
            hasMoved = true;
        }

        if (infoPanel != null)
            infoPanel.SetActive(true);
    }

    private void CalculateTargetPosition()
    {
        if (userCamera == null) return;

        Vector3 directionToUser = (userCamera.position - transform.position).normalized;

        // Ajusta la altura
        targetPosition = transform.position + directionToUser * approachDistance;
        targetPosition.y += heightOffset;

        // Ajustamos la rotación para mirar al usuario
        transform.LookAt(userCamera);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + additionalRotationY, 0);
    }
    void Update()
    {
        if (isMoving)
        {
            // Interpolación hacia la posición objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Cuando llega a la posición final, deja de mover
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                isMoving = false;
        }
    }
}
