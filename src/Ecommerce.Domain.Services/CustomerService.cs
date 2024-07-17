namespace Ecommerce.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void SaveCustomer(Customer customer)
        {
            ValidateEmail(customer.Email);

            _customerRepository.Insert(customer);
        }

        private void ValidateEmail(string email)
        {
            if (!IsEmailValid(email))
                throw new DuplicateEmailException(email);

            var existingCustomer = _customerRepository.GetByEmail(email);

            if (existingCustomer is not null)
                throw new DuplicateEmailException(email);
        }

        private bool IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var emailAddress = new MailAddress(email);

                if (emailAddress.Address != email)
                    return false;
                return CheckDomainHasMXRecord(emailAddress.Host);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CheckDomainHasMXRecord(string domain)
        {

            try
            {
                var lookup = new LookupClient();
                var result = lookup.Query(domain, QueryType.MX);
                return result.Answers.MxRecords().Any();
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
