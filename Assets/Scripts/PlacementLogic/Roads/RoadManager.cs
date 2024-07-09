using System.Collections.Generic;
using GridLogic;
using UnityEngine;

namespace PlacementLogic.Roads
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private PlacementManager _placementManager;
        [SerializeField] private RoadFixer _roadFixer;

        private Vector3Int _startPosition;
        private bool _isPlacing = false;
        private List<Vector3Int> _temporaryPlacementPosition = new List<Vector3Int>();
        private List<Vector3Int> _roadPositionToRecheck = new List<Vector3Int>();

        public void PlaceRoad(Vector3Int position)
        {
            if (_placementManager.CheckPositionBound(position) == false)
                return;

            if (CheckFreePosition(position))
                return;

            if (!_isPlacing)
            {
                _isPlacing = true;
                _startPosition = position;
                ClearLists();
                _temporaryPlacementPosition.Add(position);
                _placementManager.PlaceTemporaryStructure(position, _roadFixer.BaseRoad, CellType.Road);
            }
            else
            {
                _placementManager.RemoveAllTempStructures();
                FixRoadBack();
                ClearLists();
                _temporaryPlacementPosition = _placementManager.GetPathBetween(_startPosition, position);

                foreach (Vector3Int tempPos in _temporaryPlacementPosition)
                {
                    if (CheckFreePosition(tempPos))
                        continue;

                    _placementManager.PlaceTemporaryStructure(tempPos, _roadFixer.BaseRoad, CellType.Road);
                }
            }

            FixRoadPrefabs();
        }

        private void FixRoadBack()
        {
            foreach (Vector3Int posToFix in _roadPositionToRecheck) 
                _roadFixer.FixRoadAtPosition(_placementManager, posToFix);
        }

        private bool CheckFreePosition(Vector3Int position)
        {
            if (_placementManager.CheckPositionFree(position) == false)
                return true;
            return false;
        }

        public void FinishPlaceRoad()
        {
            _isPlacing = false;
            _placementManager.AddToStructures();
            _temporaryPlacementPosition.Clear();
            _startPosition = Vector3Int.zero;
        }

        private void ClearLists()
        {
            _temporaryPlacementPosition.Clear();
            _roadPositionToRecheck.Clear();
        }

        private void FixRoadPrefabs()
        {
            foreach (Vector3Int tempPosition in _temporaryPlacementPosition)
            {
                _roadFixer.FixRoadAtPosition(_placementManager, tempPosition);
                List<Vector3Int> neighbours = _placementManager.GetNeighbourOfTypreFor(tempPosition, CellType.Road);

                foreach (var roadPosition in neighbours)
                    if (_roadPositionToRecheck.Contains(roadPosition) == false)
                        _roadPositionToRecheck.Add(roadPosition);
            }

            foreach (var posToFix in _roadPositionToRecheck)
                _roadFixer.FixRoadAtPosition(_placementManager, posToFix);
        }
    }
}