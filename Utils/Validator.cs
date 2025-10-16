using System.Text.RegularExpressions;

namespace QLKS.Utils
{
    public static class Validator
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var regex = new Regex(@"^[\d\s\-\+\(\)]{10,15}$");
            return regex.IsMatch(phoneNumber);
        }

        public static bool IsValidIdentityNumber(string identityNumber)
        {
            if (string.IsNullOrWhiteSpace(identityNumber))
                return false;

            return identityNumber.Length >= 9 && identityNumber.Length <= 12;
        }

        public static bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return endDate > startDate;
        }

        public static bool IsValidPrice(decimal price)
        {
            return price >= 0;
        }

        public static ValidationResult ValidateCustomer(QLKS.Models.Customer customer)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(customer.FullName))
                result.AddError("Tên khách hàng không được để trống");

            if (!IsValidEmail(customer.Email))
                result.AddError("Email không hợp lệ");

            if (!IsValidPhoneNumber(customer.Phone))
                result.AddError("Số điện thoại không hợp lệ");

            if (!IsValidIdentityNumber(customer.IdCard))
                result.AddError("Số CMND/CCCD không hợp lệ");

            return result;
        }

        public static ValidationResult ValidateBooking(QLKS.Models.Booking booking)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(booking.CustomerId))
                result.AddError("Khách hàng không được để trống");

            if (string.IsNullOrWhiteSpace(booking.RoomId))
                result.AddError("Phòng không được để trống");

            if (!IsValidDateRange(booking.CheckInDate, booking.CheckOutDate))
                result.AddError("Ngày trả phòng phải sau ngày nhận phòng");

            if (booking.NumberOfGuests <= 0)
                result.AddError("Số lượng khách phải lớn hơn 0");

            return result;
        }
    }

    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public string GetErrorMessage()
        {
            return string.Join("; ", Errors);
        }
    }
}
