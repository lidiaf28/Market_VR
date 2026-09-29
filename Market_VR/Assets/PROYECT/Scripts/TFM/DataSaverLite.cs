using System;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSaverLite : MonoBehaviour
{
    [Header("Dropdowns")]
    public TMP_Dropdown desplegableComprar;


    [Header("Sliders")]
    public Slider precio;

    public Slider sliderSeguro;


    private string tiempoInicio;
    private string tiempoFin;

    private void Start()
    {
        // Guardar el tiempo de inicio cuando se inicia la escena
        tiempoInicio = DateTime.Now.ToString("HH:mm:ss");
    }

    public void SaveData()
    {
        Debug.Log("[DataSaver] Guardando datos de usuario...");
       

        // 2️ Inicializar variables a 0

        float precio1 = 0f;

        float seguro = 0f;

        int comprarGeneral = desplegableComprar.value;

        if (comprarGeneral == 1) // SI es Sí, guardas los valores de los productos, si es No se queda todo en 0 (inicializado arriba)
        {

            precio1 = precio.value; // Si es Sí (1), toma el valor del slider
            seguro = sliderSeguro.value;
        }
        // Guardar el tiempo de fin cuando se guarda la data
        tiempoFin = DateTime.Now.ToString("HH:mm:ss");

        // 5 Crear CSV
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(
            "Precio;SliderSeguro;TiempoInicio;TiempoFin" //encabezados de las columnas
        );

        sb.AppendLine(
            $"{precio1};{seguro};{tiempoInicio};{tiempoFin}" //guardamos cada valor separado por comas en la fila correspondiente
        );

        // 6 Guardar datos en carpeta "Data"

        string folderPath = Path.Combine(Application.persistentDataPath, "Data"); // esto lo guarda en local en el pc, no en el proyecto, por eso no se ve y da error
        //string folderPath = Path.Combine(Application.dataPath,"PROYECT","Data"); //editor / proyecto unity

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string timestampArchivo = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        // Combinamos el ID de usuario y el timestamp
        string fileName = $"{timestampArchivo}_Data.csv";
        string filePath = Path.Combine(folderPath, fileName);

        File.WriteAllText(filePath, sb.ToString());

        Debug.Log("Datos del usuario guardados correctamente en: " + filePath);
    }
}

