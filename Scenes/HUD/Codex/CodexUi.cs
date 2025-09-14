using Godot;
using System;


public partial class CodexUi : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// for each BookPreview in the GeographyGroup, connect the BookPreviewPressed signal to the onBookPreviewClicked method
		foreach (var bookPreview in GetNode<HBoxContainer>("%GeographyGroup").GetChildren())
		{
			if (bookPreview is BookPreview bp)
			{
				bp.Connect("BookPreviewPressed", new Callable(this, nameof(onBookPreviewClicked)));
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void FadeOutCodex()
	{
		DisplayUtils.BounceEffect(GetNode<TextureButton>("%BackButton"), 0.15f);
		TimerUtils.DelayedAction(GetTree(), 150, () =>
		{
			var transition = GetNode<ColorRect>("%TransitionScreen");
			Tween tween = GetTree().CreateTween();
			GetNode<TextureButton>("%BackButton").Disabled = false;
			tween.TweenProperty(transition, "color:a", 1.0f, 0.125f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
			tween.Chain().TweenCallback(Callable.From(() => Visible = false));
			tween.Chain().TweenProperty(transition, "color:a", 0.0f, 0.125f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
		});
	}


	private void _on_texture_button_pressed()
	{
		GetNode<TextureButton>("%BackButton").Disabled = true;
		FadeOutCodex();
	}

	
	private void onBookPreviewClicked(BookPreview bookPreview)
	{
		GD.Print($"Book {bookPreview} pressed");
		var bookOverlay = GetNode<ColorRect>("%CodexBook");
		bookOverlay.Visible = true;
		GetNode<CodexBook>("%CodexBook").LoadBookData(bookPreview.Name);
		GetNode<CodexBook>("%CodexBook").DisplayBook();
	}


}
