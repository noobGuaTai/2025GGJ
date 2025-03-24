using System;

public static class ActionExtensions
{
    public static Action AsOneTimeAction(this Action action)
    {
        return () =>
        {
            action?.Invoke();
            action = null;
        };
    }
}
