using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.InputSystem;

public class ConexionRealTimeFirebase : MonoBehaviour
{
    private FirebaseApp appFirebase;
    private FirebaseDatabase dbFirebase;
    private DatabaseReference referenciaJugadores;
    private DatabaseReference referenciaDispositivo;
    public PlayerController controladorJugador;
    private float temporizadorActualizacion;
    private DatabaseReference referenciaPickups;
    public GameObject prefabPickup;

    void Start()
    {
        controladorJugador = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        IniciarFirebase();
        dbFirebase = FirebaseDatabase.DefaultInstance;
        referenciaJugadores = dbFirebase.GetReference("Jugadores");
        referenciaPickups = dbFirebase.GetReference("Pickups");
        CargarPickups();
        RegistrarDispositivo();
    }

    FirebaseApp IniciarFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(tarea =>
        {
            var estadoDependencia = tarea.Result;
            if (estadoDependencia == DependencyStatus.Available)
            {
                appFirebase = FirebaseApp.DefaultInstance;
            }
            else
            {
                Debug.LogError($"No se pudieron resolver todas las dependencias de Firebase: {estadoDependencia}");
                appFirebase = null;
            }
        });

        return appFirebase;
    }

    void CargarPickups()
    {
        referenciaPickups.GetValueAsync().ContinueWithOnMainThread(tarea =>
        {
            if (tarea.IsFaulted)
            {
                // Manejar el error...
            }
            else if (tarea.IsCompleted)
            {
                DataSnapshot snapshot = tarea.Result;
                foreach (var item in snapshot.Children)
                {
                    Vector3 posicionSpawn = ExtraerPosicion(item);
                    GenerarPickup(posicionSpawn);
                }
            }
        });
    }

    Vector3 ExtraerPosicion(DataSnapshot datos)
    {
        float posX = float.Parse(datos.Child("x").Value.ToString());
        float posY = float.Parse(datos.Child("y").Value.ToString());
        float posZ = float.Parse(datos.Child("z").Value.ToString());
        return new Vector3(posX, posY, posZ);
    }

    void GenerarPickup(Vector3 posicion)
    {
        Instantiate(prefabPickup, posicion, Quaternion.identity);
    }

    void RegistrarDispositivo()
    {
        string idDispositivo = SystemInfo.deviceUniqueIdentifier;
        referenciaJugadores.Child(idDispositivo).Child("nombre").SetValueAsync("Dispositivo Usuario");
    }

    void Update()
    {
        string idDispositivo = SystemInfo.deviceUniqueIdentifier;
        referenciaJugadores.Child(idDispositivo).Child("puntos").SetValueAsync(controladorJugador.count);
    }
}
