using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class options : MonoBehaviour
{
    public GameObject optionsPanel; // El panel de opciones que quieres mostrar

    public void OpenOptionsMenu()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
            Debug.Log("Panel funciona");
        }
    }
}
