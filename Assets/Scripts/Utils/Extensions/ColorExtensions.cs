﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Oasez.Extensions.Color {

    public static class ColorExtensions {

        /// <summary>
        /// Lerps color of the given image component within speed (seconds)
        /// </summary>
        /// <param name="currentImage">Image with the color variable</param>
        /// <param name="targetColor">End color of the lerp</param>
        /// <param name="time">Time in seconds</param>
        /// <param name="owner">Class to run the lerp on</param>
        /// <returns></returns>
        public static Image LerpColor(this Image currentImage, UnityEngine.Color targetColor, float time, MonoBehaviour owner) {
            if (currentImage == null) return null;

            if (owner != null) {
                owner.StartCoroutine(ExtensionHelpers.LerpColor(currentImage, targetColor, time));
                return currentImage;
            } else {
                Debug.LogWarning("Our Owner is null");
                return null;
            }
        }


        /// <summary>
        /// Lerps color of the given textmeshprougui component within speed (seconds)
        /// </summary>
        /// <param name="currentImage">Image with the color variable</param>
        /// <param name="targetColor">End color of the lerp</param>
        /// <param name="time">Time in seconds</param>
        /// <param name="owner">Class to run the lerp on</param>
        /// <returns></returns>
        public static TextMeshProUGUI LerpColor(this TextMeshProUGUI currentImage, UnityEngine.Color targetColor, float time, MonoBehaviour owner) {
            if (currentImage == null) return null;

            if (owner != null) {
                owner.StartCoroutine(ExtensionHelpers.LerpColor(currentImage, targetColor, time));
                return currentImage;
            } else {
                Debug.Log("Our Owner is null");
                return null;
            }
        }

        /// <summary>
        /// Returns the color is HSL variables
        /// </summary>
        /// <param name="color">RGB color</param>
        /// <param name="H">Out Hue</param>
        /// <param name="S">Out Saturation</param>
        /// <param name="L">Out Lightness</param>
        public static void ToHSL(this UnityEngine.Color color, out float H, out float S, out float L) {
            float _Min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
            float _Max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
            float _Delta = _Max - _Min;

            H = 0;
            S = 0;
            L = (float)((_Max + _Min) / 2.0f);

            if (_Delta != 0) {
                if (L < 0.5f) {
                    S = (float)(_Delta / (_Max + _Min));
                } else {
                    S = (float)(_Delta / (2.0f - _Max - _Min));
                }

                if (color.r == _Max) {
                    H = (color.g - color.b) / _Delta;
                } else if (color.g == _Max) {
                    H = 2f + (color.b - color.r) / _Delta;
                } else if (color.b == _Max) {
                    H = 4f + (color.r - color.g) / _Delta;
                }
            }
        }

    }

}