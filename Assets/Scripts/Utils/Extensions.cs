using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Extensions
{
    public static void SetOrInit<T, V>(this Dictionary<T, V> dict, T key, V value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
    }

    public static V GetOrInit<T, V>(this Dictionary<T, V> dict, T key, V defaultValue)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];
        }
        else
        {
            dict.Add(key, defaultValue);
            return defaultValue;
        }
    }

    public static float AngleDiff(this Quaternion self, Quaternion other, Vector3 axis)
    {
        Vector3 vecA = other * axis;
        Vector3 vecB = self * axis;
        float angleA = Mathf.Atan2(vecA.x, vecA.z) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(vecB.x, vecB.z) * Mathf.Rad2Deg;
        return Mathf.DeltaAngle(angleB, angleA);
    }

    public static bool CheckLayer(this LayerMask layerMask, LayerMask layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    public static Vector2Int Rotate(this Vector2Int vector, float angle)
    {
        // Convert angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Rotate vector
        int newX = Mathf.RoundToInt(vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians));
        int newY = Mathf.RoundToInt(vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians));

        return new Vector2Int(newX, newY);
    }

    public static Vector2Int Swap(this Vector2Int vector)
    {
        return new Vector2Int(vector.y, vector.x);
    }

    public static Vector2Int Abs(this Vector2Int vector)
    {
        return new Vector2Int(System.Math.Abs(vector.x), System.Math.Abs(vector.y));
    }

    public static float RandomRange(this Vector2 vector)
    {
        return Random.Range(vector.x, vector.y);
    }

    public static int RandomRange(this Vector2Int vector)
    {
        return Random.Range(vector.x, vector.y + 1);
    }

    public static IEnumerable<float> CumulativeSum(this IEnumerable<float> sequence)
    {
        float sum = 0;
        foreach (float item in sequence)
        {
            sum += item;
            yield return sum;
        }
    }

    public static float PathDistance(this NavMeshPath path)
    {
        float distance = 0f;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector2.Distance(path.corners[i], path.corners[i + 1]);
        }
        return distance;
    }

    public static T RandomChoice<T>(this List<T> list) 
    {
        return list[Random.Range(0, list.Count)];
    }
}
