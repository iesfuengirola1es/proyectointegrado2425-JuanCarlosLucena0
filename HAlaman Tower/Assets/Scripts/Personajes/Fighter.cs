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

    public void ModifyHeath(float amount)
    {
        //Modificamos la vida en funcion de la cantidad establecida
        this.stats.health = Mathf.Clamp(this.stats.health + amount, 0f, this.stats.maxHealth);

        //Redondeamos la vida a una cantidad no decimal
        this.stats.health = Mathf.Round(this.stats.health);

        this.statusPanel.SetHealth(this.stats.health, this.stats.maxHealth);
    }

    public void LevelUp()
    {
        Debug.Log($"Antes del Level Up: Nivel {this.stats.lvl}, Salud {this.stats.health}/{this.stats.maxHealth}, Ataque {this.stats.attack}, Defensa {this.stats.defense}");


        this.stats.lvl++;
        this.stats.maxHealth += 5; // Restaurar salud al subir de nivel
        this.stats.attack += 1;  // Aumentar ataque (puedes cambiarlo)
        this.stats.defense += 1; // Aumentar defensa (opcional)

        Debug.Log($"Después del Level Up: Nivel {this.stats.lvl}, Salud {this.stats.health}/{this.stats.maxHealth}, Ataque {this.stats.attack}, Defensa {this.stats.defense}");
    }

    public Stats GetCurrentStats()
    {

        return this.stats;
    }

    public abstract void InitTurn();
}
