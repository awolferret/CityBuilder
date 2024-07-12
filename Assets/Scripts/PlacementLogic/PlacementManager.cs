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

        private Dictionary<Vector3Int, StructureModel> _structures =
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
            else if (_structures.ContainsKey(position))
                _structures[position].SwapModel(newModel, rotation);
        }

        private bool CheckPositionOfType(Vector3Int position, CellType type) =>
            _placemendGrid[position.x, position.z] == type;

        public CellType[] GetNeighbourTypresFor(Vector3Int position) =>
            _placemendGrid.GetAllAdjacentCellTypes(position.x, position.z);

        public List<Vector3Int> GetNeighbourOfTypeFor(Vector3Int position, CellType cellType)
        {
            List<Point> neighbourVertices = _placemendGrid.GetAdjacentCellsOfType(position.x, position.z, cellType);
            List<Vector3Int> neighbours = new List<Vector3Int>();

            foreach (Point point in neighbourVertices)
                neighbours.Add(new Vector3Int(point.X, 0, point.Y));

            return neighbours;
        }

        public void RemoveAllTempStructures()
        {
            foreach (StructureModel structure in _temporaryRoadObjects.Values)
            {
                Vector3Int position = Vector3Int.RoundToInt(structure.transform.position);
                _placemendGrid[position.x, position.z] = CellType.Empty;
                Destroy(structure.gameObject);
            }

            _temporaryRoadObjects.Clear();
        }

        public List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)
        {
            List<Point> resultPath = GridSearch.AStarSearch(_placemendGrid, new Point(startPosition.x, startPosition.z),
                new Point(endPosition.x, endPosition.z));

            List<Vector3Int> path = new List<Vector3Int>();

            foreach (Point point in resultPath)
                path.Add(new Vector3Int(point.X, 0, point.Y));

            return path;
        }

        public void AddToStructures()
        {
            foreach (var obj in _temporaryRoadObjects)
            {
                _structures.Add(obj.Key, obj.Value);
                DestroyNature(obj.Key);
            }

            _temporaryRoadObjects.Clear();
        }

        public void PlaceObject(Vector3Int position, GameObject prefab, CellType cellType)
        {
            _placemendGrid[position.x, position.z] = cellType;
            StructureModel structureModel = CreateNewStructureModel(position, prefab, cellType);
            _structures.Add(position, structureModel);
            DestroyNature(position);
        }
        
        public void PlaceObject(Vector3Int position, GameObject prefab, CellType cellType,int width, int height)
        {
            StructureModel structureModel = CreateNewStructureModel(position, prefab, cellType);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    var newPosition = position + new Vector3Int(x, 0, z);
                    _placemendGrid[newPosition.x, newPosition.z] = cellType;
                    _structures.Add(newPosition, structureModel);
                    DestroyNature(newPosition);
                }
            }
        }

        private void DestroyNature(Vector3Int position)
        {
            RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0),
                new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 1f,
                1 << LayerMask.NameToLayer("Nature"));

            foreach (RaycastHit item in hits)
                Destroy(item.collider.gameObject);
        }

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
    }
}