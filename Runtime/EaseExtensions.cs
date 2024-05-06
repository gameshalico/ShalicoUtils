using System;
using UnityEngine;

namespace ShalicoUtils
{
    public static class EaseExtensions
    {
        public static float Evaluate(this Ease ease, float t)
        {
            return ease switch
            {
                Ease.Linear => t,
                Ease.InQuad => t * t,
                Ease.OutQuad => t * (2 - t),
                Ease.InOutQuad => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t,
                Ease.InCubic => t * t * t,
                Ease.OutCubic => --t * t * t + 1,
                Ease.InOutCubic => t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1,
                Ease.InQuart => t * t * t * t,
                Ease.OutQuart => 1 - --t * t * t * t,
                Ease.InOutQuart => t < 0.5f ? 8 * t * t * t * t : 1 - 8 * --t * t * t * t,
                Ease.InQuint => t * t * t * t * t,
                Ease.OutQuint => 1 + --t * t * t * t * t,
                Ease.InOutQuint => t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * --t * t * t * t * t,
                Ease.InSine => 1 - Mathf.Cos(t * Mathf.PI / 2),
                Ease.OutSine => Mathf.Sin(t * Mathf.PI / 2),
                Ease.InOutSine => -0.5f * (Mathf.Cos(Mathf.PI * t) - 1),
                Ease.InExpo => t == 0 ? 0 : Mathf.Pow(2, 10 * (t - 1)),
                Ease.OutExpo => Mathf.Approximately(t, 1) ? 1 : 1 - Mathf.Pow(2, -10 * t),
                Ease.InOutExpo => t == 0 ? 0 :
                    Mathf.Approximately(t, 1) ? 1 :
                    t < 0.5f ? 0.5f * Mathf.Pow(2, 20 * t - 10) : 1 - 0.5f,
                Ease.InCirc => 1 - Mathf.Sqrt(1 - t * t),
                Ease.OutCirc => Mathf.Sqrt(1 - --t * t),
                Ease.InOutCirc => t < 0.5f
                    ? 0.5f * (1 - Mathf.Sqrt(1 - 4 * t * t))
                    : 0.5f * (Mathf.Sqrt(1 - -2 * t) + 1),
                Ease.InElastic => t == 0 ? 0 :
                    Mathf.Approximately(t, 1) ? 1 :
                    -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * (2 * Mathf.PI) / 3),
                Ease.OutElastic => t == 0 ? 0 :
                    Mathf.Approximately(t, 1) ? 1 :
                    Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * (2 * Mathf.PI) / 3) + 1,
                Ease.InOutElastic => t == 0
                    ? 0
                    : Mathf.Approximately(t, 1)
                        ? 1
                        : t < 0.5f
                            ? -0.5f * Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * (2 * Mathf.PI) / 4.5f)
                            : 0.5f * Mathf.Pow(2, -20 * t + 10) *
                            Mathf.Sin((20 * t - 11.125f) * (2 * Mathf.PI) / 4.5f) + 1,
                Ease.InBack => t * t * (2.70158f * t - 1.70158f),
                Ease.OutBack => --t * t * (2.70158f * t + 1.70158f) + 1,
                Ease.InOutBack => t < 0.5f
                    ? 4 * t * t * (3.5949095f * t - 2.5949095f)
                    : 1 + --t * t * (3.5949095f * t + 2.5949095f),
                Ease.InBounce => 1 - Evaluate(Ease.OutBounce, 1 - t),
                Ease.OutBounce => t < 1 / 2.75f
                    ? 7.5625f * t * t
                    : t < 2 / 2.75f
                        ? 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f
                        : t < 2.5 / 2.75
                            ? 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f
                            : 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f,
                Ease.InOutBounce => t < 0.5f
                    ? 0.5f * Evaluate(Ease.InBounce, t * 2)
                    : 0.5f * Evaluate(Ease.OutBounce, t * 2 - 1) + 0.5f,
                _ => throw new ArgumentOutOfRangeException(nameof(ease), ease, null)
            };
        }
    }
}