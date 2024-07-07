using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputLogic
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _layerMask;

        private Vector2 _cameraMovement;

        public Action<Vector3Int> OnMouseClick;
        public Action<Vector3Int> OnMouseHold;
        public Action OnMouseUp;

        public Vector2 CameraMovement => _cameraMovement;
        public LayerMask LayerMask => _layerMask;

        private void Update()
        {
            CheckClick();
            CheckUp();
            CheckHold();
            CheckArrowInput();
        }

        private Vector3Int? RaycastGround()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
                return Vector3Int.RoundToInt(hit.point);
            else
                return null;
        }

        private void CheckArrowInput() =>
            _cameraMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        private void CheckHold()
        {
            if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Vector3Int? position = RaycastGround();

                if (position != null)
                    OnMouseHold?.Invoke(position.Value);
            }
        }

        private void CheckUp()
        {
            if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
                OnMouseUp?.Invoke();
        }

        private void CheckClick()
        {
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Vector3Int? position = RaycastGround();

                if (position != null)
                    OnMouseClick?.Invoke(position.Value);
            }
        }
    }
}