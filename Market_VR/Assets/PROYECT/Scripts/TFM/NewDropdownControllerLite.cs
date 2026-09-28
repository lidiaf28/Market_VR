using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewDropdownControllerLite : MonoBehaviour
{
    public TMP_Dropdown dropdownCompraSiNo;
    public Button botonNo;   // Botón que se activa si eliges "No"
    public GameObject sliderPrecio;

    private void Start()
    {
        dropdownCompraSiNo.onValueChanged.AddListener(CambiarOpcionSNo);
        botonNo.gameObject.SetActive(false);
        sliderPrecio.SetActive(false);
    }
    void CambiarOpcionSNo(int index)
    {
        switch (index)
        {
            case 0:
                sliderPrecio.SetActive(false);
                botonNo.gameObject.SetActive(false);
                break;

            case 1:
                // Acciones de SÍ aquí
                sliderPrecio.SetActive(true);
                botonNo.gameObject.SetActive(false);
                break;

            case 2:
                // Acciones de NO aquí
                sliderPrecio.SetActive(false);
                botonNo.gameObject.SetActive(true);
                break;
        }
    }

}
