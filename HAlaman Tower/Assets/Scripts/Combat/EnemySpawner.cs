using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Lista de posibles enemigos
    public GameObject currentEnemy;   // Referencia al enemigo actual

    public Transform spawnPoint;       // Lugar donde aparecerán los enemigos
    public StatusPanel statusPanel;    // Referencia al StatusPanel global para asignar a los enemigos
    public CombatManager combatManager; // Referencia al CombatManager


    public void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("No hay enemigos en la lista!");
            return;
        }

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        currentEnemy = Instantiate(enemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // Asegúrate de que el nuevo enemigo tenga asignado su StatusPanel
        Fighter fighter = currentEnemy.GetComponent<Fighter>();
        if (fighter != null)
        {
            // Asignar el StatusPanel del spawner al nuevo enemigo
            fighter.statusPanel = statusPanel; // Asigna el StatusPanel

            // Si el enemigo es de tipo `Enemy`, actualizamos sus estadísticas en el panel
            Enemy enemy = fighter as Enemy;
            if (enemy != null)
            {
                // Agregamos al enemigo a la lista usando AddFighter
                this.combatManager.AddFighter(enemy);

                enemy.statusPanel.SetStats(enemy.stats, enemy.idName); // Actualiza las estadísticas

                // Aquí aseguramos que el combate pase a la siguiente fase después de respawnear el enemigo.
                this.combatManager.combatStatus = CombatStatus.NEXT_TURN; // Cambiar a NEXT_TURN
                this.combatManager.fighterIndex = 1; // Establecer al nuevo enemigo como el siguiente en el turno.
            }
        }
        else
        {
            Debug.LogError("El enemigo instanciado no tiene un componente Fighter.");
        }

    }

    public void RemoveEnemy()
    {
        if (currentEnemy != null)
        {
            Fighter fighter = currentEnemy.GetComponent<Fighter>();
            if (fighter != null)
            {
                this.combatManager.RemoveFighter(fighter); // Elimina el enemigo de la lista
            }

            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }
    public void RespawnEnemy()
    {
        // Elimina el enemigo anterior (si existe)
        if (this.combatManager.fighters.Count > 1) // Asegurarse de que hay más de 1 luchador
        {
            RemoveEnemy();  // Elimina el enemigo anterior
        }

        // Crea un nuevo enemigo
        SpawnEnemy();

        // Aquí aseguramos que el combate pase a la siguiente fase después de respawnear el enemigo.
        this.combatManager.combatStatus = CombatStatus.NEXT_TURN; // Cambiar a NEXT_TURN
        this.combatManager.fighterIndex = 1; // Establecer al nuevo enemigo como el siguiente en el turno.

    }



}
