using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomScaler : MonoBehaviour
{
    // valueToScale = the value that you need to scale between .. and ...
    // valueMin = the lowest value you want to register (outside of valueMin will go beyond the minScaleTo return value)
    // valueMax = the higest value you want to register (outside of valueMax will go beyond the maxScaleTo return value)
    // minScaleTo = the lowest result you want to return
    // maxScaleTo = the higest result you want to return

    public static double Scalef(float valueToScale, float valueMin, float valueMax, float minScaleTo, float maxScaleTo)
    {
        double scaledValue = minScaleTo + (double)(valueToScale - valueMin) / (valueMax - valueMin) * (maxScaleTo - minScaleTo);

        return scaledValue;
    }

    public static double Scale(int valueToScale, int valueMin, int valueMax, int minScaleTo, int maxScaleTo)
    {
        double scaledValue = minScaleTo + (double)(valueToScale - valueMin) / (valueMax - valueMin) * (maxScaleTo - minScaleTo);

        return scaledValue;
    }
}
