using UnityEngine;

public static class TransformExtensions
{
    public static Transform FindInChildren(this Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }
            Transform result = child.FindInChildren(name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}