using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para manejar escenas
using UnityEngine.Video;
using UnityEngine.UI;
public class FunctionsController : MonoBehaviour
{
    // Nombre de la escena a cargar (puedes configurarlo desde el inspector)
    public string sceneName;
    public TMP_Text textoAsociado;
    [Header("Video terminado")]
    public VideoPlayer VideoCocinero;
    public Button ActivarTrasVideo;

    // Esta función puedes enlazarla al botón desde el inspector
    void Start()
    {
        // Por si acaso, desactivamos el botón al inicio
        ActivarTrasVideo.interactable = false;

        // Nos suscribimos al evento de fin de vídeo
        VideoCocinero.loopPointReached += OnVideoFinished;
    }


    void OnVideoFinished(VideoPlayer vp)
    {
        ActivarTrasVideo.interactable = true;
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un nombre de escena en el script ChangeScene.");
        }
    }
    public void SliderValueToText(float value)  // Llamar desde On Value Changed (float)
    {

        textoAsociado.text = value.ToString("0.00");
    }


}