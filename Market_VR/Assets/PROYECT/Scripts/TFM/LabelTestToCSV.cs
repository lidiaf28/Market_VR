using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class LabelTestToCSV : MonoBehaviour
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

    [Header("Puntos Azul")]
    public Transform AzulP1;
    public Transform AzulP2;

    [Header("Puntos Rojo")]
    public Transform RojaP1;
    public Transform RojaP2;

    [Header("Puntos Verde")]
    public Transform VerdeP1;
    public Transform VerdeP2;

    [Header("Puntos Naranja")]
    public Transform NaranjaP1;
    public Transform NaranjaP2;

    [Header("Puntos Blanco")]
    public Transform BlancaP1;
    public Transform BlancaP2;

    [Header("Puntos Rosa")]
    public Transform RosaP1;
    public Transform RosaP2;

    [Header("Puntos Amarillo")]
    public Transform AmarillaP1;
    public Transform AmarillaP2;

    [Header("Puntos Morado")]
    public Transform MoradaP1;
    public Transform MoradaP2;


    private string fileName = "AreasTest.csv";

    [ContextMenu("Generar CSV")]//opcion para generar el csv al pulsar, en ejecucion


    public void GenerarCSV()
    {
        string filePath = Path.Combine(Application.dataPath, "PROYECT", "scripts", "TFM", fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Cabeceras
            writer.WriteLine("Area;P1;P2");

            // Escribir filas
            EscribirLinea(writer, "Ventana", ventanaP1, ventanaP2);
            EscribirLinea(writer, "Etiqueta", etiquetaP1, etiquetaP2);
            EscribirLinea(writer, "AreaAzul", AzulP1, AzulP1);
            EscribirLinea(writer, "AreaRoja", RojaP1, RojaP2);
            EscribirLinea(writer, "AreaVerde", VerdeP1, VerdeP2);
            EscribirLinea(writer, "AreaNaranja", NaranjaP1, NaranjaP2);
            EscribirLinea(writer, "AreaBlanca", BlancaP1, BlancaP2);
            EscribirLinea(writer, "AreaRosa", RosaP1, RosaP2);
            EscribirLinea(writer, "AreaAmarilla", AmarillaP1, AmarillaP2);
            EscribirLinea(writer, "AreaMorada", MoradaP1, MoradaP2);
            EscribirLineaPunto(writer, "VentanaCentro", ventanaCentro);
        }

        Debug.Log("CSV generado en: " + filePath);
    }
    private void EscribirLineaPunto(StreamWriter writer, string area, Transform p)
    {
        Vector3 localP = p.localPosition;
        string posStr = $"({localP.x.ToString(CultureInfo.InvariantCulture)}, {localP.y.ToString(CultureInfo.InvariantCulture)})";

        writer.WriteLine($"{area};{posStr}");
    }

    private void EscribirLinea( StreamWriter writer, string area, Transform p1, Transform p2)
    {
        Vector3 localP1 = p1.localPosition;
        Vector3 localP2 = p2.localPosition;

        string p1Str = $"({localP1.x.ToString(CultureInfo.InvariantCulture)}, {localP1.y.ToString(CultureInfo.InvariantCulture)})";
        string p2Str = $"({localP2.x.ToString(CultureInfo.InvariantCulture)}, {localP2.y.ToString(CultureInfo.InvariantCulture)})";

        writer.WriteLine($"{area};{p1Str};{p2Str}");
    }
}