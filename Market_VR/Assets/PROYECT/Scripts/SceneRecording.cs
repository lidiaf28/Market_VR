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

    public Camera cameraToRecord;

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

        Debug.Log("🎥 Iniciando grabación del GameView...");

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
            "recording_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"));

        // Configuración de entrada: captura GameView
        var gameViewInput = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };
        movieRecorder.ImageInputSettings = gameViewInput;

        controllerSettings.AddRecorderSettings(movieRecorder);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 30f;
        controllerSettings.CapFrameRate = true;

        recorderController.PrepareRecording();
        recorderController.StartRecording();

        Debug.Log("✅ Grabación del GameView iniciada.");
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
#endif
    }

    private void OnApplicationQuit()
    {
        StopRecording();
    }
}

