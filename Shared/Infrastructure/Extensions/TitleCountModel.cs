namespace myuzbekistan.Shared;

public class TitleCountModel
{
    public int Count { get; set; } = 0;
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public bool Linkable { get; set; }
}

public enum TitleContainerType
{
    Single = 1,
    Double,
    Triple,
    Quadruple,
    Sextuple,
}