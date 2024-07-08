using System.Collections.Generic;
using GridLogic;
using UnityEngine;

namespace PlacementLogic
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private PlacementManager _placementManager;
        [SerializeField] private GameObject _roadStraight;
        [SerializeField] private RoadFixer _roadFixer;

        private List<Vector3Int> _temporaryPlacementPosition = new List<Vector3Int>();
        private List<Vector3Int> _roadPositionToRecheck = new List<Vector3Int>();

        public void PlaceRoad(Vector3Int position)
        {
            if (_placementManager.CheckPositionBound(position) == false)
                return;

            if (_placementManager.CheckPositionFree(position) == false)
                return;

            _temporaryPlacementPosition.Clear();
            _temporaryPlacementPosition.Add(position);
            _placementManager.PlaceTemporaryStructure(position, _roadStraight, CellType.Road);
            FixRoadPrefabs();
        }

        private void FixRoadPrefabs()
        {
            foreach (Vector3Int tempPosition in _temporaryPlacementPosition)
            {
                _roadFixer.FixRoadAtPosition(_placementManager, tempPosition);
                List<Vector3Int> neighbours = _placementManager.GetNeighbourOfTypreFor(tempPosition, CellType.Road);

                foreach (var roadPosition in neighbours)
                    _roadPositionToRecheck.Add(roadPosition);
            }

            foreach (var posToFix in _roadPositionToRecheck) 
                _roadFixer.FixRoadAtPosition(_placementManager, posToFix);
        }
    }
}