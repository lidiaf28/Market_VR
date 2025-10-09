using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowObjectInfoXR : MonoBehaviour
{
    [Header("Panel de informacion")]
    public GameObject infoPanel;  // Asigna en el inspector

    public Transform userCamera;        // Cámara del usuario
    public float approachDistance = 0.7f; // Cuánto se acerca el objeto hacia el usuario

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool hasMoved = false;


    void Start()
    {
        // Obtiene el componente interactable del mismo objeto
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        // Se suscribe al evento cuando el usuario �selecciona� (clic o trigger)
        if (interactable != null)
            interactable.selectEntered.AddListener(OnSelect);

        originalPosition = transform.position;
        originalRotation = transform.rotation;
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
            MoveSlightlyTowardUser();
            hasMoved = true;
        }

        if (infoPanel != null)
            infoPanel.SetActive(true);
    }
    private void MoveSlightlyTowardUser()
    {
        if (userCamera == null) return;

        // Dirección desde el objeto hacia el usuario
        Vector3 directionToUser = (userCamera.position - transform.position).normalized;

        // Calcula la nueva posición
        Vector3 targetPosition = transform.position + directionToUser * approachDistance;

        // Mueve el objeto
        transform.position = targetPosition;

        // Que mire hacia el usuario
        transform.LookAt(userCamera);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
