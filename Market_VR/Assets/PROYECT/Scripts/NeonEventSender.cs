using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

//El script envia un JSON con un evento {"action":"START","id":"recording_sync","message":""}

public class NeonEventSender : MonoBehaviour
{
    public string neonIP = "10.68.60.135"; //IP movil s25
    public int port = 8080;

    //START y STOP son los eventos que se enviarán al Neon (nombre)

    //Cuando llamas SendStartRecording(), Unity envía un evento HTTP POST al Neon diciendo: “empieza a registrar”
    public void SendStartRecording()
    {
        StartCoroutine(SendEvent("START"));
    }

    //Cuando llamas SendStopRecording(), envía otro diciendo: “termina de registrar”.
    public void SendStopRecording()
    {
        StartCoroutine(SendEvent("STOP"));
    }
    //Los eventos quedan en eye_events.json, con timestamps


    private IEnumerator SendEvent(string action)
    {
        string url = $"http://{neonIP}:{port}/api/event";

        // Cuerpo del evento en JSON
        //Neon Companion no espera un formulario, espera JSON con los campos name y timestamp
        // Genera timestamp en nanosegundos (lo más parecido que podemos desde C#)
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1_000_000;
        var payload = new NeonSimpleEvent(action, timestamp);
        string json = JsonUtility.ToJson(new NeonSimpleEvent(action, timestamp)); //convierte el objeto NeonEvent en un string JSON
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json); //Convierte ese texto JSON en una secuencia de bytes,
                                                       //porque las conexiones HTTP no envían texto directamente: envían datos binarios.
        
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            //usamos UploadHandlerRaw y le ponemos Content-Type: application/json
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);//envia json al servidor
            request.downloadHandler = new DownloadHandlerBuffer();//permite recibir la respuesta
            request.SetRequestHeader("Content-Type", "application/json");//Esto envía el JSON exactamente como Neon lo necesita

            yield return request.SendWebRequest(); //pausa la corutina hasta que Neon responda

            string responseText = request.downloadHandler?.text ?? "";

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log($"Evento {action} enviado correctamente: {responseText}");
            else
                Debug.LogError($"Error enviando evento {action}: {request.error}\nRespuesta: {responseText}");
        }
    }

    [System.Serializable]
    private class NeonSimpleEvent
    {
        public string name;
        public long timestamp;

        public NeonSimpleEvent(string name, long timestamp)
        {
            this.name = name;
            this.timestamp = timestamp;
        }
    }
}