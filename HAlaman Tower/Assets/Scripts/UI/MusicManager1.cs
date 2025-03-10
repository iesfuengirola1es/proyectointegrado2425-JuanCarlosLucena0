using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager1 : MonoBehaviour
{
    public static MusicManager1 Instance;

    public AudioSource audioSource;  // Fuente de audio principal
    public AudioClip backgroundMusic;  // Música normal
    public AudioClip gameOverMusic;  // Música de muerte

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
        PlayMusic(backgroundMusic); // Iniciar con la música normal
    }

    // Método para cambiar la música
    public void PlayMusic(AudioClip newClip)
    {
        audioSource.Stop(); // Detener la música actual
        audioSource.clip = newClip; // Cambiar la canción
        audioSource.Play(); // Reproducir nueva música
    }

}
