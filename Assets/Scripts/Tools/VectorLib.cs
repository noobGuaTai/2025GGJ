using UnityEngine;
public static class VectorLib
{
    public static float Length(this Vector3 vector)
    {
        return Vector3.Distance(Vector3.zero, vector);
    }

    public static bool IsNormalized(this Vector3 vector)
    {
        return Mathf.Approximately(vector.sqrMagnitude, 1f);
    }

    public static Vector3 Limit(this Vector3 vector, float maxLength)
    {
        return vector.magnitude > maxLength ? vector.normalized * maxLength : vector;
    }

    public static Vector3 Project(this Vector3 vector, Vector3 normal)
    {
        normal.Normalize();
        return Vector3.Dot(vector, normal) * normal;
    }

    public static Vector3 Reflect(this Vector3 vector, Vector3 normal)
    {
        normal.Normalize();
        return vector - 2 * Vector3.Dot(vector, normal) * normal;
    }

    public static Vector3 Lerp(this Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, Mathf.Clamp01(t));
    }

    public static Vector3 RotateTowards(this Vector3 current, Vector3 target, float maxRadiansDelta)
    {
        return Vector3.RotateTowards(current, target, maxRadiansDelta, 0f);
    }

    public static float AngleTo(this Vector3 from, Vector3 to)
    {
        return Vector3.Angle(from, to);
    }

    public static Vector3 Rotate(this Vector3 vec, Vector3 axis, float degree)
    {
        axis.Normalize();
        float radians = degree * Mathf.Deg2Rad;
        return Quaternion.AngleAxis(degree, axis) * vec;
    }
    public static float DistanceTo(this Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }


    public static Vector2 ToVector2(this float v) => new Vector2(v, v);
    public static Vector3 ToVector3(this float v) => new Vector3(v, v, v);
    public static float ToFloat(this Vector3 v) => v.x;
    public static float ToFloat(this Vector2 v) => v.x;
    public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.y);
    public static Vector3 ToVector3(this Vector2 v) => new Vector3(v.x, v.y, 0);
}

public static class Vector3New
{
    /// <summary>
    /// 设置新的 X 值。
    /// </summary>
    public static Vector3 NewX(this Vector3 v, float newX) => new Vector3(newX, v.y, v.z);

    /// <summary>
    /// 设置新的 Y 值。
    /// </summary>
    public static Vector3 NewY(this Vector3 v, float newY) => new Vector3(v.x, newY, v.z);

    /// <summary>
    /// 设置新的 Z 值。
    /// </summary>
    public static Vector3 NewZ(this Vector3 v, float newZ) => new Vector3(v.x, v.y, newZ);

    /// <summary>
    /// 同时设置新的 X 和 Y 值。
    /// </summary>
    public static Vector3 NewXY(this Vector3 v, float newX, float newY) => new Vector3(newX, newY, v.z);

    /// <summary>
    /// 同时设置新的 X 和 Z 值。
    /// </summary>
    public static Vector3 NewXZ(this Vector3 v, float newX, float newZ) => new Vector3(newX, v.y, newZ);

    /// <summary>
    /// 同时设置新的 Y 和 Z 值。
    /// </summary>
    public static Vector3 NewYZ(this Vector3 v, float newY, float newZ) => new Vector3(v.x, newY, newZ);

    /// <summary>
    /// 同时设置新的 X、Y 和 Z 值。
    /// </summary>
    public static Vector3 NewXYZ(this Vector3 v, float newX, float newY, float newZ) => new Vector3(newX, newY, newZ);

    /// <summary>
    /// 交换 X 和 Y 值。
    /// </summary>
    public static Vector3 SwapXY(this Vector3 v) => new Vector3(v.y, v.x, v.z);

    /// <summary>
    /// 交换 X 和 Z 值。
    /// </summary>
    public static Vector3 SwapXZ(this Vector3 v) => new Vector3(v.z, v.y, v.x);

    /// <summary>
    /// 交换 Y 和 Z 值。
    /// </summary>
    public static Vector3 SwapYZ(this Vector3 v) => new Vector3(v.x, v.z, v.y);
}


public static class Vector2New
{
    /// <summary>
    /// 设置新的 X 值。
    /// </summary>
    public static Vector2 NewX(this Vector2 v, float newX) => new Vector2(newX, v.y);

    /// <summary>
    /// 设置新的 Y 值。
    /// </summary>
    public static Vector2 NewY(this Vector2 v, float newY) => new Vector2(v.x, newY);

    /// <summary>
    /// 同时设置新的 X 和 Y 值。
    /// </summary>
    public static Vector2 NewXY(this Vector2 v, float newX, float newY) => new Vector2(newX, newY);

    /// <summary>
    /// 交换 X 和 Y 值。
    /// </summary>
    public static Vector2 SwapXY(this Vector2 v) => new Vector2(v.y, v.x);
}


public static class Vector2IntNew{
    /// <summary>
    /// 设置新的 X 值。
    /// </summary>
    public static Vector2Int NewX(this Vector2Int v, int newX) => new Vector2Int(newX, v.y);

    /// <summary>
    /// 设置新的 Y 值。
    /// </summary>
    public static Vector2Int NewY(this Vector2Int v, int newY) => new Vector2Int(v.x, newY);

    /// <summary>
    /// 同时设置新的 X 和 Y 值。
    /// </summary>
    public static Vector2Int NewXY(this Vector2Int v, int newX, int newY) => new Vector2Int(newX, newY);

    /// <summary>
    /// 交换 X 和 Y 值。
    /// </summary>
    public static Vector2Int SwapXY(this Vector2Int v) => new Vector2Int(v.y, v.x);
}


public static class Vector3IntNew
{
    /// <summary>
    /// 设置新的 X 值。
    /// </summary>
    public static Vector3Int NewX(this Vector3Int v, int newX) => new Vector3Int(newX, v.y, v.z);

    /// <summary>
    /// 设置新的 Y 值。
    /// </summary>
    public static Vector3Int NewY(this Vector3Int v, int newY) => new Vector3Int(v.x, newY, v.z);

    /// <summary>
    /// 设置新的 Z 值。
    /// </summary>
    public static Vector3Int NewZ(this Vector3Int v, int newZ) => new Vector3Int(v.x, v.y, newZ);

    /// <summary>
    /// 同时设置新的 X 和 Y 值。
    /// </summary>
    public static Vector3Int NewXY(this Vector3Int v, int newX, int newY) => new Vector3Int(newX, newY, v.z);

    /// <summary>
    /// 同时设置新的 X 和 Z 值。
    /// </summary>
    public static Vector3Int NewXZ(this Vector3Int v, int newX, int newZ) => new Vector3Int(newX, v.y, newZ);

    /// <summary>
    /// 同时设置新的 Y 和 Z 值。
    /// </summary>
    public static Vector3Int NewYZ(this Vector3Int v, int newY, int newZ) => new Vector3Int(v.x, newY, newZ);

    /// <summary>
    /// 同时设置新的 X、Y 和 Z 值。
    /// </summary>
    public static Vector3Int NewXYZ(this Vector3Int v, int newX, int newY, int newZ) => new Vector3Int(newX, newY, newZ);

    /// <summary>
    /// 交换 X 和 Y 值。
    /// </summary>
    public static Vector3Int SwapXY(this Vector3Int v) => new Vector3Int(v.y, v.x, v.z);

    /// <summary>
    /// 交换 X 和 Z 值。
    /// </summary>
    public static Vector3Int SwapXZ(this Vector3Int v) => new Vector3Int(v.z, v.y, v.x);

    /// <summary>
    /// 交换 Y 和 Z 值。
    /// </summary>
    public static Vector3Int SwapYZ(this Vector3Int v) => new Vector3Int(v.x, v.z, v.y);
}


public static class ColorNew
{
    /// <summary>
    /// 设置新的 R 值。
    /// </summary>
    public static Color NewR(this Color color, float newR) => new Color(newR, color.g, color.b, color.a);

    /// <summary>
    /// 设置新的 G 值。
    /// </summary>
    public static Color NewG(this Color color, float newG) => new Color(color.r, newG, color.b, color.a);

    /// <summary>
    /// 设置新的 B 值。
    /// </summary>
    public static Color NewB(this Color color, float newB) => new Color(color.r, color.g, newB, color.a);

    /// <summary>
    /// 设置新的 A 值。
    /// </summary>
    public static Color NewA(this Color color, float newA) => new Color(color.r, color.g, color.b, newA);

    /// <summary>
    /// 同时设置新的 R 和 G 值。
    /// </summary>
    public static Color NewRG(this Color color, float newR, float newG) => new Color(newR, newG, color.b, color.a);

    /// <summary>
    /// 同时设置新的 R 和 B 值。
    /// </summary>
    public static Color NewRB(this Color color, float newR, float newB) => new Color(newR, color.g, newB, color.a);

    /// <summary>
    /// 同时设置新的 R 和 A 值。
    /// </summary>
    public static Color NewRA(this Color color, float newR, float newA) => new Color(newR, color.g, color.b, newA);

    /// <summary>
    /// 同时设置新的 G 和 B 值。
    /// </summary>
    public static Color NewGB(this Color color, float newG, float newB) => new Color(color.r, newG, newB, color.a);

    /// <summary>
    /// 同时设置新的 G 和 A 值。
    /// </summary>
    public static Color NewGA(this Color color, float newG, float newA) => new Color(color.r, newG, color.b, newA);

    /// <summary>
    /// 同时设置新的 B 和 A 值。
    /// </summary>
    public static Color NewBA(this Color color, float newB, float newA) => new Color(color.r, color.g, newB, newA);

    /// <summary>
    /// 同时设置新的 R、G 和 B 值。
    /// </summary>
    public static Color NewRGB(this Color color, float newR, float newG, float newB) => new Color(newR, newG, newB, color.a);

    /// <summary>
    /// 同时设置新的 R、G 和 A 值。
    /// </summary>
    public static Color NewRGA(this Color color, float newR, float newG, float newA) => new Color(newR, newG, color.b, newA);

    /// <summary>
    /// 同时设置新的 R、B 和 A 值。
    /// </summary>
    public static Color NewRBA(this Color color, float newR, float newB, float newA) => new Color(newR, color.g, newB, newA);

    /// <summary>
    /// 同时设置新的 G、B 和 A 值。
    /// </summary>
    public static Color NewGBA(this Color color, float newG, float newB, float newA) => new Color(color.r, newG, newB, newA);

    /// <summary>
    /// 同时设置新的 R、G、B 和 A 值。
    /// </summary>
    public static Color NewRGBA(this Color color, float newR, float newG, float newB, float newA) => new Color(newR, newG, newB, newA);

    /// <summary>
    /// 交换 R 和 G 值。
    /// </summary>
    public static Color SwapRG(this Color color) => new Color(color.g, color.r, color.b, color.a);

    /// <summary>
    /// 交换 R 和 B 值。
    /// </summary>
    public static Color SwapRB(this Color color) => new Color(color.b, color.g, color.r, color.a);

    /// <summary>
    /// 交换 R 和 A 值。
    /// </summary>
    public static Color SwapRA(this Color color) => new Color(color.a, color.g, color.b, color.r);

    /// <summary>
    /// 交换 G 和 B 值。
    /// </summary>
    public static Color SwapGB(this Color color) => new Color(color.r, color.b, color.g, color.a);

    /// <summary>
    /// 交换 G 和 A 值。
    /// </summary>
    public static Color SwapGA(this Color color) => new Color(color.r, color.a, color.b, color.g);

    /// <summary>
    /// 交换 B 和 A 值。
    /// </summary>
    public static Color SwapBA(this Color color) => new Color(color.r, color.g, color.a, color.b);

}

public static class ColorSpace
{
    /// <summary>
    /// 将 RGB 转换为标准化的 HSL 值。
    /// </summary>
    public static void RGBToHSL(Color color, out float h, out float s, out float l)
    {
        float max = Mathf.Max(color.r, color.g, color.b);
        float min = Mathf.Min(color.r, color.g, color.b);
        l = (max + min) / 2; // 亮度

        if (max == min)
        {
            h = s = 0; // 灰色
        }
        else
        {
            float d = max - min;
            s = l > 0.5f ? d / (2 - max - min) : d / (max + min);

            if (max == color.r) h = (color.g - color.b) / d + (color.g < color.b ? 6 : 0);
            else if (max == color.g) h = (color.b - color.r) / d + 2;
            else h = (color.r - color.g) / d + 4;

            h /= 6; // 标准化到 0~1
        }
    }

    /// <summary>
    /// 将标准化的 HSL 值转换为 RGB。
    /// </summary>
    public static Color HSLToRGB(float h, float s, float l)
    {
        h = Mathf.Clamp01(h) * 360f; // 标准化 H 转换到 0~360
        s = Mathf.Clamp01(s);
        l = Mathf.Clamp01(l);

        float c = (1 - Mathf.Abs(2 * l - 1)) * s;
        float x = c * (1 - Mathf.Abs((h / 60f) % 2 - 1));
        float m = l - c / 2;

        float r, g, b;
        if (h < 60) { r = c; g = x; b = 0; }
        else if (h < 120) { r = x; g = c; b = 0; }
        else if (h < 180) { r = 0; g = c; b = x; }
        else if (h < 240) { r = 0; g = x; b = c; }
        else if (h < 300) { r = x; g = 0; b = c; }
        else { r = c; g = 0; b = x; }

        return new Color(r + m, g + m, b + m, 1f); // 返回 RGB 颜色，Alpha 默认为 1
    }
    /// <summary>
    /// 在 HSV 空间中对两个颜色进行插值
    /// </summary>
    /// <param name="colorA">起始颜色</param>
    /// <param name="colorB">结束颜色</param>
    /// <param name="t">插值参数，范围 [0, 1]</param>
    /// <returns>插值后的颜色</returns>
    public static Color LerpHSV(this Color colorA, Color colorB, float t)
    {
        // 将颜色转换为 HSV
        Color.RGBToHSV(colorA, out float h1, out float s1, out float v1);
        Color.RGBToHSV(colorB, out float h2, out float s2, out float v2);

        // 处理 Hue 环绕插值
        float h;
        if (Mathf.Abs(h2 - h1) > 0.5f)
        {
            if (h1 > h2) h2 += 1;
            else h1 += 1;
        }
        h = Mathf.Lerp(h1, h2, t) % 1.0f;

        // 插值 Saturation 和 Value
        float s = Mathf.Lerp(s1, s2, t);
        float v = Mathf.Lerp(v1, v2, t);

        // 插值 Alpha 通道
        float a = Mathf.Lerp(colorA.a, colorB.a, t);

        // 将插值结果转换回 RGB 并返回
        return Color.HSVToRGB(h, s, v) * new Color(1, 1, 1, a);
    }


    static public Color Lerp(this Color from, Color to, float a){
        return from * (1 - a) + to * a;
    }
}

