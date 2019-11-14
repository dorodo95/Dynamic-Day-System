using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aquiris.Pepe.Common.Util.Animation
{
    public class UIAnimationTransition : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve _textTransitionCurve;

        [SerializeField]
        private Text _targetText;

        [SerializeField]
        private AnimationCurve _imageTransitionCurve;

        [SerializeField]
        private Image _targetImage;

		[SerializeField]
		private bool direction;

        [SerializeField]
        private float _speed = 1f;

        private float _timer;
        private float _direction;
        private Color ColorTransition;
		

		[ContextMenu("Do Transition")]
        public void DoTransition()
        {
            enabled = true;
            _direction = direction ? 1f : -1f;
            _timer = direction ? 0f : 1f;
        }

        private void Update()
        {
            _timer += Time.deltaTime * _direction * _speed;
            if(_targetImage != null)
            {
                ColorTransition = _targetImage.color;
                ColorTransition.a = _imageTransitionCurve.Evaluate(_timer);
                _targetImage.color = ColorTransition;
            }
            
            if(_targetText != null)
            {
                ColorTransition = _targetText.color;
                ColorTransition.a = _textTransitionCurve.Evaluate(_timer);
                _targetText.color = ColorTransition;
            }

            if (_timer >= 1.0f)
            {
                enabled = false;
            }

            if (_timer <= -1.0f)
            {
                enabled = false;
            }

        }
    }
}