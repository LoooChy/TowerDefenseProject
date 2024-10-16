using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BuildingDataBase : ScriptableObject {
    public List<BuildingData> buildings;
}

[Serializable]
public class BuildingData {
    [field: SerializeField] public string name;
    [field: SerializeField] public int id;
    [field: SerializeField] public int cost;
    [field: SerializeField] public int upgradeCost;
    [field: SerializeField] public GameObject prefab;
    [field: SerializeField] public GameObject upgradedPrefab;
}