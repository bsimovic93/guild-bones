using Godot;
using System;

public partial class BookPreview : TextureButton
{
	private float _originalPositionY;
	private bool _positionInitialized = false;
	[Signal] public delegate void BookPreviewPressedEventHandler(BookPreview bookPreview);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetBooksCursor();
		_originalPositionY = Position.Y;
		_positionInitialized = true;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetBooksCursor()
	{
		if (this is Control control)
		{
			control.MouseDefaultCursorShape = CursorShape.PointingHand;
			control.MouseFilter = MouseFilterEnum.Pass;
		}
	}

	private void _on_book_preview_mouse_entered()
	{
		if (!_positionInitialized) return;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position:y", _originalPositionY - 10, 0.15f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
	}

	private void _on_book_preview_mouse_exited()
	{
		if (!_positionInitialized) return;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position:y", _originalPositionY, 0.15f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
	}

	private void _on_pressed()
	{
		GD.Print($"Book {Name} pressed");
		EmitSignal(nameof(BookPreviewPressed), this);
	}
}
