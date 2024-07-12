using CameraLogic;
using InputLogic;
using PlacementLogic;
using PlacementLogic.Buildings;
using PlacementLogic.Roads;
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