namespace Stories.Models.User
{
    public interface ISignUpUserModel
    {
        string Email { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        string Username { get; set; }
    }
}
