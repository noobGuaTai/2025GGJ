using UnityEngine;

public static class ProjectileMotion
{
    /// <summary>
    /// 计算抛体运动的初速度向量
    /// </summary>
    /// <param name="startPoint">起始点位置</param>
    /// <param name="targetPoint">目标点位置</param>
    /// <param name="angle">发射角度(角度制)</param>
    /// <param name="gravity">重力加速度，默认使用Unity的物理重力</param>
    /// <returns>返回所需的初速度向量，如果无法到达目标则返回Vector3.zero</returns>
    public static Vector3 CalculateInitialVelocity(Vector3 startPoint, Vector3 targetPoint, float angle, float gravity = 9.81f)
    {
        // 将角度转换为弧度
        float angleRad = angle * Mathf.Deg2Rad;

        // 计算水平距离
        Vector3 displacement = targetPoint - startPoint;
        float horizontalDistance = new Vector3(displacement.x, 0, displacement.z).magnitude;

        // 计算高度差
        float heightDifference = displacement.y;

        // 计算发射速度大小
        // 使用公式：v^2 = (g * x^2) / (2 * (y - x * tan(θ)) * cos^2(θ))
        // 其中 x 是水平距离，y 是高度差，θ 是发射角度，g 是重力加速度

        float denominator = 2 * (heightDifference - horizontalDistance * Mathf.Tan(angleRad)) * Mathf.Pow(Mathf.Cos(angleRad), 2);

        // 检查是否有解
        if (denominator <= 0)
        {
            Debug.LogWarning("无法以给定角度到达目标点");
            return Vector3.zero;
        }

        float speedSquared = (gravity * horizontalDistance * horizontalDistance) / denominator;

        // 确保速度值有效
        if (speedSquared <= 0)
        {
            Debug.LogWarning("计算结果速度无效");
            return Vector3.zero;
        }

        float speed = Mathf.Sqrt(speedSquared);

        // 计算水平方向
        Vector3 horizontalDir = new Vector3(displacement.x, 0, displacement.z).normalized;

        // 构建初速度向量
        Vector3 initialVelocity = horizontalDir * speed * Mathf.Cos(angleRad);
        initialVelocity.y = speed * Mathf.Sin(angleRad);

        return initialVelocity;
    }

    /// <summary>
    /// 预测抛体运动轨迹点
    /// </summary>
    /// <param name="startPoint">起始点</param>
    /// <param name="initialVelocity">初速度</param>
    /// <param name="gravity">重力加速度</param>
    /// <param name="timeStep">时间步长</param>
    /// <param name="maxTime">最大预测时间</param>
    /// <returns>轨迹点数组</returns>
    public static Vector3[] PredictTrajectory(Vector3 startPoint, Vector3 initialVelocity, float gravity, float timeStep, float maxTime)
    {
        int steps = Mathf.CeilToInt(maxTime / timeStep);
        Vector3[] points = new Vector3[steps];

        for (int i = 0; i < steps; i++)
        {
            float time = i * timeStep;
            Vector3 point = startPoint + initialVelocity * time;
            point.y = startPoint.y + initialVelocity.y * time - 0.5f * gravity * time * time;
            points[i] = point;
        }

        return points;
    }
}

