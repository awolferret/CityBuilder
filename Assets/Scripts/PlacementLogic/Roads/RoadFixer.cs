using System.Linq;
using GridLogic;
using UnityEngine;

namespace PlacementLogic.Roads
{
    public class RoadFixer : MonoBehaviour
    {
        [SerializeField] private GameObject _deadEnd;
        [SerializeField] private GameObject _straight;
        [SerializeField] private GameObject _corner;
        [SerializeField] private GameObject _threeWay;
        [SerializeField] private GameObject _fourWay;

        public GameObject BaseRoad => _deadEnd;
        
        public void FixRoadAtPosition(PlacementManager placementManager, Vector3Int tempPosition)
        {
            var result = placementManager.GetNeighbourTypresFor(tempPosition);
            int roadCount = 0;
            roadCount = result.Where(x => x == CellType.Road).Count();

            CheckNeigbourNumber(placementManager, tempPosition, roadCount, result);
        }

        private void CheckNeigbourNumber(PlacementManager placementManager, Vector3Int tempPosition, int roadCount,
            CellType[] result)
        {
            switch (roadCount)
            {
                case 0:
                    CreateDeadEnd(placementManager, result, tempPosition);
                    break;
                case 1:
                    CreateDeadEnd(placementManager, result, tempPosition);
                    break;
                case 2:
                    TryHandleTwoHeighbours(placementManager, tempPosition, result);
                    break;
                case 3:
                    CreateThreeWay(placementManager, result, tempPosition);
                    break;
                case 4:
                    CreateFourWay(placementManager, result, tempPosition);
                    break;
            }
        }

        private void TryHandleTwoHeighbours(PlacementManager placementManager, Vector3Int tempPosition,
            CellType[] result)
        {
            if (CreateStraightRoad(placementManager, result, tempPosition))
                return;

            CreateCorner(placementManager, result, tempPosition);
        }

        private void CreateFourWay(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition) =>
            placementManager.ModifyStructureModel(tempPosition, _fourWay, Quaternion.identity);

        private void CreateThreeWay(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
        {
            if (result[1] == CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _threeWay, Quaternion.identity);
            else if (result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _threeWay, Quaternion.Euler(0, 90, 0));
            else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _threeWay, Quaternion.Euler(0, 180, 0));
            else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _threeWay, Quaternion.Euler(0, 270, 0));
        }

        private void CreateCorner(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
        {
            if (result[1] == CellType.Road && result[2] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _corner, Quaternion.Euler(0, 90, 0));
            else if (result[2] == CellType.Road && result[3] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _corner, Quaternion.Euler(0, 180, 0));
            else if (result[3] == CellType.Road && result[0] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _corner, Quaternion.Euler(0, 270, 0));
            else if (result[0] == CellType.Road && result[1] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _corner, Quaternion.identity);
        }

        private bool CreateStraightRoad(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
        {
            if (result[0] == CellType.Road && result[2] == CellType.Road)
            {
                placementManager.ModifyStructureModel(tempPosition, _straight, Quaternion.identity);
                return true;
            }
            else if (result[1] == CellType.Road && result[3] == CellType.Road)
            {
                placementManager.ModifyStructureModel(tempPosition, _straight, Quaternion.Euler(0, 90, 0));
                return true;
            }
            else
                return false;
        }

        private void CreateDeadEnd(PlacementManager placementManager, CellType[] result, Vector3Int tempPosition)
        {
            if (result[1] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _deadEnd, Quaternion.Euler(0, 270, 0));
            else if (result[2] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _deadEnd, Quaternion.identity);
            else if (result[3] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _deadEnd, Quaternion.Euler(0, 90, 0));
            else if (result[0] == CellType.Road)
                placementManager.ModifyStructureModel(tempPosition, _deadEnd, Quaternion.Euler(0, 180, 0));
        }
    }
}