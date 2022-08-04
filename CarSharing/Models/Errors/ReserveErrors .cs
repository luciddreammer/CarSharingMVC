namespace CarSharing.Models.Errors
{
    public class ReserveErrors
    {
        public static bool PastError { get; set; }
        public static bool MaxLengthError { get; set; }
        public static bool FromNullError { get; set; }
        public static bool ToNullError { get; set; }
        public static bool AnotherReservationError { get; set; }
    }
}