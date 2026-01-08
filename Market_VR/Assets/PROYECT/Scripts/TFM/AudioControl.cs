using System.Collections;
using System.Collections.Generic;
using TMPro; // si usas TextMeshPro, si es UI.Text cambia el tipo
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    [Header("Textos y GameObjects de audio en orden")]
    public GameObject[] textosGO;      // Arrastra aquí tus textos
    public GameObject[] audiosGO;    // Arrastra aquí tus GameObjects con AudioSource


    private int indiceActual = 0; // Índice del audio/texto actual
    private AudioSource[] audioSources; // Array interno con los AudioSource reales


    void Start()
    {
        //Debug.Log("textosGO length = " + textosGO.Length);
        //Debug.Log("audiosGO length = " + audiosGO.Length);

        if (textosGO.Length != audiosGO.Length)
        {
            Debug.LogError("[TextAudioSequencer] Los arrays de textos y audios no tienen la misma longitud.");
            return;
        }

        audioSources = new AudioSource[audiosGO.Length];

        // Obtener los AudioSource de cada GameObject
        for (int i = 0; i < audiosGO.Length; i++)
        {
            audioSources[i] = audiosGO[i].GetComponent<AudioSource>();
            if (audioSources[i] == null)
                Debug.LogError($"[TextAudioSequencer] No hay AudioSource en {audiosGO[i].name}");
        }

        //  No tocamos texto 0 ni audio 0 (ya están activos en la escena)
        // Solo desactivamos texto 1,2,3,...
        for (int i = 1; i < textosGO.Length; i++)
            textosGO[i].SetActive(false);

        // Reproducir el primero si no está en playOnAwake
        audioSources[0].Play();
    }

    void Update()
    {

        if (indiceActual >= textosGO.Length) return;

        // Si el audio actual ha terminado...
        if (!audioSources[indiceActual].isPlaying)
        {
            // Pasamos al siguiente
            int siguiente = indiceActual + 1;

            if (siguiente < textosGO.Length)
            {
                textosGO[indiceActual].SetActive(false);  // apagar texto actual
                textosGO[siguiente].SetActive(true);      // encender texto siguiente
                audioSources[siguiente].Play();           // reproducir su audio
            }

            indiceActual = siguiente;
        }
    }
}
