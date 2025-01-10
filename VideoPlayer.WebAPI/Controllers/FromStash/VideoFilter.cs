public sealed record VideoFilter(string Name, string Value)
{
    public override string ToString() => $"{Name}={Value}";
}