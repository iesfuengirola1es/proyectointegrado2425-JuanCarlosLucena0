using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public enum CombatStatus
{
    WAITING_FOR_FIGHTER,
    FIGHTER_ACTION,
    CHECK_FOR_VICTORY,
    NEXT_TURN
}

public class CombatManager : MonoBehaviour
{

    public List<Fighter> fighters;  // Usar List<Fighter> en lugar de Fighter[]
    public int fighterIndex;

    private bool isCombatActive;

    public EnemySpawner enemySpawner;

    //Para diferenciar los distintos estados del combate
    public CombatStatus combatStatus;

    private Skill currentFighterSkill;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public Button restartButton;


    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false); // Ocultar panel al inicio

        LogPanel.Write("Battle initiated");

        // Verificar si hay un enemigo. Si no, generar uno
        if (fighters.Count < 2 || fighters[1] == null)
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
    //Bucle de acciones del combate
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
                        CheckLevelUp(player);

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
                        StartCoroutine(SendScoreToServer(player.stats.lvl-1));
                        Debug.Log("El jugador ha perdido la batalla.");
                        int score = Mathf.Max(0, player.stats.lvl - 1);
                        ShowGameOver(score);
                        StartCoroutine(SendScoreToServer(score));


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
                    this.fighterIndex = (this.fighterIndex + 1) % this.fighters.Count;


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
    //Agrega un nuevo luchador
    public void AddFighter(Fighter newFighter)
    {
        this.fighters.Add(newFighter); 
        newFighter.combatManager = this;  // Asignamos el CombatManager al nuevo luchador
    }
    //Quita un luchador
    public void RemoveFighter(Fighter fighterToRemove)
    {
        if (this.fighters.Contains(fighterToRemove))
        {
            this.fighters.Remove(fighterToRemove); // Usamos .Remove() en lugar de convertir a lista
        }
    }

    //Optiene el luchador opuesto
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
    //Comprueba el nivel al que acaba de subir el luchador seleccionado
    public void CheckLevelUp(Fighter player)
    {
        if (player.stats.lvl % 10 == 1 && player.stats.lvl != 1) // Si es nivel 11, 21, 31...
        {
            Debug.Log("Mostrando popup de mejora...");
            ShowUpgradePopup(player); // Pasamos el jugador
        }
    }
    //Muestra el pop up con las mejoras
    void ShowUpgradePopup(Fighter player)
    {
        UpgradePopup.Instance.Show(player, OnUpgradeSelected);
        Debug.Log($"player preupgrade hp:{player.stats.maxHealth},  player attack: {player.stats.attack}, player defense: {player.stats.defense}");
    }
    // Según la mejora realiza una acción
    void OnUpgradeSelected(Fighter player, UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.Health:
               float extraHp=
                player.stats.maxHealth = Mathf.CeilToInt(player.stats.maxHealth * 1.10f);
                player.stats.health += extraHp;
                break;
            case UpgradeType.Attack:
                player.stats.attack = Mathf.CeilToInt(player.stats.attack * 1.15f);
                break;
            case UpgradeType.Defense:
                player.stats.defense = Mathf.CeilToInt(player.stats.defense * 1.12f);
                break;
        }

        // Actualizar el status panel para reflejar los cambios
        player.statusPanel.SetStats(player.stats, player.idName);
    }
    //Manda el Score al servidor
    private IEnumerator SendScoreToServer(int playerLvl)
    {
        string url = "https://luze0oo0.pythonanywhere.com/score/create";

        if (!PlayerPrefs.HasKey("user_id"))
        {
            Debug.LogError("No se encontró user_id en PlayerPrefs.");
            yield break;
        }

        int userId = PlayerPrefs.GetInt("user_id");
        int score = Mathf.Max(0, playerLvl);
        string jsonData = $"{{\"user_id\":{userId},\"score\":{score}}}";

        yield return StartCoroutine(ApiManager.SendPostRequest(url, jsonData, "Puntaje enviado exitosamente", "Error al enviar el puntaje"));
    }
    //Muestra la pantalla de GameOver
    private void ShowGameOver(int score)
    {
        gameOverPanel.SetActive(true);
        scoreText.text = $"Score: {score}";
        restartButton.onClick.AddListener(RestartGame);
    }
    //Acaba la partida forzadamente
    public void ForceGameOver(int score)
    {
        this.isCombatActive = false;
        LogPanel.Write("Has perdido por rendición.");
        ShowGameOver(score);
        StartCoroutine(SendScoreToServer(score));
        MusicManager1.Instance.PlayMusic(MusicManager1.Instance.gameOverMusic);

    }

    //Reinicia el juego
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
