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
                // Aquí agregamos al enemigo en el índice 1 del array fighters
                this.combatManager.fighters[1] = enemy; // Agregar al índice 1

                enemy.statusPanel.SetStats(enemy.stats, enemy.idName); // Actualiza las estadísticas

                // Aseguramos que el turno del enemigo se inicie correctamente
                this.combatManager.fighterIndex = 1; // Establecer el índice para el turno del enemigo

                // Actualiza el estado del combate para continuar
                this.combatManager.combatStatus = CombatStatus.NEXT_TURN; // Cambiar a NEXT_TURN

 
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
                combatManager.RemoveFighter(fighter); // Asegurarnos de remover al enemigo del CombatManager antes de destruirlo
            }

            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }
    public void RespawnEnemy()
    {
        RemoveEnemy();  // Elimina el enemigo anterior
        SpawnEnemy();   // Crea uno nuevo

        // Aquí aseguramos que el combate pase a la siguiente fase después de respawnear el enemigo.
        combatManager.fighterIndex = 1; // Establecer al nuevo enemigo como el siguiente en el turno.
        combatManager.combatStatus = CombatStatus.NEXT_TURN; // Cambiar a NEXT_TURN
    }



}
