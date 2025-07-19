using UnityEngine;
using System.Collections;
using System.IO; // 用于文件操作

public class ScreenshotExporter : MonoBehaviour
{
    // 你可以通过 Inspector 面板或者代码触发这个方法
    public void TakeScreenshot()
    {
        StartCoroutine(CaptureFrame());
    }

    private IEnumerator CaptureFrame()
    {
        // 等待当前帧渲染结束
        yield return new WaitForEndOfFrame();

        // 捕获屏幕截图为 Texture2D
        Texture2D screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();

        // 将 Texture2D 编码为 PNG 格式的字节数组
        // 对于 JPG，可以使用 screenshotTexture.EncodeToJPG()，但 PNG 通常更适合截图，因为它无损
        byte[] bytes = screenshotTexture.EncodeToPNG();

        // （可选）销毁临时的 Texture2D 对象以释放内存
        // 如果你还需要在其他地方使用这个 texture，可以稍后销毁
        Object.Destroy(screenshotTexture);

        // 定义文件保存路径
        // Application.persistentDataPath 是一个在所有平台上都可以安全写入的路径
        string directoryPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        // 使用时间戳命名文件以避免覆盖
        string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string filePath = Path.Combine(directoryPath, fileName);

        // 将字节数组写入文件
        try
        {
            File.WriteAllBytes(filePath, bytes);
            Debug.Log($"截图已保存到: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"保存截图失败: {e.Message}");
        }
    }

    // 示例：按下P键截图
    void Start()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }
}