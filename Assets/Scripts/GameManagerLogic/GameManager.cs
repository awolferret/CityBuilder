using System.Collections.Generic;
using CameraLogic;
using GridLogic;
using InputLogic;
using PlacementLogic;
using PlacementLogic.Buildings;
using PlacementLogic.Roads;
using SaveLoadSystem;
using UI;
using UnityEngine;

namespace GameManagerLogic
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private RoadManager _roadManager;
        [SerializeField] private UIController _uiController;
        [SerializeField] private StructureManager _structureManager;
        [SerializeField] private SaveSystem _saveSystem;

        private void OnEnable()
        {
            _uiController.OnRoadButtonClick += OnRoadClick;
            _uiController.OnBuildingButtonClick += OnBuildingClick;
            _uiController.OnSpecialButtonClick += OnSpecialClick;
            _uiController.OnBigButtonClick += OnBigClick;
        }

        private void OnDisable()
        {
            _uiController.OnRoadButtonClick -= OnRoadClick;
            _uiController.OnBuildingButtonClick -= OnBuildingClick;
            _uiController.OnSpecialButtonClick -= OnSpecialClick;
        }

        public void SaveGame()
        {
            SaveDataSerialization saveData = new SaveDataSerialization();

            foreach (KeyValuePair<Vector3Int, StructureModel> structureData in _structureManager.GetAllStructures())
                saveData.AddStructureData(structureData.Key, structureData.Value.BuildingPrefabIndex,
                    structureData.Value.Type);

            string jsonString = JsonUtility.ToJson(saveData);
            _saveSystem.SaveData(jsonString);
        }

        public void LoadData()
        {
            string jsonString = _saveSystem.LoadData();

            if (string.IsNullOrEmpty(jsonString))
                return;

            SaveDataSerialization saveData = JsonUtility.FromJson<SaveDataSerialization>(jsonString);
            _structureManager.ClearMap();
            
            foreach (BuildingDataSerialization structureData in saveData.structuresData)
            {
                Vector3Int position = Vector3Int.RoundToInt(structureData.position.GetValue());

                if (structureData.buildingType == CellType.Road)
                    LoadRoad(position);
                else
                {
                    _structureManager.PlaceLoadedStructure(position, structureData.buildingIndex,
                        structureData.buildingType);
                }
            }
        }
        
        private void LoadRoad(Vector3Int position)
        {
            _roadManager.PlaceRoad(position);
            _roadManager.FinishPlaceRoad();
        }

        private void OnBigClick()
        {
            ClearInputActions();
            _inputManager.OnMouseClick += _structureManager.PlaceBig;
        }

        private void OnSpecialClick()
        {
            ClearInputActions();
            _inputManager.OnMouseClick += _structureManager.PlaceSpecial;
        }

        private void OnBuildingClick()
        {
            ClearInputActions();
            _inputManager.OnMouseClick += _structureManager.PlaceHouse;
        }

        private void OnRoadClick()
        {
            ClearInputActions();
            _inputManager.OnMouseClick += _roadManager.PlaceRoad;
            _inputManager.OnMouseHold += _roadManager.PlaceRoad;
            _inputManager.OnMouseUp += _roadManager.FinishPlaceRoad;
        }

        private void ClearInputActions()
        {
            _inputManager.OnMouseClick = null;
            _inputManager.OnMouseHold = null;
            _inputManager.OnMouseUp = null;
        }

        private void Update() =>
            MoveCamera();

        private void MoveCamera() =>
            _cameraMovement.MoveCamera(new Vector3(_inputManager.CameraMovement.x, 0, _inputManager.CameraMovement.y));
    }
}