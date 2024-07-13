using System;
using System.Collections.Generic;
using System.Linq;
using GridLogic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlacementLogic.Buildings
{
    public class StructureManager : MonoBehaviour
    {
        [SerializeField] private StructurePrefabWeighted[] _housesPrefabs;
        [SerializeField] private StructurePrefabWeighted[] _specialPrefabs;
        [SerializeField] private StructurePrefabWeighted[] _bigPrefabs;
        [SerializeField] private PlacementManager _placementManager;

        private float[] _houseWieghts;
        private float[] _specialWieghts;
        private float[] _bigWieghts;

        private void Start()
        {
            _houseWieghts = _housesPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
            _specialWieghts = _specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
            _bigWieghts = _bigPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        }

        public void PlaceHouse(Vector3Int position)
        {
            if (CheckPosition(position))
            {
                int randomIndex = GetRandomWightedIndex(_houseWieghts);
                PlaceSmallStructure(position, _housesPrefabs, randomIndex, CellType.Structure);
            }
        }

        public void PlaceSpecial(Vector3Int position)
        {
            if (CheckPosition(position))
            {
                int randomIndex = GetRandomWightedIndex(_specialWieghts);
                PlaceSmallStructure(position, _specialPrefabs, randomIndex, CellType.SpecialStructure);
            }
        }

        public void PlaceBig(Vector3Int position)
        {
            int width = 2;
            int height = 2;

            if (CheckBigStructure(position, width, height))
            {
                int randomIndex = GetRandomWightedIndex(_bigWieghts);
                PlaceBigStructure(position, randomIndex);
            }
        }

        private bool CheckBigStructure(Vector3Int position, int width, int height)
        {
            bool nearRoad = false;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3Int newPosition = position + new Vector3Int(x, 0, z);

                    if (!DefaultCheck(newPosition))
                        return false;
                    if (!nearRoad)
                        nearRoad = RoadCheck(newPosition);
                }
            }

            return nearRoad;
        }

        private int GetRandomWightedIndex(float[] wieghts)
        {
            float sum = 0;

            for (int i = 0; i < wieghts.Length; i++)
            {
                sum += wieghts[i];
            }

            float randomValue = Random.Range(0, sum);
            float tempSum = 0;

            for (int i = 0; i < wieghts.Length; i++)
            {
                if (randomValue >= tempSum && randomValue < tempSum + wieghts[i])
                    return i;

                tempSum += wieghts[i];
            }

            return 0;
        }

        private bool CheckPosition(Vector3Int position)
        {
            if (!DefaultCheck(position))
                return false;

            if (!RoadCheck(position))
                return false;

            return true;
        }

        private bool RoadCheck(Vector3Int position)
        {
            if (_placementManager.GetNeighbourOfTypeFor(position, CellType.Road).Count <= 0)
                return false;

            return true;
        }

        private bool DefaultCheck(Vector3Int position)
        {
            if (_placementManager.CheckPositionBound(position) == false)
                return false;

            if (_placementManager.CheckPositionFree(position) == false)
                return false;

            return true;
        }

        public Dictionary<Vector3Int, StructureModel> GetAllStructures() =>
            _placementManager.GetAllStructures();

        public void PlaceLoadedStructure(Vector3Int position, int buildingIndex, CellType buildingType)
        {
            switch (buildingType)
            {
                case CellType.Structure:
                    PlaceSmallStructure(position, _housesPrefabs, buildingIndex, CellType.Structure);
                    break;
                case CellType.BigStructure:
                    PlaceBigStructure(position, buildingIndex);
                    break;
                case CellType.SpecialStructure:
                    PlaceSmallStructure(position, _specialPrefabs, buildingIndex, CellType.SpecialStructure);
                    break;
            }
        }

        private void PlaceBigStructure(Vector3Int position, int buildingIndex)
        {
            _placementManager.PlaceObject(position, _bigPrefabs[buildingIndex].prefab, CellType.BigStructure, 2,
                2, buildingIndex);
        }

        private void PlaceSmallStructure(Vector3Int position, StructurePrefabWeighted[] array, int buildingIndex,
            CellType type)
        {
            _placementManager.PlaceObject(position, array[buildingIndex].prefab, type,
                buildinIndex: buildingIndex);
        }


        public void ClearMap() =>
            _placementManager.ClearGrid();
    }

    [Serializable]
    public struct StructurePrefabWeighted
    {
        public GameObject prefab;
        [Range(0, 1)] public float weight;
    }
}