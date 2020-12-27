using UnityEngine;

public static class TweenManagerLibrary
{
    public delegate float TweenFunction(float time, float beginning, float change, float duration);
    static TweenFunction[] tweenFunctions = { LinearTween, EaseInQuad, EaseOutQuad, EaseInOutQuad, EaseInOutQuint, EaseInOutSine };

    public static TweenFunction GetTweenFunction(int index)
    {
        return tweenFunctions[index];
    }

    public static float LinearTween(float time, float beginning, float change, float duration)
    {
        return time * change / duration + beginning;
    }

    public static float EaseInQuad(float time, float beginning, float change, float duration)
    {
        return change * (time /= duration) * time + beginning;
    }

    public static float EaseOutQuad(float time, float beginning, float change, float duration)
    {
        return -change * (time /= duration) * (time - 2) + beginning;
    }

    public static float EaseInOutQuad(float time, float beginning, float change, float duration)
    {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time + beginning;
        return -change / 2 * ((--time) * (time - 2) - 1) + beginning;
    }

    public static float EaseInOutQuint(float time, float beginning, float change, float duration)
    {
        if ((time /= duration / 2) < 1)
            return change / 2 * Mathf.Pow(time, 5) + beginning;
        return change / 2 * (Mathf.Pow(time-2, 5) + 2) + beginning;
    }
    public static float EaseInOutSine(float time, float beginning, float change, float duration)
    {
        return -change / 2 * (Mathf.Cos(Mathf.PI * time / duration) - 1) + beginning;
    }

    /*
        HOW TO USE

        float time;
        float change;
        float startValue;
        float targetValue;
        float tweenDuration;

    //example function
    void MoveValue(int exampleValue)
    {
        change = targetValue - startValue;

        if (time <= tweenDuration)
        {
            time += Time.deltaTime;
            exampleValue = TweenManager.LinearTween(time, startValue, change, tweenDuration);
        }
    }
    */
}
