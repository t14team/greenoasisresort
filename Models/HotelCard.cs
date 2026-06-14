namespace GreenOasisResort.Models;

public class HotelCard
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string[] Amenities { get; set; } = [];
}
