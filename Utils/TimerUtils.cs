using Godot;
using System;
using System.Threading.Tasks;

public static class TimerUtils
{
    /// <summary>
    /// Executes an action after a delay (in milliseconds).
    /// </summary>
    /// <param name="tree">SceneTree (usually pass GetTree()).</param>
    /// <param name="milliseconds">Delay in milliseconds.</param>
    /// <param name="action">Action to execute after delay.</param>
    public static async void DelayedAction(SceneTree tree, int milliseconds, Action action)
    {
        if (tree == null || action == null)
            return;

        // Convert milliseconds → seconds (float)
        float seconds = milliseconds / 1000f;

        // Create a timer and wait
        await tree.ToSignal(tree.CreateTimer(seconds), "timeout");

        // Execute the callback
        action();
    }

    /// <summary>
    /// Awaitable version, returns a Task.
    /// </summary>
    public static async Task Delay(SceneTree tree, int milliseconds)
    {
        if (tree == null)
            return;

        float seconds = milliseconds / 1000f;
        await tree.ToSignal(tree.CreateTimer(seconds), "timeout");
    }
}
