using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Structs
{
    public struct PlacedObject
    {
        public Transform instanceRef;
        public Vector2Int origin;
        public float rotation;
        public PlaceableSO placeableSO;

        public PlacedObject(Transform instance, Vector2Int origin, float rotation, PlaceableSO so)
        {
            instanceRef = instance;
            this.origin = origin;
            this.rotation = rotation;
            this.placeableSO = so;
        }

        public override string ToString()
        {
            return placeableSO.name;
        }

    }
    
    [Serializable]
    public struct GoodsContainerData
    {
        public string prefabName;
        public string gridName;
        public Vector2Int coordinates;
        public Quaternion rotation;

        public GoodsContainerData(GoodsContainer goodsContainer)
        {
            prefabName = goodsContainer.ObjectData.prefab.name;
            gridName = goodsContainer.ConnectedGrid.gameObject.name;
            coordinates = goodsContainer.GridCoordinates;
            rotation = goodsContainer.transform.rotation;
        }

        public void Load()
        {
            GameObject placablePrefab = (GameObject)Resources.Load("Placable/" + prefabName);
            BuildingGrid connectedGrid = GameObject.Find(gridName).GetComponent<BuildingGrid>();
            connectedGrid.PlaceObjectAnew(placablePrefab.GetComponent<GoodsContainer>().ObjectData, coordinates, rotation.eulerAngles.y);
            // GameObject.Instantiate(placablePrefab, position, rotation);
        }
    }


    [Serializable]
    public class PersistentData
    {
        public int money;
        public List<GoodsContainerData> containers;
        
        public PersistentData()
        {
            money = 500;
            containers = new List<GoodsContainerData>();
        }

    }

}