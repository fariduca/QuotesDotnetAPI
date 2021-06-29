using System.Collections.Generic;

namespace QuotesDotnetAPI.Services
{
    public static class SubscribersService
    {
        static List<string> EmailSubscribers {get; }
        static List<string> PhoneSubscribers {get; }

        static SubscribersService()
        {
            EmailSubscribers = new List<string> { "joe@doe.com", "jim@car.com", "some123@post.com" };
            PhoneSubscribers = new List<string> { "+999 111 2222", "+888 444 5555" };
        }

        public static void AddEmail(string email)
        {
            if (EmailSubscribers.Exists(s => s.Equals(email)))
                return;
            
            EmailSubscribers.Add(email);
        }

        public static void AddPhone(string phone)
        {
            if (PhoneSubscribers.Exists(s => s.Equals(phone)))
                return;
            
            PhoneSubscribers.Add(phone);
        }

        public static List<string> GetAllEmails() => EmailSubscribers;

        public static List<string> GetAllPhones() => PhoneSubscribers;
    }
}