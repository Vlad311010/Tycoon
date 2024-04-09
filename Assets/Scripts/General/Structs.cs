using System;
using System.Collections.Generic;
using System.Linq;
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
    public struct GridData
    {
        public int id;
        public bool blocked;

        public GridData(GridBlocker blocker)
        {
            blocked = blocker.blocked;
            // id = blocker.GetComponent<UniqueID>().ID;
            id = blocker.ID;
        }

        public override bool Equals(object obj)
        {
            GridData? item = obj as GridData?;

            if (item == null)
            {
                return false;
            }

            return this.id.Equals(item.Value.id);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

    }


    [Serializable]
    public struct GoodsContainerData
    {
        public string prefabName;
        public int gridId;
        public Vector2Int coordinates;
        public Quaternion rotation;

        public GoodsContainerData(GoodsContainer goodsContainer)
        {
            prefabName = goodsContainer.ObjectData.prefab.name;
            gridId = goodsContainer.ConnectedGrid.gridId;
            coordinates = goodsContainer.GridCoordinates;
            rotation = goodsContainer.transform.rotation;
        }

        public void Load()
        {
            GameObject placablePrefab = (GameObject)Resources.Load("Placable/" + prefabName);
            int id = this.gridId;
            BuildingGrid connectedGrid = GameObject.FindObjectsOfType<GridBlocker>(true).Where(gb => gb.ID == id).Single().Grid;
            connectedGrid.PlaceObjectAnew(placablePrefab.GetComponent<GoodsContainer>().ObjectData, coordinates, rotation.eulerAngles.y);
            // GameObject.Instantiate(placablePrefab, position, rotation);
        }
    }

    [Serializable]
    public class SettingsData
    {
        public int cameraMovementSpeed;
        public int cameraRotationSpeed;

        public SettingsData()
        {
            cameraMovementSpeed = 20;
            cameraRotationSpeed = 120;
        }

        public SettingsData(SettingsData settings)
        {
            cameraMovementSpeed = settings.cameraMovementSpeed;
            cameraRotationSpeed = settings.cameraRotationSpeed;
        }
    }



    [Serializable]
    public class PersistentData
    {
        public SettingsData settings;
        public int money;
        public int cleaningTools;
        public List<GoodsContainerData> containers;
        public List<GridData> grids;
        
        public PersistentData()
        {
            settings = new SettingsData();
            money = 500;
            cleaningTools = 2;
            containers = new List<GoodsContainerData>();
            grids = new List<GridData>();
        }

        public PersistentData(SettingsData settings)
        {
            this.settings = new SettingsData(settings);
            money = 500;
            cleaningTools = 2;
            containers = new List<GoodsContainerData>();
            grids = new List<GridData>();
        }

        public int CameraMovementSpeed { get => settings.cameraMovementSpeed; set => settings.cameraMovementSpeed = value; }
        public int CameraRotationSpeed { get => settings.cameraRotationSpeed; set => settings.cameraRotationSpeed = value; }

    }

}