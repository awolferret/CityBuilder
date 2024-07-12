using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button _road;
        [SerializeField] private Button _building;
        [SerializeField] private Button _special;
        [SerializeField] private Button _big;
        [SerializeField] private Color _outlinClor;

        private List<Button> _buttons;

        public Action OnRoadButtonClick;
        public Action OnBuildingButtonClick;
        public Action OnSpecialButtonClick;
        public Action OnBigButtonClick;

        private void Start()
        {
            _buttons = new List<Button> { _road, _building, _special, _big };
        }

        private void OnEnable()
        {
            _road.onClick.AddListener(() =>
            {
                ResetButtonColor();
                ModifyOutline(_road);
                OnRoadButtonClick?.Invoke();
            });

            _building.onClick.AddListener(() =>
            {
                ResetButtonColor();
                ModifyOutline(_building);
                OnBuildingButtonClick?.Invoke();
            });

            _special.onClick.AddListener(() =>
            {
                ResetButtonColor();
                ModifyOutline(_special);
                OnSpecialButtonClick?.Invoke();
            });
            
            _big.onClick.AddListener(() =>
            {
                ResetButtonColor();
                ModifyOutline(_big);
                OnBigButtonClick?.Invoke();
            });
        }

        private void ModifyOutline(Button button)
        {
            Outline outline = button.GetComponent<Outline>();
            outline.effectColor = _outlinClor;
            outline.enabled = true;
        }

        private void ResetButtonColor()
        {
            foreach (var button in _buttons)
                button.GetComponent<Outline>().enabled = false;
        }
    }
}