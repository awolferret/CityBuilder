using UnityEngine;

namespace PlacementLogic
{
    public class StructureModel : MonoBehaviour
    {
        private float _yHeight = 0;

        public void CreateModel(GameObject model)
        {
            GameObject newModel = Instantiate(model, transform);
            _yHeight = newModel.transform.position.y;
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