using GridLogic;
using UnityEngine;
using Grid = GridLogic.Grid;

namespace RoadLogic
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        private Grid _placemendGrid;

        private void Start() =>
            _placemendGrid = new Grid(_width, _height);

        public bool CheckPositionBound(Vector3Int position)
        {
            if (position.x >= 0 && position.x < _width && position.z >= 0 && position.z < _height)
                return true;
            else
                return false;
        }

        public bool CheckPositionFree(Vector3Int position) =>
            CheckPositionOfType(position, CellType.Empty);

        private bool CheckPositionOfType(Vector3Int position, CellType type) =>
            _placemendGrid[position.x, position.z] == type;

        public void PlaceTemporaryStructure(Vector3Int position, GameObject roadPrefab, CellType cellType)
        {
            _placemendGrid[position.x, position.z] = cellType;
            GameObject newStructure = Instantiate(roadPrefab, position, Quaternion.identity);
        }
    }
}