using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomUtilities
{
    public static float Average(Queue<float> queue)
    {
        float average = 0;
        foreach (float el in queue) {
            average += el;
        }
        return (average / queue.Count);
    }
    
    public static float WeightedAverage(Queue<float> queue)
    {
        float average = 0;
        float coeff = queue.Count;
        float total = (coeff * (coeff + 1)) / 2;
        foreach (float el in queue.Reverse()) {
            average += el * coeff;
            coeff -= 1;
        }
        return (average / total);
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
