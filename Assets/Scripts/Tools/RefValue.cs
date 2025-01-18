using System;

[Serializable]
public class RefValue<T>
{
    
    public T value;

    public RefValue() { }

    public RefValue(T value)
    {
        this.value = value;
    }

    // 隐式转换为 T
    public static implicit operator T(RefValue<T> refValue)
    {
        return refValue.value;
    }

    // 隐式从 T 转换为 RefValue<T>
    public static implicit operator RefValue<T>(T value)
    {
        return new RefValue<T>(value);
    }

    // 获取或设置值
    public T Value
    {
        get => value;
        set => this.value = value;
    }

    public override string ToString()
    {
        return value?.ToString() ?? "null";
    }
}