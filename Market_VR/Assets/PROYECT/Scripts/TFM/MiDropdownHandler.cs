using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiDropdownHandler : MonoBehaviour
{
    [Header("DesplegableSiNo")]
    public TMP_Dropdown dropdownCompraSiNo;
    public TMP_Dropdown dropdownConfirmarCompra;

    [Header("Botones")]
    public Button botonSi;   // Botón que se activa si eliges "Sí"
    public Button botonNo;   // Botón que se activa si eliges "No"
    public Button botonComprar;   // <-- EL BOTÓN FINAL


    [Header("Productos")]
    public ProductoUI[] productos;   // Lista de productos con su dropdown, botón y slider


    [System.Serializable]
    public class ProductoUI //para englobar todo lo relacionado con un mismo producto
    {
        public TMP_Dropdown dropdown;
        public Button boton;
        public Slider slider;

        [HideInInspector] public bool terminado = false;
        [HideInInspector] public bool elegidoSi = false;
    }


    void Start()
    {
        dropdownCompraSiNo.onValueChanged.AddListener(CambiarOpcionSNo);

        // Inicializar productos (bloqueados)
        foreach (var p in productos)
        {
            // Desactivar interacción pero se siguen viendo
            p.boton.interactable = false;
            p.slider.interactable = false;

            // Escuchar cada dropdown con la misma función
            p.dropdown.onValueChanged.AddListener((i) => CompraProducto(p, i)); //se escucha el cambio de opcion

            // Escuchar el botón de “añadir a la cesta”
            p.boton.onClick.AddListener(() => ConfirmarCompra(p)); //no hace falta que lo ponga en OnClick, se escucha aqui
        }
        //BOTON COMPRAR
        botonComprar.interactable = false; // Desactivado al inicio
        botonComprar.gameObject.SetActive(false); //desactivado para que no se vea
        //DROPDOWN CONFIRMACION
        dropdownConfirmarCompra.onValueChanged.AddListener(ConfirmarCompraFinal);
        dropdownConfirmarCompra.gameObject.SetActive(false);

        dropdownCompraSiNo.gameObject.SetActive(false);

        // Al inicio desactivamos ambos
        botonSi.gameObject.SetActive(false);
        botonNo.gameObject.SetActive(false);
    }
    void CambiarOpcionSNo(int index)
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

    // ------------------------------------------------------------
    // Dropdown de cada producto (misma función)
    // ------------------------------------------------------------
    void CompraProducto(ProductoUI producto, int index)
    {
        switch (index)
        {
            case 0:
                // Opción de control no hacer nada
                producto.boton.interactable = false;
                producto.slider.interactable = false;
                producto.terminado = false;
                producto.elegidoSi = false;

                break;

            case 1:
                // Acciones de SÍ aquí
                producto.boton.interactable = true;
                producto.slider.interactable = true;
                producto.elegidoSi = true;
                producto.terminado = false; // Aún NO está terminado

                break;

            case 2:
                // Acciones de NO aquí
                producto.boton.interactable = false;
                producto.slider.interactable = false;
                producto.elegidoSi = false;
                producto.terminado = true; // NO cuenta como terminado inmediato
                RevisarEstadoGeneral();
                break;
        }
    }

    void ConfirmarCompraFinal(int index)
    {
        switch (index)
        {
            case 0:
                // opción por defecto, no hacer nada
                botonComprar.interactable = false;
                break;

            case 1:
                // SÍ confirmar compra
                botonComprar.interactable = true;
                break;

            case 2:
                // NO confirmar → reiniciar ventana
                ReiniciarVentana();
                break;
        }
    }

    // ------------------------------------------------------------
    // Cuando pulsa “Añadir a la cesta”
    // ------------------------------------------------------------
    void ConfirmarCompra(ProductoUI producto) //se hace en el boton, cuando se confirma la compra
    {
        if (producto.elegidoSi) 
        {
            producto.terminado = true; //le decimos que ya esta confirmado el pedido
            RevisarEstadoGeneral();
        }
    }

    void ReiniciarVentana()
    {
        // Ocultar confirmación
        dropdownConfirmarCompra.gameObject.SetActive(false);
        dropdownConfirmarCompra.SetValueWithoutNotify(0);

        // Desactivar botón comprar
        botonComprar.interactable = false;
        botonComprar.gameObject.SetActive(false);

        // Resetear productos
        foreach (var p in productos)
        {
            p.dropdown.SetValueWithoutNotify(0);
            p.boton.interactable = false;
            p.slider.interactable = false;
            p.terminado = false;
            p.elegidoSi = false;
        }

    }

    // ------------------------------------------------------------
    // Revisión global: ¿los 3 productos están listos?
    // ------------------------------------------------------------

    //se revisa cuando completados llevamos mediante producto.terminado. Si llevamos ya 3, se activa comprar
    void RevisarEstadoGeneral()
    {
        int completados = 0;

        foreach (var p in productos)
        {
            if (p.terminado)
                completados++;
        }

        if (completados == 3)
        {
            dropdownConfirmarCompra.SetValueWithoutNotify(0); // reset a "Selecciona" para las siguientes veces q se abra
            dropdownConfirmarCompra.gameObject.SetActive(true);
        }
    }
}