using Godot;
using System;

public partial class ChapterLabel : RichTextLabel
{
	[Export] public int ChapterIndex { get; set; } = 0;
	[Export] public int ChapterStartPage { get; set; } = 1;
	[Signal] public delegate void ChapterClickedEventHandler(int chapterStartPage);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_mouse_entered()
	{
		// add underline to text using BBCode
		BbcodeEnabled = true;
		Text = $"[u]{Text}[/u]";
	}
	private void _on_mouse_exited()
	{
		// remove underline from text using BBCode
		BbcodeEnabled = true;
		Text = Text.Replace("[u]", "").Replace("[/u]", "");
	}
	private void _on_gui_input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			GD.Print($"Chapter {ChapterIndex} clicked, starting on page {ChapterStartPage}");
			// Emit signal to parent CodexBook to go to the chapter start page
			EmitSignal(SignalName.ChapterClicked, ChapterStartPage);
		}
	}

}
