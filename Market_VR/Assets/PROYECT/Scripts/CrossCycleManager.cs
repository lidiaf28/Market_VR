using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ReadyPlayerMe.Core.Analytics.Constants;
using static Unity.Burst.Intrinsics.Arm;

public class CrossCycleManager : MonoBehaviour
{
    [Header("Cruces individuales")]
    public GameObject cross1;
    public GameObject cross2;
    public GameObject cross3;
    public GameObject cross4;
    public GameObject cross5;

    [Header("Tiempo entre cruces (segundos)")]
    public float changeInterval = 5f;

    public GameObject siguiente;

    private GameObject[] crosses;
    private int currentIndex = 0;
    private bool isCycling = false;
    NeonEventSender neon;

    void Start()
    {
        // Crea el array automáticamente con las referencias asignadas
        crosses = new GameObject[] { cross1, cross2, cross3, cross4, cross5 };
        neon = FindObjectOfType<NeonEventSender>(); //Buscamos el NeonEventSender en la escena
    }

    // Llamar desde el botón "Comenzar"
    public void StartCycle()
    {
        neon.SendNewEvent("X1_START"); //Envía evento START al Neon
        if (!isCycling)
        {
            isCycling = true;
            StartCoroutine(CycleCrosses());
        }
    }

    private IEnumerator CycleCrosses()
    {
        while (isCycling && currentIndex < crosses.Length - 1)
        {
            yield return new WaitForSeconds(changeInterval);

            // Desactiva la cruz actual
            if (crosses[currentIndex] != null)
            {
                crosses[currentIndex].SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Cruz en índice {currentIndex} es nula. Terminando el ciclo.");
                yield break; // Salir si hay un hueco vacío en medio del array
            }
            // Avanza al siguiente índice
            currentIndex++;
            // Activa la siguiente
            if (crosses[currentIndex] != null)
            {
                crosses[currentIndex].SetActive(true);
                // Envía el evento con el número dinámico
                string eventName = $"X{currentIndex + 1}_START";
                neon.SendNewEvent(eventName);
            }     
        }

        // Espera el último intervalo con la cruz final activa antes de terminar
        yield return new WaitForSeconds(changeInterval);

        neon.SendNewEvent("X_STOP"); //Envía evento STOP al Neon
        // Fin del ciclo
        StopCycle();
    }

    private void StopCycle()
    {
        isCycling = false;
        StopAllCoroutines();
        Debug.Log(" Ciclo de cruces finalizado.");
        //desactivar ultima cruz y activar boton
        cross5.SetActive(false);
        siguiente.SetActive(true);
    }
}
