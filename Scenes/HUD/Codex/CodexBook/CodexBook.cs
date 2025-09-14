using Godot;
using System.Text.Json;

public partial class CodexBook : ColorRect
{

	private int currentPageIndex = 0;
	Book currentBook;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadBookData(string bookId)
	{
		var file = FileAccess.Open($"res://Resources/Books/{bookId}.json", FileAccess.ModeFlags.Read);
		var json = file.GetAsText();
		currentBook = JsonSerializer.Deserialize<Book>(json);

		VBoxContainer tableOfContents = GetNode<VBoxContainer>("%TableOfContents");
		// Clear previous content
		foreach (var chapter in tableOfContents.GetChildren())
		{
			chapter.QueueFree();
		}
		foreach (var chapter in currentBook.Chapters)
		{
			Label chapterLabel = new Label();
			chapterLabel.MouseDefaultCursorShape = Control.CursorShape.PointingHand;
			chapterLabel.MouseFilter = Control.MouseFilterEnum.Pass;
			chapterLabel.Connect("gui_input", new Callable(this, nameof(OnChapterLabelGuiInput)));
			chapterLabel.Connect("mouse_entered", Callable.From(() => OnChapterLabelMouseEntered(chapterLabel)));

			chapterLabel.Text = chapter.Title;
			chapterLabel.AddThemeFontSizeOverride("font_size", 13);
			chapterLabel.AddThemeColorOverride("font_color", DisplayUtils.BookTextColor);
			chapterLabel.AddThemeFontOverride("font", GD.Load<Font>("res://Assets/Fonts/PIXELADE.ttf"));
			tableOfContents.AddChild(chapterLabel);
		}
		GetNode<Label>("%Title").Text = currentBook.BookTitle;
		GetNode<Label>("%Author").Text = currentBook.Author;
		file.Close();

	}

	public void DisplayBook()
	{
		ShowStartPages();
		DisplayUtils.FadeIn(this, 0.25f);
		TimerUtils.DelayedAction(GetTree(), 150, () =>
				{
					var bookContainer = GetNode<CenterContainer>("CenterContainer");
					// tween to book container position y = 0 over 0.25 seconds
					Tween tween = GetTree().CreateTween();
					tween.TweenProperty(bookContainer, "position:y", 0, 0.5f)
						.SetTrans(Tween.TransitionType.Quint)
						.SetEase(Tween.EaseType.Out);
				});
	}

	// When we click on a chapter in the table of contents, we want to hide the start pages and show the chapter pages
	private void OnChapterLabelGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left) HideStartPages();
	}

	private void HideStartPages()
	{
		var bookContainer = GetNode<HBoxContainer>("%BookContainer");
		bookContainer.GetChild<Control>(0).Visible = false;
		bookContainer.GetChild<Control>(1).Visible = false;

		bookContainer.GetChild<Control>(2).Visible = true;
		bookContainer.GetChild<Control>(3).Visible = true;

		currentPageIndex = 0;

		var leftPage = bookContainer.GetChild<Control>(2).GetChild<RichTextLabel>(0);
		var rightPage = bookContainer.GetChild<Control>(3).GetChild<RichTextLabel>(0);

		leftPage.Text = currentBook.Chapters[0].Pages[currentPageIndex].Content;
		rightPage.Text = currentBook.Chapters[0].Pages[currentPageIndex + 1].Content;



	}

	private void ShowStartPages()
	{
		var bookContainer = GetNode<HBoxContainer>("%BookContainer");
		bookContainer.GetChild<Control>(0).Visible = true;
		bookContainer.GetChild<Control>(1).Visible = true;

		bookContainer.GetChild<Control>(2).Visible = false;
		bookContainer.GetChild<Control>(3).Visible = false;
	}

	private void OnChapterLabelMouseEntered(Control chapterLabel)
	{
		// GD.Print($"Chapter {chapterLabel} hovered");
	}

	private void _on_close_book_button_pressed()
	{
		GetNode<TextureButton>("%CloseBookButton").Disabled = true;
		DisplayUtils.FadeOut(this, 0.25f);
		var bookContainer = GetNode<CenterContainer>("CenterContainer");
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(bookContainer, "position:y", -500, 0.25f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.In);
		tween.Chain().TweenCallback(Callable.From(() =>
		{
			Visible = false;
			GetNode<TextureButton>("%CloseBookButton").Disabled = false;
		}));


	}

}
