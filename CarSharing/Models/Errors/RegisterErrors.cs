namespace CarSharing.Models.Errors
{
    public class RegisterErrors
    {
        public static bool nameError { get; set; }
        public static bool emailError { get; set; }
        public static bool passwordError { get; set; }
        public static bool phoneError { get; set; }
        public static bool loginError { get; set; }
        public static bool dateOfBirthError { get; set; }
        public static bool cityError { get; set; }
        public static bool loginExists { get; set; }
        public static bool emailExists { get; set; }
    }
}
