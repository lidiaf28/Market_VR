using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProductOrderLoader : MonoBehaviour
{
    [Header("Configuración")]
    public int sujeto = 0;  // Número del sujeto
    public string csvFileName = "orden_productos.csv";

    [Header("Productos (A,B,C)")]
    public GameObject productoA;
    public GameObject productoB;
    public GameObject productoC;

    [Header("Posiciones de los productos")]
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;

    private Dictionary<int, string[]> ordenPorSujeto = new Dictionary<int, string[]>();

    void Start()
    {
        DesactivarProductos();

        if (sujeto == 0) //no se ha puesto el numero de sujeto
        {
            Debug.LogWarning("[ProductOrderLoader] No se ha asignado número de sujeto. No se cargará ningún producto.");
            return;
        }

        CargarCSV();


        if (!ordenPorSujeto.ContainsKey(sujeto)) //  numero de sujeto no encontrado en la lista
        {
            Debug.LogError("[ProductOrderLoader] El sujeto " + sujeto + " no está en el CSV.");
            return;
        }

        string[] orden = ordenPorSujeto[sujeto]; // obtenemos el orden para el sujeto

        if (orden.Length != 3)
        {
            Debug.LogError("[ProductOrderLoader] Orden inválido para el sujeto " + sujeto);
            return;
        }

        ColocarProductos(orden); // ponemos los productos en las posiciones
    }

    void CargarCSV()
    {
        string filePath = Path.Combine(Application.dataPath, "PROYECT/Scripts/TFM", csvFileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError("[ProductOrderLoader] NO se encontró el CSV en: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++) // saltamos header
        {
            string line = lines[i].Trim(); // limpiar espacios
            if (string.IsNullOrEmpty(line)) continue; // saltar líneas vacías

            Debug.Log("[CSV] Línea original: '" + line + "'"); // debug línea original

            // Dividir por punto y coma
            string[] parts = line.Split(';'); //dividir por ;
            if (parts.Length < 2) // verificar que hay al menos 2 campos
            {
                Debug.LogError("[CSV] Línea inválida (faltan campos): " + line);
                continue;
            }

            // primer campo: ID de sujeto
            if (!int.TryParse(parts[0].Trim(), out int id)) 
            {
                Debug.LogError("[CSV] No se pudo leer el ID de sujeto en: " + parts[0]);
                continue;
            }

            // Segundo campo: orden de productos
            string ordenRaw = parts[1].Trim();

            // ELIMINAR cualquier comilla y retorno de carro/espacios invisibles
            //ordenRaw = ordenRaw.Replace("\"", "").Replace("\r", "").Replace("\n", "").Trim();

            // Dividir por comas
            string[] orden = ordenRaw.Split(',');

            for (int j = 0; j < orden.Length; j++)
                orden[j] = orden[j].Trim();

            ordenPorSujeto[id] = orden;

            Debug.Log("[CSV] Guardado sujeto " + id + " => " + string.Join(",", orden));
        }

        Debug.Log("[ProductOrderLoader] CSV cargado correctamente.");
    }



    void ColocarProductos(string[] orden)
    {
        Debug.Log($"[ProductOrderLoader] Orden sujeto {sujeto}: {orden[0]}-{orden[1]}-{orden[2]}"); // debug del orden A B C

        PosicionProducto(orden[0], pos1);
        PosicionProducto(orden[1], pos2);
        PosicionProducto(orden[2], pos3);
    }

    void PosicionProducto(string letra, Transform destino)
    {
        GameObject obj = ObtenerProducto(letra);

        if (obj != null && destino != null) // si el producto y destino son válidos
        {
            obj.transform.position = destino.position; // movemos a la posición
            obj.SetActive(true);// activamos el producto
        }
    }

    GameObject ObtenerProducto(string letra)  // devuelve el GameObject correspondiente a la letra
    {
        switch (letra.ToUpper())
        {
            case "A": return productoA;
            case "B": return productoB;
            case "C": return productoC;
            default:
                Debug.LogError("[ProductOrderLoader] Letra desconocida: " + letra);
                return null;
        }
    }

    void DesactivarProductos()
    {
        if (productoA) productoA.SetActive(false);
        if (productoB) productoB.SetActive(false);
        if (productoC) productoC.SetActive(false);
    }
}
