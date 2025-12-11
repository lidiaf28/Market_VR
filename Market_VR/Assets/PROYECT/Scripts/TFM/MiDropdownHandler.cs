using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdownCompraSiNo;

    public Button botonSi;   // Botón que se activa si eliges "Sí"
    public Button botonNo;   // Botón que se activa si eliges "No"

    void Start()
    {
        dropdownCompraSiNo.onValueChanged.AddListener(CambiarOpcion);

        // Al inicio desactivamos ambos
        botonSi.gameObject.SetActive(false);
        botonNo.gameObject.SetActive(false);
    }

    void CambiarOpcion(int index)
    {
        switch (index)
        {
            case 0:
                // Opción de control  no hacer nada, apagar ambos botones
                botonSi.gameObject.SetActive(false);
                botonNo.gameObject.SetActive(false);
         
                break;

            case 1:
                // Acciones de SÍ aquí
                botonSi.gameObject.SetActive(true);
                botonNo.gameObject.SetActive(false);
                break;

            case 2:
                // Acciones de NO aquí
                botonSi.gameObject.SetActive(false);
                botonNo.gameObject.SetActive(true);
                break;
        }
    }
}