using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour {
    private GameObject tower;
    private BuildingData building;

    public bool isDisabled;
    private bool isUpgraded;

    public void OnMouseOver() {
        if (isDisabled) {
            return;
        }
        
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

        tower = Build(building.prefab);
    }

    public void OnBuildingUpgrade() {
        if (BuildManager.Instance.IsEnough(building.upgradeCost)) {
            isUpgraded = true;
            BuildManager.Instance.ChangeMoney(-building.upgradeCost);
            Destroy(tower);
            tower = Build(building.upgradedPrefab);
        }
    }

    public void OnBuildingDestroy() {
        if (building != null && tower != null) {
            // return 70% of spent money
            var cashback = (int)((float)building.cost * 0.7);
            BuildManager.Instance.ChangeMoney(cashback);
            Destroy(tower);
        }
        
        building = null;
        tower = null;
        isUpgraded = false;
    }

    private GameObject Build(GameObject prefab) {
        return Instantiate(prefab, transform.position + Vector3.up * (float)0.5, Quaternion.identity);
    }
}