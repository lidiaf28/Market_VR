using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private GameObject[] crosses;
    private int currentIndex = 0;
    private bool isCycling = false;

    void Start()
    {
        // Crea el array automáticamente con las referencias asignadas
        crosses = new GameObject[] { cross1, cross2, cross3, cross4, cross5 };
    }

    // Llamar desde el botón "Comenzar"
    public void StartCycle()
    {
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
                crosses[currentIndex].SetActive(false);

            // Activa la siguiente
            currentIndex++;
            if (crosses[currentIndex] != null)
                crosses[currentIndex].SetActive(true);
        }

        // Espera el último intervalo con la cruz final activa antes de terminar
        yield return new WaitForSeconds(changeInterval);

        // Fin del ciclo
        StopCycle();
    }

    private void StopCycle()
    {
        isCycling = false;
        StopAllCoroutines();
        Debug.Log(" Ciclo de cruces finalizado.");
    }
}
