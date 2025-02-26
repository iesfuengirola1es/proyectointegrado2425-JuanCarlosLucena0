using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    private void Awake()
    {
        this.stats = new Stats(10, 15, 5, 1, 3);

    }

     public override void InitTurn()
     {

        StartCoroutine(this.IA());

     }

    IEnumerator IA()
    {
        yield return new WaitForSeconds(1f);

        //Cogemos una habilidad aleatoria
        Skill skill = this.skills[Random.Range(0, this.skills.Length)];

        //Enviamos la habilidad al combat manager
        skill.SetEmitterAndReciver(this, this.combatManager.GetOppositeFighter());

        this.combatManager.OnFigtherSkill(skill);

    }

}
