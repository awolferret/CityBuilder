using System.Collections.Generic;
using GridLogic;
using UnityEngine;

namespace RoadLogic
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private PlacementManager _placementManager;
        [SerializeField] private GameObject _roadStraight;

        private List<Vector3Int> _temporaryPlacementPosition = new List<Vector3Int>();

        public void PlaceRoad(Vector3Int position)
        {
            if (_placementManager.CheckPositionBound(position) == false)
                return;

            if (_placementManager.CheckPositionFree(position) == false)
                return;

            _placementManager.PlaceTemporaryStructure(position, _roadStraight, CellType.Road);
        }
    }
}