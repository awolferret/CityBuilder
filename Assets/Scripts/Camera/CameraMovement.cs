using UnityEngine;

namespace Camera
{
    public class CameraMovement : MonoBehaviour
    {
        public UnityEngine.Camera gameCamera;
        public float cameraMovementSpeed = 5;

        private void Start() => 
            gameCamera = GetComponent<UnityEngine.Camera>();

        public void MoveCamera(Vector3 inputVector)
        {
            var movementVector = Quaternion.Euler(0,30,0) * inputVector;
            gameCamera.transform.position += movementVector * Time.deltaTime * cameraMovementSpeed;
        }
    }
}