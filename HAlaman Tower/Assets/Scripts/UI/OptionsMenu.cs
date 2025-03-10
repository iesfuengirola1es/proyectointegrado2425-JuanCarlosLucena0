using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel; // Panel de opciones
    public GameObject scoresPanel;  // Panel de los scores
    public GameObject statsPanel;   // Panel de estadísticas
    public TMP_Text scoresText;         // Texto donde se mostrarán los scores
    public TMP_Text statsText;          // Texto donde se mostrarán las stats

    public Player player;           // Referencia al jugador
    public CombatManager combatManager; // Para rendirse

    private void Start()
    {
        optionsPanel.SetActive(false);
        scoresPanel.SetActive(false);
        statsPanel.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void ShowMyScores()
    {
        StartCoroutine(GetUserScores());
    }

    public void ShowTopScores()
    {
        StartCoroutine(GetTopScores());
    }
    //Aceptar derrota
    public void Surrender()
    {
        int score = Mathf.Max(0, player.stats.lvl - 1);
        optionsPanel.SetActive(false);
        combatManager.ForceGameOver(score); // Llamamos a la función que maneja la derrota
    }
    //Mostrar stats
    public void ShowStats()
    {
        statsPanel.SetActive(true);
        statsText.text = $"Nivel: {player.stats.lvl}\nVida: {player.stats.health}\nFuerza: {player.stats.attack}\nDefensa: {player.stats.defense}";
    }
    //Cerrar stats
    public void CloseStats()
    {
        statsPanel.SetActive(false);
    }
    //Ir al menú
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("LoginScene");
    }
    //Cerrar panel de scores
    public void CloseScoresPanel()
    {
        scoresPanel.SetActive(false);
    }
    //Coger scores del usuario
    private IEnumerator GetUserScores()
    {
        string url = $"https://luze0oo0.pythonanywhere.com/score/user/{PlayerPrefs.GetInt("user_id")}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (scoresPanel == null || scoresText == null)
                {
                    Debug.LogError("scoresPanel o scoresText no están asignados en el Inspector.");
                    yield break;
                }

                scoresPanel.SetActive(true);

                string json = request.downloadHandler.text;
                ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);

                scoresText.text = "";
                foreach (var entry in scoreList.scores)
                {
                    scoresText.text += $"Score: {entry.score} Usuario: {entry.username}\n";
                }
            }
            else
            {
                Debug.LogError("Error al obtener los scores del usuario: " + request.error);
            }
        }

    }

    private IEnumerator GetTopScores()
    {
        string url = "https://luze0oo0.pythonanywhere.com/scores/top";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Convertir JSON a objetos de C#
                ScoreList scoreList = JsonUtility.FromJson<ScoreList>("{\"scores\":" + request.downloadHandler.text + "}");

                // Crear el texto a mostrar
                scoresText.text = "";
                foreach (var entry in scoreList.scores)
                {
                    scoresText.text += $"Score: {entry.score} Usuario: {entry.username}\n";
                }

                scoresPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Error al obtener los mejores scores: " + request.error);
            }
        }
    }

}


