using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewDropdownControllerLite : MonoBehaviour
{
    public TMP_Dropdown dropdownCompraSiNo;
    public GameObject sliderPrecio;

    private void Start()
    {
        dropdownCompraSiNo.onValueChanged.AddListener(CambiarOpcionSNo);

        sliderPrecio.SetActive(false);
    }
    void CambiarOpcionSNo(int index)
    {
        switch (index)
        {
            case 0:
                sliderPrecio.SetActive(false);
                break;

            case 1:
                // Acciones de SÍ aquí
                sliderPrecio.SetActive(true);
                break;

            case 2:
                // Acciones de NO aquí
                sliderPrecio.SetActive(false);
                break;
        }
    }

}
