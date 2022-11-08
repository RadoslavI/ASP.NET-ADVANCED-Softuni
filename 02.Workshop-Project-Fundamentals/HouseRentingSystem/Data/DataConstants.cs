using Humanizer;

namespace HouseRentingSystem.Data
{
    static public class DataConstants
    {
        //Category constants
        public const int NameMaxLength = 50;

        //House constants
        public const int TitleMaxLength = 50;
        public const int TitleMinLength = 10;
        public const int AddressMinLength = 10;
        public const int AddressMaxLength = 150;
        public const int DescruptionMinLength = 50;
        public const int DescruptionMaxLength = 500;
        public const double PricePerMonthMin = 0;
        public const double PricePerMonthMax = 2000;


        //Agent constants
        public const int PhoneNumberMinLength = 7;
        public const int PhoneNumberMaxLength = 15;
    }
}
