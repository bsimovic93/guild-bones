using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Book
{
    [JsonPropertyName("book_title")]
    public string BookTitle { get; set; }
    [JsonPropertyName("author")]
    public string Author { get; set; }
    [JsonPropertyName("chapters")]
    public List<Chapter> Chapters { get; set; }
}

public class Chapter
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("pages")]
    public List<Page> Pages { get; set; }
}

public class Page
{
    [JsonPropertyName("number")]
    public int Number { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
}