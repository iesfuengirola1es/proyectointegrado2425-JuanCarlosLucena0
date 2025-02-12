using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerSkillsPanel : MonoBehaviour
{

    public GameObject[] skillButtons;

    public TextMeshProUGUI[] skillButtonLabel;

    //Desactivamos todos los botones de serie
    private void Awake()
    {
        this.Hide();

        foreach (var btn in this.skillButtons)
        {
            btn.SetActive(false);
        }
    }

    //Activamos individualmente los botones a la vez q le asignamos el nombre de la habilidad
    public void ConfigureButtons(int index, string skillName)
    {
        this.skillButtons[index].SetActive(true);
        this.skillButtonLabel[index].text = skillName;
    }

    //Mostramos los botones
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    //Los escondemos
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
