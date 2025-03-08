using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    // Referenciamos el panel de habilidades
    [Header("UI")]
    public PlayerSkillsPanel skillsPanel;

    //public Stats stats;

    private void Awake()
    {
        // Asignar el idName al nombre del usuario guardado
        if (PlayerPrefs.HasKey("username"))
        {
            this.idName = PlayerPrefs.GetString("username");
        }
        else
        {
            Debug.LogError("No se encontró el nombre de usuario en PlayerPrefs.");
            this.idName = "Jugador"; // Valor por defecto
        }

        this.stats = new Stats(1, 25, 7, 2, 5);

    }

    public override void InitTurn()
    {
        //Mostramos el panel 
        this.skillsPanel.Show();

        //Configuramos el panel con los nombres correctos
        for (int i=0; i<this.skills.Length; i++)
        {
            this.skillsPanel.ConfigureButtons(i, this.skills[i].skillName);
        }

    }

    //Ejecutamos la funcion execute skill segun el boton que pulsemos.
    //Además mostramos en el debug en nombre de la habilidad para confirmar que es el que queremos
    public void ExecuteSkill(int index)
    {
        this.skillsPanel.Hide();

        //Recogemos la habilidad seleccionada
        Skill skill = this.skills[index];

        //Establecemos receptor y emisor
        skill.SetEmitterAndReciver(this, this.combatManager.GetOppositeFighter());

        //Enviamos la skill seleccionada al combat manager
        this.combatManager.OnFigtherSkill(skill);

        Debug.Log($"Running {skill.skillName} skill");
    }

}
