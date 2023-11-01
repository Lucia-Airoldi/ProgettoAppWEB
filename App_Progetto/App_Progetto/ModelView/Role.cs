namespace App_Progetto.ModelView
{
    public class Role
    {
        required public string UserId { get; set; }
        required public string UserName { get; set; }
        public List<RoleViewUser>? Roles { get; set; }
    }
}
