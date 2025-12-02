using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;



public class ShowPanel : MonoBehaviour
{
    [Header("Panel de informacion")]
    public GameObject infoPanel;  // Asigna en el inspector
    NeonEventSender neon;
    public string eventName; // Nombre del evento a enviar al Neon
    public Transform userCamera;

    [Header("Movimiento de objeto")]
    public float moveSpeed = 1f;             // Velocidad de acercamiento
    public float moveDistance = 0.7f;     // Lo que debe moverse hacia la cámara

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private bool isOpen = false;



    void Start()
    {
        // Obtiene el componente interactable del mismo objeto
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        // Se suscribe al evento cuando el usuario �selecciona� (clic o trigger)
        if (interactable != null)
            interactable.selectEntered.AddListener(OnSelect);

        if (neon == null)
            neon = FindObjectOfType<NeonEventSender>(); //Buscamos el NeonEventSender en la escena

        startPosition = transform.position;

        // Obtener cámara del usuario
        if (Camera.main != null)
            userCamera = Camera.main.transform;

        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        // Por limpieza, eliminar listener cuando se destruya
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnSelect);
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        neon.SendNewEvent(eventName + "_Open");
        if (isOpen) return; // ya está abierto

        // Calcula la posición a la que debe acercarse
        Vector3 direction = (userCamera.position - transform.position).normalized;
        targetPosition = startPosition + direction * moveDistance;

        StopAllCoroutines();
        StartCoroutine(MoveTo(targetPosition));

        isOpen = true;
    }

    public void ClosePanel()   // LLAMA ESTO DESDE EL BOTÓN DE CERRAR
    {
        if (!isOpen) return;

        StopAllCoroutines();
        StartCoroutine(MoveTo(startPosition));

        infoPanel.SetActive(false);
        isOpen = false;

    }
    // ---- CORUTINA DE MOVIMIENTO ----
    private System.Collections.IEnumerator MoveTo(Vector3 endPos)
    {
        while (Vector3.Distance(transform.position, endPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, endPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = endPos;
        infoPanel.SetActive(true);
    }
}
