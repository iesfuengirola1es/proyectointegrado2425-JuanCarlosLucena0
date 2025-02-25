using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Lista de posibles enemigos
    public GameObject currentEnemy;   // Referencia al enemigo actual

    public Transform spawnPoint;       // Lugar donde aparecer�n los enemigos
    public StatusPanel statusPanel;    // Referencia al StatusPanel global para asignar a los enemigos
    public CombatManager combatManager; // Referencia al CombatManager


    public void SpawnEnemy(int level=1)
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("No hay enemigos en la lista!");
            return;
        }

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        currentEnemy = Instantiate(enemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // Aseg�rate de que el nuevo enemigo tenga asignado su StatusPanel
        Fighter fighter = currentEnemy.GetComponent<Fighter>();
        if (fighter != null)
        {
            // Generar estad�sticas aleatorias seg�n el nivel
            int health = level * Random.Range(8, 11);
            int attack = level * Random.Range(1, 3);
            int defense = level * Random.Range(1, 3);
            int spirit = 3;

            if (level % 10 == 0)
            {
                health = level * 12;
                attack = level * 5;
                defense = level * 3;
            }

            fighter.stats = new Stats(level, health, attack, defense, spirit);
            Debug.Log($"Attack: {attack}, Defense: {defense}");

            // Asignar el StatusPanel del spawner al nuevo enemigo
            fighter.statusPanel = statusPanel; // Asigna el StatusPanel

            // Si el enemigo es de tipo `Enemy`, actualizamos sus estad�sticas en el panel
            Enemy enemy = fighter as Enemy;
            if (enemy != null)
            {
                // Agregamos al enemigo a la lista usando AddFighter
                this.combatManager.AddFighter(enemy);

                enemy.statusPanel.SetStats(enemy.stats, enemy.idName); // Actualiza las estad�sticas

                // Aqu� aseguramos que el combate pase a la siguiente fase despu�s de respawnear el enemigo.
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
        int newLevel = 1; // Nivel base

        if (currentEnemy != null)
        {
            Enemy oldEnemy = currentEnemy.GetComponent<Enemy>();
            if (oldEnemy != null)
            {
                newLevel = oldEnemy.stats.lvl + 1; // Subir de nivel al siguiente enemigo
            }

            RemoveEnemy();  // Elimina el enemigo anterior
        }

        SpawnEnemy(newLevel);  // Spawnea un nuevo enemigo con stats aleatorios basados en su nivel


        // Aqu� aseguramos que el combate pase a la siguiente fase despu�s de respawnear el enemigo.
        this.combatManager.combatStatus = CombatStatus.NEXT_TURN; // Cambiar a NEXT_TURN
        this.combatManager.fighterIndex = 1; // Establecer al nuevo enemigo como el siguiente en el turno.

    }



}
