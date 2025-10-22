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
    [Header("Rotación manual")]
    public float additionalRotationY = 0f; // Rotación extra sobre Y para que se gire más
    public float additionalRotationX = 0f;
    public float additionalRotationZ = 0f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private bool hasMoved = false;
    private Vector3 targetPosition; //Representa dónde queremos que se desplace el objeto para que quede frente al usuario.
    private bool isMoving = false;
    //Guarda la posición y rotación inicial del objeto en la escena.
    private Vector3 originalPosition;
    private Quaternion originalRotation;



    void Start()
    {
        // Obtiene el componente interactable del mismo objeto
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        // Se suscribe al evento cuando el usuario �selecciona� (clic o trigger)
        if (interactable != null)
            interactable.selectEntered.AddListener(OnSelect);
        // Guardamos la posición y rotación original
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
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + additionalRotationY, 0);
        transform.rotation = Quaternion.Euler(additionalRotationX, additionalRotationY, additionalRotationZ);

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
    public void ReturnToOriginalPosition() //llamar desde boton cerrar de la ventana
    {
        StopAllCoroutines(); // Por si el objeto estaba moviéndose
        StartCoroutine(MoveBackCoroutine());
    }

    private IEnumerator MoveBackCoroutine()
    {
        float distanceThreshold = 0.01f;
        while (Vector3.Distance(transform.position, originalPosition) > distanceThreshold)
        {
            // Mueve suavemente hacia la posición original
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Aseguramos que queda exacto al final
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Reiniciamos flag para poder seleccionarlo otra vez
        hasMoved = false;

        // Opcional: ocultar panel al volver
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }
}
