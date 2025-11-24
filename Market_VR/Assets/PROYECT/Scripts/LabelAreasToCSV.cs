using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class LabelAreasToCSV : MonoBehaviour
{
    [Header("Canvas World Space")]
    public Canvas miCanvas;

    [Header("Puntos Ventana")]
    public Transform ventanaP1;
    public Transform ventanaP2;

    [Header("Punto Central Ventana")]
    public Transform ventanaCentro;

    [Header("Puntos Etiqueta")]
    public Transform etiquetaP1;
    public Transform etiquetaP2;

    [Header("Puntos Logo")]
    public Transform logoP1;
    public Transform logoP2;

    [Header("Puntos Info Nutricional")]
    public Transform infoNutriP1;
    public Transform infoNutriP2;

    [Header("Puntos PlanetScore")]
    public Transform planetScoreP1;
    public Transform planetScoreP2;

    private string fileName = "AreasLabels.csv";

    [ContextMenu("Generar CSV")]//opcion para generar el csv al pulsar, en ejecucion


    public void GenerarCSV()
    {
        string filePath = Path.Combine(Application.dataPath, "PROYECT", "scripts", fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Cabeceras
            writer.WriteLine("Area;P1;P2");

            // Escribir filas
            EscribirLinea(writer, "Ventana", ventanaP1, ventanaP2);
            EscribirLinea(writer, "Etiqueta", etiquetaP1, etiquetaP2);
            EscribirLinea(writer, "Logo", logoP1, logoP2);
            EscribirLinea(writer, "InfoNutricional", infoNutriP1, infoNutriP2);
            EscribirLinea(writer, "PlanetScore", planetScoreP1, planetScoreP2);
            EscribirLineaPunto(writer, "VentanaCentro", ventanaCentro);
        }

        Debug.Log("CSV generado en: " + filePath);
    }
    private void EscribirLineaPunto(StreamWriter writer, string area, Transform p)
    {
        Vector3 localP = p.localPosition;
        string posStr = $"({localP.x.ToString(CultureInfo.InvariantCulture)};" +
                        $"{localP.y.ToString(CultureInfo.InvariantCulture)})";

        writer.WriteLine($"{area};{posStr}");
    }

    private void EscribirLinea( StreamWriter writer, string area, Transform p1, Transform p2)
    {
        Vector3 localP1 = p1.localPosition;
        Vector3 localP2 = p2.localPosition;

        string p1Str = $"({localP1.x.ToString(CultureInfo.InvariantCulture)};" +
                       $"{localP1.y.ToString(CultureInfo.InvariantCulture)})";

        string p2Str = $"({localP2.x.ToString(CultureInfo.InvariantCulture)};" +
                       $"{localP2.y.ToString(CultureInfo.InvariantCulture)})";

        writer.WriteLine($"{area};{p1Str};{p2Str}");
    }
}