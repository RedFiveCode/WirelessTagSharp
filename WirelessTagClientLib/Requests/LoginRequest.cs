namespace WirelessTagClientLib.Requests
{
    /// <summary>
    /// Request to login
    /// </summary>
    internal class LoginRequest : JsonPostRequest
    {
        public LoginRequest() : base("/ethAccount.asmx/SignIn") { }

        public string Email { get; set; }
        public string Password { get; set; }

        protected override object GetRequestBody()
        {
            return new { email = Email, password = Password };
        }
    }
}
