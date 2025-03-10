using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Modificacion de vida basada en: Stats   // Cantidades fijas  // Porcentajes

public enum HealthModType
{
    STAT_BASE, FIXED, PERCENTAGE
}
public class HealthModSkill : Skill
{
    [Header("Health amount")]
    public float amount;

    public HealthModType modType;

    //override de la funcion "OnRun" que se ejecuta en cada turno de los personajes
    protected override void OnRun()
    {
        //Cogemos la cantidad según el tipo de modificacion de salud
        float amount = this.GetModification();

        //Modificamos la salud del receptor segun el calculo realizado anteriormente 
        this.receiver.ModifyHeath(amount);
    }
    //Realiza la acción correspondiente a la vida según el tipo de modificacion de vida
    public float GetModification()
    {
        switch (this.modType)
        {
            case HealthModType.STAT_BASE: //Si la modificacion de vida se basa en el ataque y la defensa

                //Recogemos los stats del emisor
                Stats emitterStats = this.emitter.GetCurrentStats();

                //Recogemos los stats del receptor
                Stats reciverStats = this.receiver.GetCurrentStats();

                // Formula de daño
                float rawDamage = (((2 * emitterStats.lvl) / 5) + 2) * this.amount * (emitterStats.attack / reciverStats.defense);
                Debug.Log($"lvl: {emitterStats.lvl}, atk: {emitterStats.attack}, def: {reciverStats.defense}, amount: {this.amount}, rawDamage: {rawDamage}");
                Debug.Log((rawDamage/50) - 2);
                return (rawDamage / 50) - 2;

            case HealthModType.FIXED:  //Si la cantidad es fija

                return this.amount;

            case HealthModType.PERCENTAGE: // Si afecta a un porcentaje de la vida
                Stats rStats = this.receiver.GetCurrentStats();

                return rStats.maxHealth * this.amount;
        }
        throw new System.InvalidOperationException("HealthModSkill::GetDamage.Unrecheable! ");

    }
}
