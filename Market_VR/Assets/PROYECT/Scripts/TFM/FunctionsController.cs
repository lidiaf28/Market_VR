using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para manejar escenas

public class FunctionsController : MonoBehaviour
{
    // Nombre de la escena a cargar (puedes configurarlo desde el inspector)
    public string sceneName;

    // Esta función puedes enlazarla al botón desde el inspector
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

}