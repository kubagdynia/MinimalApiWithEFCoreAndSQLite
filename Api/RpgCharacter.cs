namespace Api;

public record RpgCharacter
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RpgClass { get; set; } = string.Empty;
    public int HitPoints { get; set; } = 100;
    public string Weapon { get; set; } = string.Empty;
    public int Strength { get; set; } = 10;
    public int Defense { get; set; } = 10;
}