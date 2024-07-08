using System.Collections.Generic;
using GridLogic;
using UnityEngine;
using Grid = GridLogic.Grid;

namespace PlacementLogic
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        private Grid _placemendGrid;

        private Dictionary<Vector3Int, StructureModel> _temporaryRoadObjects =
            new Dictionary<Vector3Int, StructureModel>();

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

        public void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType cellType)
        {
            _placemendGrid[position.x, position.z] = cellType;
            StructureModel structureModel = CreateNewStructureModel(position, structurePrefab, cellType);
            _temporaryRoadObjects.Add(position, structureModel);
        }

        public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
        {
            if (_temporaryRoadObjects.ContainsKey(position))
                _temporaryRoadObjects[position].SwapModel(newModel, rotation);
        }

        private bool CheckPositionOfType(Vector3Int position, CellType type) =>
            _placemendGrid[position.x, position.z] == type;

        private StructureModel CreateNewStructureModel(Vector3Int position, GameObject structurePrefab,
            CellType cellType)
        {
            GameObject structure = new GameObject(cellType.ToString());
            structure.transform.SetParent(transform);
            structure.transform.localPosition = position;
            StructureModel structureModel = structure.AddComponent<StructureModel>();
            structureModel.CreateModel(structurePrefab);
            return structureModel;
        }

        public CellType[] GetNeighbourTypresFor(Vector3Int position) =>
            _placemendGrid.GetAllAdjacentCellTypes(position.x, position.z);

        public List<Vector3Int> GetNeighbourOfTypreFor(Vector3Int position, CellType cellType)
        {
            List<Point> neighbourVertices = _placemendGrid.GetAdjacentCellsOfType(position.x, position.z, cellType);
            List<Vector3Int> neighbours = new List<Vector3Int>();

            foreach (Point point in neighbourVertices) 
                neighbours.Add(new Vector3Int(point.X, 0, point.Y));

            return neighbours;
        }
    }
}