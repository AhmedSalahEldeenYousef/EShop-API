namespace Eshop.Core.DTO
{
    public record LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public record RegisterDto : LoginDto
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        //TODO: Uncomment these properties if needed in the future

        //public string ConfirmPassword { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
    }

    public record RestPasswordDto : LoginDto
    {
        public string Token { get; set; }

    }

    public record AcctiveAccountDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
