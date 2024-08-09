namespace GameStore.api.Dtos;

public record class GameDto(
    int Id, 
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate);