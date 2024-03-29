# Integración Firebase en proyecto

Para poder integrar Firebase a nuestro proyecto lo primero será ir a la página oficial de firebase y crear un proyecto.

Una vez creado descargamos el google-service.json a tu proyecto de unity y despues descargaremos la carpeta SDK y despues exportaremos el analyce al proyecto de unity.

![alt text](<Fotos/Captura de pantalla 2024-03-13 193420.png>)


## Base de datos 

Ahora iremos a Realtime Database en la cual podremos trabajar con el formate de Firebase. 

![alt text](<Fotos/Captura de pantalla 2024-03-13 194527.png>)


## Al comenzar el juego 

Recogerá la posición de los prefabs y los posicionará en función de la base de datos.

Primero para que podamos interactuar en la base de datos crearemos un obejeto con un script el cual llamaremos Realtime.cs el cual posicionará los elementos 


Ahora para hacer la referencia a la variable de la base de datos lo haremos con el siguiente código.

```csharp

referenciaPickups = dbFirebase.GetReference("Pickups");
referenciaPickups.GetValueAsync().ContinueWithOnMainThread(tarea =>
{
    if (tarea.IsFaulted)
    {
        // Aquí podrías manejar el error, por ejemplo, mostrando un mensaje al usuario.
    }
    else if (tarea.IsCompleted)
    {
        DataSnapshot snapshot = tarea.Result;
        // Procesa el snapshot para generar los pickups.
        ProcesarPickups(snapshot);
    }
});


```

Esto hacer referencia a la base de datos de Firebase la cual encuentra info de los elementos los cuales posicionaremos en el juego 

```csharp
void ProcesarPickups(DataSnapshot snapshot)
{
    foreach (DataSnapshot item in snapshot.Children)
    {
        // Aquí podrías extraer las coordenadas de cada pickup y crearlos en la escena.
        var posicion = new Vector3(
            float.Parse(item.Child("x").Value.ToString()),
            float.Parse(item.Child("y").Value.ToString()),
            float.Parse(item.Child("z").Value.ToString())
        );
        
        CrearPickup(posicion);
    }
}

```

Aqui con la función ProcesarPickUps recorre los datos y almacena las coordenadas y luego CrearPickUp se encargara de el poscionamiento del elemento. De esta manera los objetos se posicionaran al comenzar el juego donde le corresponde


Antes de iniciar no se verán los pickups

![alt text](<Fotos/Captura de pantalla 2024-03-13 201136.png>)

Y al ejecutar los pickups ya se posicionan.


![alt text](<Fotos/Captura de pantalla 2024-03-13 201212.png>)





