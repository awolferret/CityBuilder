using UnityEngine;

namespace CameraLogic
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _cameraMovementSpeed = 5;
        
        private Camera _gameCamera;

        private void Start() =>
            _gameCamera = Camera.main;

        public void MoveCamera(Vector3 inputVector)
        {
            var movementVector = Quaternion.Euler(0, 30, 0) * inputVector;
            _gameCamera.transform.position += movementVector * Time.deltaTime * _cameraMovementSpeed;
        }
    }
}