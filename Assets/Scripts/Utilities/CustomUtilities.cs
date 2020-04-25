using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomUtilities
{
    public static float WeightedAverage(Queue<float> queue)
    {
        float average = 0;
        float weight = 1.0f;
        foreach (float el in queue) {
            average += weight * el;
            weight -= 1 / queue.Count;
        }
        return (average / queue.Count);
    }

    public static float Average(Queue<float> queue)
    {
        float average = 0;
        foreach (float el in queue) {
            average += el;
        }
        return (average / queue.Count);
    }
}
