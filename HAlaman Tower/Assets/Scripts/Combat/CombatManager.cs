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
    public int fighterIndex;

    private bool isCombatActive;

    public EnemySpawner enemySpawner;

    //Para diferenciar los distintos estados del combate
    public CombatStatus combatStatus;

    private Skill currentFighterSkill;


    // Start is called before the first frame update
    void Start()
    {
        LogPanel.Write("Battle initiated");

        // Verificar si hay un enemigo. Si no, generar uno
        if (fighters.Length < 2 || fighters[1] == null)
        {
            enemySpawner.SpawnEnemy(); // Generar enemigo si falta
        }

        // Asegurar que todos los luchadores tengan referencia al CombatManager
        foreach (var fgtr in this.fighters)
        {
            if (fgtr != null) // Evitar errores si hay algún slot vacío
            {
                fgtr.combatManager = this;
            }
        }

        this.fighterIndex = -1;
        this.isCombatActive = true;

        Debug.Log("Llega");
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
                    Fighter player = this.fighters[0]; // Se asume que el jugador es el primero en la lista
                    Fighter enemy = this.fighters[1];  // Se asume que el enemigo es el segundo

                    // Si el enemigo ha sido derrotado, el jugador gana
                    if (!enemy.isALive)
                    {
                        this.isCombatActive = false;
                        LogPanel.Write("Victory!");

                        // Subir de nivel al jugador
                        player.LevelUp();

                        // Esperar un momento antes de reemplazar al enemigo (opcional)
                        yield return new WaitForSeconds(8f);

                        // Reemplazar enemigo con uno nuevo
                        enemySpawner.RespawnEnemy();

                        // Esperar a que el nuevo enemigo se genere antes de continuar el combate
                        yield return new WaitForSeconds(0.5f);

                        // **NUEVO CÓDIGO: Obtener y asignar el nuevo enemigo**
                        Fighter newEnemy = enemySpawner.currentEnemy.GetComponent<Fighter>();

                        if (newEnemy != null)
                        {
                            // Reemplazar el enemigo en la lista de fighters
                            this.fighters[1] = newEnemy;

                            // Asignar CombatManager al nuevo enemigo
                            newEnemy.combatManager = this;
                        }

                        // Reactivar combate y continuar con el siguiente turno
                        this.isCombatActive = true;
                        this.combatStatus = CombatStatus.NEXT_TURN;
                    }
                    // Si el jugador ha sido derrotado, la IA gana
                    else if (!player.isALive)
                    {
                        this.isCombatActive = false;
                        LogPanel.Write("Defeated!");
                    }
                    // Si ambos siguen vivos, continúa el combate
                    else
                    {
                        this.combatStatus = CombatStatus.NEXT_TURN;
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

    public void AddFighter(Fighter newFighter)
{
    List<Fighter> fighterList = new List<Fighter>(this.fighters);
    fighterList.Add(newFighter);
    this.fighters = fighterList.ToArray();

    newFighter.combatManager = this;  // Asignamos el CombatManager al nuevo luchador

}

public void RemoveFighter(Fighter fighterToRemove)
{
    List<Fighter> fighterList = new List<Fighter>(this.fighters);
    if (fighterList.Contains(fighterToRemove))
    {
        fighterList.Remove(fighterToRemove);
        this.fighters = fighterList.ToArray();
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
