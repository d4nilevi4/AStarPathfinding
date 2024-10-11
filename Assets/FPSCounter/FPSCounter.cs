using TMPro;
using UnityEngine;

namespace FPSCounter
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _averageText;

        private int _frameRange = 120;
        private int[] _fpsBuffer;
        private int _fpsBufferIndex;

        private void Update()
        {
            if (_fpsBuffer == null || _frameRange != _fpsBuffer.Length)
            {
                InitializeBuffer();
            }
            
            UpdateBuffer();
            CalculateFps();
        }

        private void InitializeBuffer()
        {
            if (_frameRange <= 0)
            {
                _frameRange = 1;
            }
            
            _fpsBuffer = new int[_frameRange];
            _fpsBufferIndex = 0;
        }

        private void UpdateBuffer()
        {
            _fpsBuffer[_fpsBufferIndex++] = (int) (1f / Time.unscaledDeltaTime);

            if (_fpsBufferIndex >= _frameRange)
            {
                _fpsBufferIndex = 0;
            }
        }

        private void CalculateFps()
        {
            int sum = 0;
            for (int i = 0; i < _frameRange; i++)
            {
                sum += _fpsBuffer[i];
            }

            _averageText.text = $"{sum/_frameRange}";
        }
    }
}
