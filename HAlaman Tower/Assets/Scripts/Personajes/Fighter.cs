using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public string idName;
    public StatusPanel statusPanel;

    public CombatManager combatManager;

    protected Stats stats;

    protected Skill[] skills;

    public bool isALive
    {
        get => this.stats.health > 0;
    }
      

    protected virtual void Start()
    {
        this.statusPanel.SetStats(this.idName, this.stats);

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

    public Stats GetCurrentStats()
    {

        return this.stats;
    }

    public abstract void InitTurn();
}
