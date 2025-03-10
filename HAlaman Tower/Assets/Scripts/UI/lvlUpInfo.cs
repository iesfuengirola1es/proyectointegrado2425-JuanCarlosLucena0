using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class lvlUpInfo : MonoBehaviour
{
    public TextMeshProUGUI levelUpText;  // Texto del mensaje de nivel

    public float displayTime = 3f;  // Duración del popup en pantalla

    private void Awake()
    {
        gameObject.SetActive(false);  // Ocultar el popup al inicio
    }
    //Muestra el pop up
    public void ShowPopup(string playerName, int newLevel, float hpIncrease, float atkIncrease, float defIncrease)
    {
        // Asegúrate de que levelUpText esté asignado
        if (levelUpText == null)
        {
            Debug.LogError("No se asignó 'levelUpText' en el Inspector.");
            return;
        }

        // Configurar el texto del popup
        levelUpText.text = $"{playerName} ha subido a Nivel {newLevel}!\n\n" +
                           $"{hpIncrease} HP\n" +
                           $"{atkIncrease} ATK\n" +
                           $"{defIncrease} DEF";

        // Mostrar el popup
        gameObject.SetActive(true);
        StartCoroutine(HidePopupAfterDelay());
    }
    //Lo quita despues de un tiempo
    private IEnumerator HidePopupAfterDelay()
    {
        // Espera el tiempo configurado antes de ocultar el popup
        yield return new WaitForSeconds(displayTime);
        gameObject.SetActive(false);  // Ocultar el popup después de X segundos
    }




}
