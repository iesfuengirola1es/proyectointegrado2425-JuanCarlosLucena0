using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager1 : MonoBehaviour
{
    public static MusicManager1 Instance;

    public AudioSource audioSource;  // Fuente de audio principal
    public AudioClip backgroundMusic;  // M�sica normal
    public AudioClip gameOverMusic;  // M�sica de muerte

    private void Awake()
    {
        // Hacer que este objeto no se destruya al cambiar de escena
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource.volume = 0.1f;
        PlayMusic(backgroundMusic); // Iniciar con la m�sica normal
    }

    // M�todo para cambiar la m�sica
    public void PlayMusic(AudioClip newClip)
    {
        audioSource.Stop(); // Detener la m�sica actual
        audioSource.clip = newClip; // Cambiar la canci�n
        audioSource.Play(); // Reproducir nueva m�sica
    }

}
