using Oasez.Extensions.Generics.Singleton;
using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper : GenericSingleton<CoroutineHelper, CoroutineHelper> {

    /// <summary>
    /// Delays action for given duration
    /// </summary>
    /// <param name="delayDuration">The duration of the delay</param>
    /// <param name="onDelayed">The action to be called after the delay</param>
    /// <param name="unscaled">If the time should be unscaled</param>
    /// <returns></returns>
    public static Coroutine Delay(float delayDuration, Action onDelayed, bool unscaled = true) {
        if (delayDuration <= 0) {
            onDelayed?.Invoke();
            return null;
        }

        return Instance.StartCoroutine(DelayOverTimeRoutine(delayDuration, onDelayed, unscaled));
    }

    /// <summary>
    /// Delays action till predicate is true
    /// </summary>
    /// <param name="predicate">The predicate that the action will wait for</param>
    /// <param name="onTrue">Invoked when predicate == true</param>
    /// <returns></returns>
    public static Coroutine DelayTillTrue(Func<bool> predicate, Action onTrue) {
        return Instance.StartCoroutine(DelayTillTrueRoutine(predicate, onTrue));
    }

    /// <summary>
    /// Delays action for 1 frame
    /// </summary>
    /// <param name="onDelayed">The action that is delayed</param>
    /// <returns></returns>
    public static Coroutine DelayFrame(Action onDelayed) {
        return Instance.StartCoroutine(DelayOverFramesRoutine(1, onDelayed));
    }

    /// <summary>
    /// Delays action for 1 frame
    /// </summary>
    /// <param name="onDelayed">The action that is delayed</param>
    /// <returns></returns>
    public static Coroutine DelayFrames(int frames, Action onDelayed) {
        return Instance.StartCoroutine(DelayOverFramesRoutine(frames, onDelayed));
    }

    /// <summary>
    /// Starts a coroutine
    /// </summary>
    /// <param name="routine">Coroutine to start</param>
    /// <returns></returns>
    public static Coroutine Start(IEnumerator routine) {
        return Instance.StartLocalCoroutine(routine);
    }

    /// <summary>
    /// Stops a coroutine running through CoroutineHelper
    /// </summary>
    /// <param name="routine">The routine to be stopped</param>
    public static void Stop(Coroutine routine) {
        if (routine == null) { return; }
        Instance.StopCoroutine(routine);
    }

    private Coroutine StartLocalCoroutine(IEnumerator routine) {
        return StartCoroutine(routine);
    }

    private static IEnumerator DelayOverTimeRoutine(float duration, Action onDelayed, bool unscaled) {
        string blockInteractionId = Guid.NewGuid().ToString();

        if (unscaled) {
            yield return new WaitForSecondsRealtime(duration);
        } else {
            yield return new WaitForSeconds(duration);
        }

        onDelayed?.Invoke();
    }

    private static IEnumerator DelayOverFramesRoutine(int frames, Action onDelayed) {
        for (int i = 0; i < frames; i++) {
            yield return null;
        }
        onDelayed?.Invoke();
    }

    private static IEnumerator DelayTillTrueRoutine(Func<bool> predicate, Action onTrue) {
        yield return new WaitUntil(predicate);
        onTrue?.Invoke();
    }

}