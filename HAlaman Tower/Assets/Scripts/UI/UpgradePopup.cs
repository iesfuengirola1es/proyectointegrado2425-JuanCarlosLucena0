using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public enum UpgradeType
{
    Health,
    Attack,
    Defense
}

public class UpgradePopup : MonoBehaviour
{
    public static UpgradePopup Instance { get; private set; }

    public Button healthButton;
    public Button attackButton;
    public Button defenseButton;
    private System.Action<Fighter, UpgradeType> onUpgradeSelected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameObject.SetActive(false); // Oculta el popup al iniciar
    }

    public void Show(Fighter player, System.Action<Fighter, UpgradeType> callback)
    {
        gameObject.SetActive(true); // Muestra el popup cuando sea necesario
        onUpgradeSelected = callback;

        healthButton.onClick.RemoveAllListeners();
        attackButton.onClick.RemoveAllListeners();
        defenseButton.onClick.RemoveAllListeners();

        healthButton.onClick.AddListener(() => SelectUpgrade(player, UpgradeType.Health));
        attackButton.onClick.AddListener(() => SelectUpgrade(player, UpgradeType.Attack));
        defenseButton.onClick.AddListener(() => SelectUpgrade(player, UpgradeType.Defense));
    }

    private void SelectUpgrade(Fighter player, UpgradeType upgrade)
    {
        onUpgradeSelected?.Invoke(player, upgrade);
        gameObject.SetActive(false); // Oculta el popup después de seleccionar una mejora
        Debug.Log($"Post upgrades: player hp:{player.stats.maxHealth},  player attack: {player.stats.attack}, player defense: {player.stats.defense}");

    }

}



