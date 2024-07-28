namespace Api;

public record RpgCharacter
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RpgClass { get; set; } = string.Empty;
    public int HitPoints { get; set; } = 100;
}