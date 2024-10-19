using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildManager : MonoBehaviour {
    public static BuildManager Instance { get; private set; }

    [SerializeField] private BuildingDataBase towers;

    public TextMeshProUGUI moneyText;
    private Animator moneyTextAnim;
    public UpgradeUI upgradeUI;
    public DeleteUI deleteUI;

    public int money = 200;
    private MapCube activeCube;
    private BuildingData selectedBuilding;

    private void Awake() {
        Instance = this;
        moneyTextAnim = moneyText.GetComponent<Animator>();
    }

    public BuildingData CurrentBuilding() {
        return selectedBuilding;
    }

    public void OnTowerSelected(int towerId) {
        var i = towers.buildings.FindIndex(data => data.id == towerId);
        if (i > -1) {
            selectedBuilding = towers.buildings[i];
        }
        else
            throw new System.Exception($"No object with ID {towerId}");
    }

    public bool IsEnough(int need) {
        if (need <= money) {
            return true;
        }

        MoneyFlicker();
        return false;
    }

    public void ChangeMoney(int value) {
        money += value;
        moneyText.text = "$" + money;
    }

    private void MoneyFlicker() {
        moneyTextAnim.SetTrigger("flicker");
    }

    public void ShowUpgradeUI(MapCube cube, Vector3 position) {
        activeCube = cube;
        deleteUI.Hide();
        upgradeUI.Show(position);
    }

    public void ShowDeleteUI(MapCube cube, Vector3 position) {
        activeCube = cube;
        upgradeUI.Hide();
        deleteUI.Show(position);
    }

    public void OnTurretUpgrade() {
        activeCube?.OnBuildingUpgrade();
    }

    public void OnTurretDestroy() {
        activeCube?.OnBuildingDestroy();
    }
}