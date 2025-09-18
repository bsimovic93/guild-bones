using Godot;
using System;
using System.Linq;

[Tool]
public partial class DialogSequence : Resource
{
    [Export] public string CharacterName { get; set; } = "NPC Default";
    [Export] public string[] DialogLines { get; set; } = Array.Empty<string>();

    public override void _ValidateProperty(Godot.Collections.Dictionary property)
    {
        base._ValidateProperty(property);

        foreach (var line in DialogLines)
        {
            if (line.Length > 390)
            {
                GD.PushWarning("Dialog line exceeds 390 characters. Dialog box will resize to fit, which may not look good.");
                break;
            }
        }
    }
}
