using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models
{
    public class CustomerResult
    {
        public bool IsNotAllowed
        {
            get;
            private set;
        }

        public bool IsLockedOut
        {
            get;
            private set;
        }

        public bool RequiresTwoFactor
        {
            get;
            protected set;
        }

        public Customer Customer
        {
            get;
        }

        public bool IsSucceeded
        {
            get;
            private set;
        }

        public bool IsFailed
        {
            get;
            private set;
        }

        private CustomerResult(Customer customer)
        {
            Customer = customer;
        }

        public static CustomerResult Failed() => new CustomerResult(null)
        {
            IsFailed = true
        };

        public static CustomerResult Succeeded(Customer customer) => new CustomerResult(customer)
        {
            IsSucceeded = true
        };

        public static CustomerResult NotAllowed(Customer customer) => new CustomerResult(customer)
        {
            IsNotAllowed = true
        };

        public static CustomerResult LockedOut(Customer customer) => new CustomerResult(customer)
        {
            IsLockedOut = true
        };

        public static CustomerResult TwoFactorRequired(Customer customer) => new CustomerResult(customer)
        {
            RequiresTwoFactor = true
        };
    }
}