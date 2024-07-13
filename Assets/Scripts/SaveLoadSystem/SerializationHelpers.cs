using System;
using System.Collections.Generic;
using GridLogic;
using UnityEngine;

namespace SaveLoadSystem
{
    [Serializable]
    public class SaveDataSerialization
    {
        public List<BuildingDataSerialization> structuresData = new List<BuildingDataSerialization>();

        public void AddStructureData(Vector3Int position, int buildingIndex, CellType buildingType) => 
            structuresData.Add(new BuildingDataSerialization(position, buildingIndex, buildingType));
    }

    [Serializable]
    public class Vector3Serialization
    {
        public float x;
        public float y;
        public float z;

        public Vector3Serialization(Vector3 position)
        {
            this.x = position.x;
            this.y = position.y;
            this.z = position.z;
        }

        public Vector3 GetValue()
        {
            return new Vector3(x, y, z);
        }
    }

    [Serializable]
    public class BuildingDataSerialization
    {
        public Vector3Serialization position;
        public int buildingIndex;
        public CellType buildingType;

        public BuildingDataSerialization(Vector3Int position, int buildingIndex, CellType buildingType)
        {
            this.position = new Vector3Serialization(position);
            this.buildingIndex = buildingIndex;
            this.buildingType = buildingType;
        }
    }
}