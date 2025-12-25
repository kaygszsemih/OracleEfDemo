namespace OracleEfDemo.Dtos
{
    public class ResetPasswordDto
    {
        public string? OldPassword { get; set; }

        public string NewPassword { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;
    }
}
