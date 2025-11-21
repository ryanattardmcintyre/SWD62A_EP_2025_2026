using System.Text.Json;

namespace Presentation.Factory
{
    public class ImportItemFactory
    {
       /* public IItemValidating Create(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("type", out var typeProp))
                throw new ArgumentException("Missing 'type' field in json", nameof(json));

            var type = typeProp.GetString();

            return type?.ToLowerInvariant() switch
            {
                "restaurant" => JsonSerializer.Deserialize<Restaurant>(json)
                                ?? throw new InvalidOperationException("Could not deserialize Restaurant"),
                "menuitem" => JsonSerializer.Deserialize<MenuItem>(json)
                                ?? throw new InvalidOperationException("Could not deserialize MenuItem"),
                _ => throw new ArgumentOutOfRangeException(nameof(json),
                                $"Unknown type '{type}' in json")
            };
        }*/
    }
}
