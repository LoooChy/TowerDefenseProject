using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour {
    private GameObject tower;
    private BuildingData building;
    
    public GameObject buildEffect;

    private bool isUpgraded;

    public void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (tower) {
                if (isUpgraded) {
                    return;
                }
                
                var position = new Vector3(transform.position.x, tower.transform.lossyScale.y,
                    transform.position.z - 2);
                BuildManager.Instance.ShowUpgradeUI(this, position);
            }
            else {
                BuildTower();
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            if (tower) {
                var position = new Vector3(transform.position.x, tower.transform.lossyScale.y,
                    transform.position.z - 2);
                BuildManager.Instance.ShowDeleteUI(this, position);
            }
        }
    }
    
    private void BuildTower() {
        building = BuildManager.Instance.CurrentBuilding();
        if (building == null || !building.prefab) return;

        if (BuildManager.Instance.IsEnough(building.cost) == false) {
            return;
        }

        BuildManager.Instance.ChangeMoney(-building.cost);

        tower = InstantiateTurret(building.prefab);
    }

    public void OnTurretUpgrade() {
        if (BuildManager.Instance.IsEnough(building.upgradeCost)) {
            isUpgraded = true;
            BuildManager.Instance.ChangeMoney(-building.upgradeCost);
            Destroy(tower);
            tower = InstantiateTurret(building.upgradedPrefab);
        }
    }

    public void OnTurretDestroy() {
        Destroy(tower);
        building = null;
        tower = null;
        isUpgraded = false;
        GameObject go = Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(go, 2);
    }

    private GameObject InstantiateTurret(GameObject prefab) {
        GameObject turretGo = Instantiate(prefab, transform.position + Vector3.up * (float)0.5, Quaternion.identity);
        GameObject go = Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(go, 2);
        return turretGo;
    }
}