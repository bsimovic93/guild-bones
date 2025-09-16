using Godot;
using System.Text.Json;
using System.Linq;

public partial class CodexBook : ColorRect
{

	private int currentPage = 0;
	private bool isOnStartPages = true;
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
			var chapterLabel = (ChapterLabel)GD.Load<PackedScene>("res://Scenes/HUD/Codex/CodexBook/ChapterLabel.tscn").Instantiate();
			chapterLabel.Text = chapter.Title;
			chapterLabel.ChapterIndex = currentBook.Chapters.IndexOf(chapter);
			chapterLabel.ChapterStartPage = chapter.Pages[0].Number;
			chapterLabel.ChapterClicked += GoToPage;
			tableOfContents.AddChild(chapterLabel);
		}
		GetNode<Label>("%Title").Text = currentBook.BookTitle;
		GetNode<Label>("%Author").Text = currentBook.Author;
		file.Close();

	}

	public void DisplayBook()
	{
		ToggleStartPage(true);
		isOnStartPages = true;
		currentPage = 0;
		GetNode<TextureButton>("%TurnLeft").Disabled = true;
		GetNode<TextureButton>("%TurnRight").Disabled = false;
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

	/// <summary>
	/// Go to a specific page in the book. The page on the left is odd, the page on the right is even.
	/// If the page number is out of range, do nothing.
	/// </summary>
	/// <param name="pageNumber"></param>
	public void GoToPage(int pageNumber)
	{
		GD.Print($"Going to page {pageNumber}");
		currentPage = pageNumber;


		int lastPageNumber = currentBook.Chapters.SelectMany(c => c.Pages).Max(p => p.Number);
		if (pageNumber + 2 >= lastPageNumber)
		{
			GetNode<TextureButton>("%TurnRight").Disabled = true;
		}
		
		if (pageNumber <= 0)
		{
			ToggleStartPage(true);
			isOnStartPages = true;
			currentPage = 0;
			GetNode<TextureButton>("%TurnLeft").Disabled = true;
			GetNode<TextureButton>("%TurnRight").Disabled = false;
			return;
		}


		var bookContainer = GetNode<HBoxContainer>("%BookContainer");
		ToggleStartPage(false);
		isOnStartPages = false;

		GetNode<TextureButton>("%TurnLeft").Disabled = false;


		var leftPage = bookContainer.GetChild<Control>(2).GetChild<RichTextLabel>(0);
		var rightPage = bookContainer.GetChild<Control>(3).GetChild<RichTextLabel>(0);

		var allPages = currentBook.Chapters.SelectMany(c => c.Pages).ToList();
		var left = allPages.FirstOrDefault(p => p.Number == pageNumber);
		var right = allPages.FirstOrDefault(p => p.Number == pageNumber + 1);

		leftPage.Text = left != null ? left.Content : "";
		rightPage.Text = right != null ? right.Content : "";

	}

	private void ToggleStartPage(bool showStartPages)
	{
		var bookContainer = GetNode<HBoxContainer>("%BookContainer");
		bookContainer.GetChild<Control>(0).Visible = showStartPages;
		bookContainer.GetChild<Control>(1).Visible = showStartPages;
		bookContainer.GetChild<Control>(2).Visible = !showStartPages;
		bookContainer.GetChild<Control>(3).Visible = !showStartPages;
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

	private void _on_turn_left_pressed()
	{
		currentPage -= 2;
		GoToPage(currentPage);
	}

	private void _on_turn_right_pressed()
	{
		currentPage += isOnStartPages ? 1 : 2;
		GoToPage(currentPage);
	}

}
