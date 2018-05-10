using System;

namespace EdiTools.Tests
{
    public class Organization
    {
        public Organization(OrganizationName name, AddressInformation addressInformation1, AddressInformation addressInformation2,
                            CityName city, StateOrProvince stateOrProvince, PostalCode postalCode, CountryCode countryCode,
                            IdentificationCodeQualifier identificationCodeQualifier, IdentificationCode identificationCode)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AddressInformation1 = addressInformation1 ?? throw new ArgumentNullException(nameof(addressInformation1));
            AddressInformation2 = addressInformation2 ?? throw new ArgumentNullException(nameof(addressInformation2));
            City = city ?? throw new ArgumentNullException(nameof(city));
            StateOrProvince = stateOrProvince ?? throw new ArgumentNullException(nameof(stateOrProvince));
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
            CountryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode));
            IdentificationCodeQualifier = identificationCodeQualifier;
            IdentificationCode = identificationCode ?? throw new ArgumentNullException(nameof(identificationCode));
        }
        public OrganizationName Name { get; }
        public AddressInformation AddressInformation1 { get; }
        public AddressInformation AddressInformation2 { get; }
        public CityName City { get; }
        public StateOrProvince StateOrProvince { get; }
        public PostalCode PostalCode { get; }
        public CountryCode CountryCode { get; }
        public IdentificationCodeQualifier IdentificationCodeQualifier { get; }
        public IdentificationCode IdentificationCode { get; }
    }
}