using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUtilities
{
    public static float WeightedAverage(Queue<float> queue)
    {
        float average = 0;
        float weight = 1.0f;
        foreach (float el in queue) {
            average += weight * el;
            weight -= 1 / (float)(queue.Count);
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
    
    public static Vector3 Average(Vector3[] array)
    {
        Vector3 average = Vector3.zero;
        foreach (Vector3 el in array) {
            average += el;
        }
        return (average / (float)(array.Length));
    }
    
}
