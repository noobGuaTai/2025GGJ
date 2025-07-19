using UnityEngine;

[RequireComponent(typeof(Camera))] // 确保此脚本附加到有 Camera 组件的对象上
public class AspectRatioEnforcer : MonoBehaviour
{
    // 期望的宽高比 (例如 16:9)
    public float targetAspectRatio = 16.0f / 9.0f;

    private Camera _camera;
    private int lastScreenWidth = 0;
    private int lastScreenHeight = 0;

    void Start()
    {
        _camera = GetComponent<Camera>();
        if (_camera == null)
        {
            Debug.LogError("AspectRatioEnforcer: No Camera component found on this GameObject.");
            enabled = false; // 禁用脚本如果没有相机
            return;
        }

        // 确保相机背景是黑色 (或者你希望的边框颜色)
        _camera.backgroundColor = Color.black;
        _camera.clearFlags = CameraClearFlags.SolidColor; // 或者 Skybox 如果你的游戏区域外应该显示天空盒

        UpdateAspectRatio();
    }

    void Update()
    {
        // 仅当屏幕尺寸发生变化时才更新，以提高效率
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            UpdateAspectRatio();
        }
    }

    void UpdateAspectRatio()
    {
        if (_camera == null) return;

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        lastScreenWidth = screenWidth;
        lastScreenHeight = screenHeight;

        float windowAspectRatio = (float)screenWidth / screenHeight;
        float scaleHeight = windowAspectRatio / targetAspectRatio;

        Rect rect = _camera.rect;

        if (scaleHeight < 1.0f) // 如果窗口比目标宽高比“更高”（例如 4:3 窗口显示 16:9 内容），需要 Letterboxing (上下黑边)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else // 如果窗口比目标宽高比“更宽”（例如 21:9 窗口显示 16:9 内容），需要 Pillarboxing (左右黑边)
        {
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        _camera.rect = rect;
        // Debug.Log($"Updated Aspect Ratio. Window: {windowAspectRatio}, Viewport Rect: {rect}");
    }

    // (可选) 如果你希望在编辑器中改变 Game 视图大小时也能预览效果 (但通常在运行时效果更准确)
#if UNITY_EDITOR
    void OnValidate()
    {
        // 这个方法在编辑器中值被改变时调用
        // 注意：在编辑器中频繁更新可能不总是完美反映运行时行为，
        // 且可能需要你在编辑器中 Play/Stop 一次才能看到效果
        // 或者在 Game 视图中选择一个固定的分辨率进行测试
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }
        // 可以在这里调用 UpdateAspectRatio()，但要注意它在非运行时获取 Screen.width/height 可能不准确
        // 更好的做法是在编辑器中设置Game视图为固定分辨率来预览。
    }
#endif
}