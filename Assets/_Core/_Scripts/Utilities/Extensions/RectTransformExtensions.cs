using UnityEngine;

public static class RectTransformExtensions
{
    public static bool IsOverlapping(this RectTransform rectTransform1, RectTransform rectTransform2)
    {
        Rect rect1 = GetWorldRect(rectTransform1);
        Rect rect2 = GetWorldRect(rectTransform2);

        return rect1.Overlaps(rect2);
    }

    private static Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        return new Rect(bottomLeft, topRight - bottomLeft);
    }
    public static float GetDistance(this RectTransform rectTransform1, RectTransform rectTransform2)
    {
        Rect rect1 = GetWorldRect(rectTransform1);
        Rect rect2 = GetWorldRect(rectTransform2);

        return Vector2.Distance(rect1.center, rect2.center);
    }
}