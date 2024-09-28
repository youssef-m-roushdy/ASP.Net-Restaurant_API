using System;
namespace efcoremongodb.Dtos.AccountDto
{
    public class RegisterResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}

