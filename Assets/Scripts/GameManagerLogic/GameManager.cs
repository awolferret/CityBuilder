using CameraLogic;
using InputLogic;
using RoadLogic;
using UnityEngine;

namespace GameManagerLogic
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private RoadManager _roadManager;

        private void OnEnable() => 
            _inputManager.OnMouseClick += HandleMouseClick;

        private void OnDisable() => 
            _inputManager.OnMouseClick -= HandleMouseClick;

        private void HandleMouseClick(Vector3Int position)
        {
            Debug.Log(position);
            _roadManager.PlaceRoad(position);
        }

        private void Update()
        {
            MoveCamera();
        }

        private void MoveCamera() => 
            _cameraMovement.MoveCamera(new Vector3(_inputManager.CameraMovement.x, 0, _inputManager.CameraMovement.y));
    }
}