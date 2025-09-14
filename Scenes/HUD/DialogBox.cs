using Godot;
using System;
using System.Threading.Tasks;

public partial class DialogBox : PanelContainer
{
	private RichTextLabel richLabel;
	private enum TextSpeed { Slow = 0, Normal = 1, Fast = 2 }
	private bool isTyping = false;
	private bool skipTyping = false;
	private int currentLineIndex = 0;
	[Export] public DialogSequence dialogSequence;
	[Export] TextSpeed textSpeed = TextSpeed.Normal;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		richLabel = GetNode<RichTextLabel>("%DialogText");

		Visible = false;
		GetParent<Control>().Position += new Vector2(0, 200);
		TimerUtils.DelayedAction(GetTree(), 500, () =>
		{
			TimerUtils.DelayedAction(GetTree(), 250, () =>
			{
				DisplayDialog(dialogSequence.DialogLines[currentLineIndex]);
			});

			ShowDialog();
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async void DisplayDialog(string text)
	{
		if (isTyping) skipTyping = true;
		GetNode<Label>("%HeroName").Text = dialogSequence.CharacterName;
		isTyping = true;
		richLabel.Text = "";
		foreach (char c in text)
		{
			if (skipTyping)
			{
				richLabel.Text = text;
				break;
			}
			richLabel.Text += c;
			await ToSignal(GetTree().CreateTimer(GetTextSpeedValue()), "timeout");
		}
		isTyping = false;
		skipTyping = false;
	}

	public void HideDialog(int delayMiliseconds = 0)
	{
		Tween tween = GetTree().CreateTween();
		Vector2 target = GetParent<Control>().Position + new Vector2(0, 200);
		tween.TweenProperty(GetParent(), "position", target, 250 / 1000f);
		if (delayMiliseconds > 0)
		{

			TimerUtils.DelayedAction(GetTree(), delayMiliseconds, () =>
			{
				Visible = false;
			});
			return;
		}
	}

	public void ShowDialog()
	{
		Visible = true;
		Tween tween = GetTree().CreateTween();
		Vector2 target = GetParent<Control>().Position + new Vector2(0, -200);
		tween.TweenProperty(GetParent(), "position", target, 250 / 1000f);
	}

	private double GetTextSpeedValue()
	{
		switch (textSpeed)
		{
			case TextSpeed.Slow:
				return 0.1;
			case TextSpeed.Fast:
				return 0.01;
			case TextSpeed.Normal:
			default:
				return 0.03;
		}
	}

	private void AdvanceDialogLine()
	{
		if (isTyping)
		{
			skipTyping = true;
			return;
		}
		currentLineIndex++;
		if (currentLineIndex < dialogSequence.DialogLines.Length)
		{
			DisplayDialog(dialogSequence.DialogLines[currentLineIndex]);
		}
		else
		{
			HideDialog(500);
		}
	}

	private void _on_DialogBox_gui_input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == MouseButton.WheelUp || mouseEvent.ButtonIndex == MouseButton.WheelDown) return;
			AdvanceDialogLine();
		}
	}

	private void _on_codex_button_pressed()
	{
		var button = GetNode<TextureButton>("%CodexButton");
		DisplayUtils.BounceEffect(button, 0.15f);
		button.Disabled = true;
		TimerUtils.DelayedAction(GetTree(), 150, () =>
		{
			GD.Print("Codex button pressed");
			FadeInCodex();
		});

	}

	private void FadeInCodex()
	{
		var transition = GetNode<ColorRect>("%TransitionScreen");
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(transition, "color:a", 1, 0.125f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
		tween.Chain().TweenCallback(Callable.From(() => GetNode<CanvasLayer>("%CodexUI").Visible = true));
		tween.Chain().TweenProperty(transition, "color:a", 0f, 0.25f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
		GetNode<TextureButton>("%CodexButton").Disabled = false;
	}
}
