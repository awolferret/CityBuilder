using GridLogic;
using UnityEngine;

namespace PlacementLogic
{
    public class StructureModel : MonoBehaviour
    {
        private float _yHeight = 0;

        [field: SerializeField] public CellType Type { get; private set; }
        [field: SerializeField] public int BuildingPrefabIndex { get; private set; }

        public void CreateModel(GameObject model, int buildingIndex, CellType type)
        {
            GameObject newModel = Instantiate(model, transform);
            _yHeight = newModel.transform.position.y;
            Type = type;
            BuildingPrefabIndex = buildingIndex;
        }

        public void SwapModel(GameObject model, Quaternion rotation)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            CreateNewModel(model, rotation);
        }

        private void CreateNewModel(GameObject model, Quaternion rotation)
        {
            GameObject structure = Instantiate(model, transform);
            structure.transform.localPosition = new Vector3(0, _yHeight, 0);
            structure.transform.localRotation = rotation;
        }
    }
}