using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float health;
    public float maxHealth;


    public int lvl;
    public float attack;
    public float defense;
    public float spirit;

    public Stats(int lvl, float maxHealth, float attack, float defense, float spirit)
    {
        this.lvl = lvl;
        this.maxHealth = maxHealth;
        this.health = maxHealth;

        this.attack = attack;
        this.defense = defense;
        this.spirit = spirit;
    }

    public Stats Clone()
    {
        return new Stats(this.lvl, this.maxHealth, this.attack, this.defense, this.spirit);
    }

}
