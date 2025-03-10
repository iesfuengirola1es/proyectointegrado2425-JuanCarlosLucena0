using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public string idName;
    public StatusPanel statusPanel;

    public CombatManager combatManager;

    public Stats stats;

    protected Skill[] skills;

    public bool isALive
    {
        get => this.stats.health > 0;
    }
      

    protected virtual void Start()
    {
        this.statusPanel.SetStats(name: this.idName, stats: this.stats);

        this.skills = this.GetComponentsInChildren<Skill>();
    }
    //Modifica la vida de los luchadores
    public void ModifyHeath(float amount)
    {
        //Modificamos la vida en funcion de la cantidad establecida
        this.stats.health = Mathf.Clamp(this.stats.health + amount, 0f, this.stats.maxHealth);

        //Redondeamos la vida a una cantidad no decimal
        this.stats.health = Mathf.Round(this.stats.health);

        this.statusPanel.SetHealth(this.stats.health, this.stats.maxHealth);
    }

    // Se encarga de subir el nivel del luchador establecido, subiendo la vida, ataque y defensa, la vida entre 3 y 6, y ataque y defensa entre 1 y 4
    public void LevelUp()
    {
        Debug.Log($"Antes del Level Up: Nivel {this.stats.lvl}, Salud {this.stats.health}/{this.stats.maxHealth}, Ataque {this.stats.attack}, Defensa {this.stats.defense}");


        float previousHP = this.stats.maxHealth;
        float previousATK = this.stats.attack;
        float previousDEF = this.stats.defense;

        float hpGain= Random.Range(3, 6);

        float actualLevel=this.stats.lvl++;
        this.stats.maxHealth += hpGain;
        this.stats.health += hpGain;
        this.stats.attack += Random.Range(1, 4);
        this.stats.defense += Random.Range(1, 4);

        LogPanel.Write($"{this.idName} ha subido a Nivel {this.stats.lvl}!");

        StartCoroutine(ShowStatChanges(previousHP, previousATK, previousDEF));

        statusPanel.SetHealth(this.stats.health, this.stats.maxHealth);
        statusPanel.levelLable.text = $"Lvl {this.stats.lvl}";

        Debug.Log($"Después del Level Up: Nivel {this.stats.lvl}, Salud {this.stats.health}/{this.stats.maxHealth}, Ataque {this.stats.attack}, Defensa {this.stats.defense}");
    }

    private IEnumerator ShowStatChanges(float previousHP, float previousATK, float previousDEF)
    {
        // Esperar 3 segundos antes de mostrar los cambios de estadísticas
        yield return new WaitForSeconds(3f);

        // Mostrar los cambios de estadísticas en el log durante 5 segundos
        LogPanel.Write($"HP: {previousHP} -> {this.stats.maxHealth}\n" +
                        $"Ataque: {previousATK} -> {this.stats.attack}\n" +
                        $"Defensa: {previousDEF} -> {this.stats.defense}");

        // Esperar 5 segundos antes de pasar al siguiente mensaje
        yield return new WaitForSeconds(5f);

        LogPanel.Write(""); // Limpiar el log o mostrar el siguiente mensaje
    }
    // Obtiene las estadisticas actuales
        public Stats GetCurrentStats()
    {

        return this.stats;
    }
    //Metodo abstracto para iniciar el turno
    public abstract void InitTurn();
}
