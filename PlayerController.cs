using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ControladorJugador : MonoBehaviour
{
    private Rigidbody componenteRb; 

    private float desplazamientoX;
    private float desplazamientoZ;

    public float velocidad = 0;

    private float fuerzaSalto = 7.0f;

    public int contadorObjetos;
    
    public TextMeshProUGUI textoContador;

    void Start()
    {
        componenteRb = GetComponent<Rigidbody>();
        contadorObjetos = 0;
        ActualizarTextoContador();
    }

    void ActualizarTextoContador() 
    {
        textoContador.text = "Contador: " + contadorObjetos.ToString();
    }

    public void AlMoverse(InputValue valorMovimiento)
    {
        Vector2 vectorMovimiento = valorMovimiento.Get<Vector2>();
        desplazamientoX = vectorMovimiento.x;
        desplazamientoZ = vectorMovimiento.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            componenteRb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector3 movimiento = new Vector3(desplazamientoX, 0.0f, desplazamientoZ);
        componenteRb.AddForce(movimiento * velocidad);
    }

    void OnTriggerEnter(Collider otro)
    {
        if (otro.gameObject.CompareTag("PickUp"))
        {
            otro.gameObject.SetActive(false);
            contadorObjetos += 1;
            ActualizarTextoContador();
        }
    }
}
