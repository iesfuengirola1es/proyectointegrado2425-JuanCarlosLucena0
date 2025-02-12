using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatStatus
{
    WAITING_FOR_FIGHTER,
    FIGHTER_ACTION,
    CHECK_FOR_VICTORY,
    NEXT_TURN
}

public class CombatManager : MonoBehaviour
{

    public Fighter[] fighters;
    private int fighterIndex;

    private bool isCombatActive;

    //Para diferenciar los distintos estados del combate
    private CombatStatus combatStatus;

    private Skill currentFighterSkill;


    // Start is called before the first frame update
    void Start()
    {
        LogPanel.Write("Battle initiated");

        //asigna una referencia al combat manager en cada luchador para q los luchadores puedan enviar las habilidades q ejecutan
        //se apoya en "currentFighterSkill" para hacerlo
        foreach (var fgtr in this.fighters)
        {
            fgtr.combatManager = this;

        }

        //pasamos el turno
        this.combatStatus = CombatStatus.NEXT_TURN;
        
        //se lo damos al q tenga index 0, nuestro personaje en este caso (inicia en -1 ya que iniciamos pasando el turno al index 0)
        this.fighterIndex = -1;

        //activamos el combate

        this.isCombatActive = true;

        //encnedemos el combate por turnos

        StartCoroutine(this.CombatLoop());
    }

    IEnumerator CombatLoop()
    {
        while (this.isCombatActive)
        {
            switch (this.combatStatus)
            {

                //Se queda quieto esperando que el luchador envie una habilidad
                case CombatStatus.WAITING_FOR_FIGHTER:
                    yield return null;
                    break;

                //Cuando llega la acción del luchador
                case CombatStatus.FIGHTER_ACTION:
                    //Mensaje indicando el luchador y que accion realiza
                    LogPanel.Write($"{this.fighters[this.fighterIndex].idName} usó {currentFighterSkill.skillName}");

                    //Saltamos un frame
                    yield return null;

                    //Ejecutamos la habilidad del luchador
                    currentFighterSkill.Run();

                    //Esperamos a que terminen las animaciones si es que tienen
                    yield return new WaitForSeconds(currentFighterSkill.animationDuration);

                    //Comprobamos si el que atacó ganó
                    this.combatStatus = CombatStatus.CHECK_FOR_VICTORY;

                    //Volvemos a poner la skill usada como nula
                    currentFighterSkill = null;

                    break;

                //Comprobaciones de la victoria
                case CombatStatus.CHECK_FOR_VICTORY:
                    foreach (var fgtr in this.fighters)
                    {
                        //Comprueba si el luchador esta vivo, da victoria si no lo esta, continua el combate si lo está
                        if(fgtr.isALive == false)
                        {
                            this.isCombatActive = false;

                            LogPanel.Write("Victory!");
                        }
                        else
                        {
                            this.combatStatus = CombatStatus.NEXT_TURN;
                        }
                    }
                    yield return null;

                    break;



                //Pasa el turno
                case CombatStatus.NEXT_TURN:

                    yield return new WaitForSeconds(1f);

                    //Actualizamos  el turno asignandoselo a la siguiente persona
                    this.fighterIndex = (this.fighterIndex + 1) % this.fighters.Length;


                    var currentTurn = this.fighters[this.fighterIndex];
                    //muestra el mensaje de quien tiene el turno e inicia el turno del q le toque
                    LogPanel.Write($"Turno de {currentTurn.idName}");
                    currentTurn.InitTurn();


                    // se queda esperando a q se realice la acción
                    this.combatStatus = CombatStatus.WAITING_FOR_FIGHTER;

                    break;
            }
        }
    }


    public Fighter GetOppositeFighter()
    {
        if (this.fighterIndex == 0)
        {
            return this.fighters[1];
        }
        else
        {
            return this.fighters[0];
        }
    }


    //Asignamos la habilidad que deseamos utilizar y ponemos el estado en "FighterAction"
    public void OnFigtherSkill(Skill skill)
    {
        this.currentFighterSkill = skill;
        this.combatStatus = CombatStatus.FIGHTER_ACTION;
    }
}
