using System;
using System.Linq;
using GridLogic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlacementLogic.Buildings
{
    public class StructureManager : MonoBehaviour
    {
        [SerializeField] private StruckturePrefabWeighted[] _housesPrefabs;
        [SerializeField] private StruckturePrefabWeighted[] _specialPrefabs;
        [SerializeField] private PlacementManager _placementManager;

        private float[] _houseWieghts;
        private float[] _specialWieghts;

        private void Start()
        {
            _houseWieghts = _housesPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
            _specialWieghts = _specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        }

        public void PlaceHouse(Vector3Int position)
        {
            if (CheckPosition(position))
            {
                int randomIndex = GetRandomWightedIndex(_houseWieghts);
                _placementManager.PlaceObject(position, _housesPrefabs[randomIndex].prefab, CellType.Structure);
            }
        }
        
        public void PlaceSpecial(Vector3Int position)
        {
            if (CheckPosition(position))
            {
                int randomIndex = GetRandomWightedIndex(_specialWieghts);
                _placementManager.PlaceObject(position, _specialPrefabs[randomIndex].prefab, CellType.Structure);
            }
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
            if (_placementManager.CheckPositionBound(position) == false)
                return false;

            if (_placementManager.CheckPositionFree(position) == false)
                return false;

            if (_placementManager.GetNeighbourOfTypreFor(position, CellType.Road).Count <= 0)
                return false;

            return true;
        }
    }

    [Serializable]
    public struct StruckturePrefabWeighted
    {
        public GameObject prefab;
        [Range(0, 1)] public float weight;
    }
}