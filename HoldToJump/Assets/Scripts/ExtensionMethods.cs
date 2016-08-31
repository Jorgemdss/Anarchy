using UnityEngine;
using System.Collections;
using System;

public static class ExtensionMethods
{
    /// <summary>
    /// Resets the Position, rotation and scale of the object.
    /// </summary>
    /// <param name="transform">The transform.</param>
    /// <param name="resetPosition">Reset position indicator.</param>
    /// <param name="resetRotation">Reset rotation indicator.</param>
    /// <param name="resetScale">Reset scale indicator.</param>
    public static void ResetTransform(
        this Transform transform,
        bool resetPosition,
        bool resetRotation,
        bool resetScale)
    {
        if(resetPosition)
            transform.position = Vector3.zero;

        if (resetRotation)
            transform.localRotation = Quaternion.identity;

        if (resetScale)
            transform.localScale = new Vector3(1, 1, 1);
    }


    public static T ChangeType<T>(this object obj)
    {
        return (T)Convert.ChangeType(obj, typeof(T));
    }
}
