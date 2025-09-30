class Character
{
    public UInt64 Ids { get; set; }
    public string Names { get; set; } = string.Empty;
    public string Descriptions { get; set; } = string.Empty;
    public string FirstAppearances { get; set; } = string.Empty;
    public UInt64 YearsCreated { get; set; }

    public string Display()
    {
        return $"Id: {Ids}\nName: {Names}\nDescritpion: {Descriptions}\nFirst Appearance: {FirstAppearances}\nYear Created: {YearsCreated}\n";
    }
}