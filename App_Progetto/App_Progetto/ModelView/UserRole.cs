namespace App_Progetto.ModelView;

public class UserRole
{
    public int Id { get; set; }
    required public string UserId { get; set; }
    required public string UserName { get; set; }
    required public string Email { get; set; }
    public IEnumerable<string>? Roles { get; set; }
}
