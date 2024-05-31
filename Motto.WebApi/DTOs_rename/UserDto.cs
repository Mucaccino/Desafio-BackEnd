using Motto.Enums;

public class UserDto {
    
    public int Id { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public UserType Type { get; set; }

}