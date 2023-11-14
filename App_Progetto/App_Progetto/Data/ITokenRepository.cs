namespace App_Progetto.Models
{
    public interface ITokenRepository
    {
        Token GenerateToken(int days);
    }
}
