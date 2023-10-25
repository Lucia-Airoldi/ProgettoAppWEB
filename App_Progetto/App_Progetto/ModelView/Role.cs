namespace App_Progetto.ModelView
{
    public class Role
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleViewUser> Roles { get; set; }
    }
}
