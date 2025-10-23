using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Importa el Recorder API de Unity (solo disponible en el Editor, por eso está dentro del #if
#if UNITY_EDITOR
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
#endif
using System.IO;

public class SceneRecording : MonoBehaviour
{
#if UNITY_EDITOR
    private RecorderController recorderController;
#endif

    [Header("Cámara XR")]
    public Camera cameraToRecord;
    public bool grabRightEye = false; // false = centrado, true = ojo derecho
    public float ipd = 0.067f; // distancia interpupilar

    private Vector3 originalLocalPos;

    void Start()
    {
        if (cameraToRecord == null)
            cameraToRecord = Camera.main;
    }

    public void StartRecording()
    {
#if UNITY_EDITOR
        if (cameraToRecord == null)
        {
            Debug.LogError("❌ No hay cámara asignada.");
            return;
        }

        // Guardamos posición original
        originalLocalPos = cameraToRecord.transform.localPosition;

        // Aplicamos offset temporal

        if (grabRightEye)
            cameraToRecord.transform.localPosition += new Vector3(ipd, 0f, 0f);   // ojo derecho
        else
            cameraToRecord.transform.localPosition += new Vector3(ipd / 2f, 0f, 0f); // centrado

        Debug.Log($"🎥 Iniciando grabación del GameView (offset aplicado: {cameraToRecord.transform.localPosition.x - originalLocalPos.x:F4} m)");


        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        recorderController = new RecorderController(controllerSettings);

        var movieRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        movieRecorder.name = "GameViewRecorder";
        movieRecorder.Enabled = true;
        movieRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;

        // Carpeta de salida
        string folderPath = Path.Combine(Application.dataPath, "Recordings");
        Directory.CreateDirectory(folderPath);
        movieRecorder.OutputFile = Path.Combine(folderPath,
            "recording_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss_Unity") + "_Unity");

        // Configuración de entrada: captura GameView
        var gameViewInput = new GameViewInputSettings
        {
            OutputWidth = 1600, //1920
            OutputHeight = 1200 //1080
        };
        movieRecorder.ImageInputSettings = gameViewInput;

        controllerSettings.AddRecorderSettings(movieRecorder);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 20f;
        controllerSettings.CapFrameRate = true;

        recorderController.PrepareRecording();
        recorderController.StartRecording();

        Debug.Log("✅ Grabación de Unity iniciada.");
#else
        Debug.LogWarning("Solo funciona en el Editor de Unity.");
#endif
    }

    public void StopRecording()
    {
#if UNITY_EDITOR
        if (recorderController != null && recorderController.IsRecording())
        {
            recorderController.StopRecording();
            Debug.Log("🛑 Grabación detenida.");
        }
        // Restauramos posición original
        if (cameraToRecord != null)
        {
            cameraToRecord.transform.localPosition = originalLocalPos;
            Debug.Log("🔁 Posición de cámara restaurada.");
        }
#endif
    }

    private void OnApplicationQuit()
    {
        StopRecording();
    }
}

