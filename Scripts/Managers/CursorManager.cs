using Godot;
using System;

public partial class CursorManager : Node
{
    [Export] private Texture2D HandCursor;
    [Export] private Vector2 HandHotspot = Vector2.Zero;

    public override void _Ready()
    {
        if (HandCursor != null)
        {
            Input.SetCustomMouseCursor(
                HandCursor,
                Input.CursorShape.PointingHand,
                HandHotspot
            );
        }
    }
}
