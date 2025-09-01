namespace myuzbekistan.Shared;

public class CreateReviewDto
{
    public long ContentId { get; set; }
    public string? Comment { get; set; }
    public int Rating { get; set; }
}