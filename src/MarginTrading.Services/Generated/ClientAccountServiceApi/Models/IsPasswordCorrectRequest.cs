// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace MarginTrading.Services.Generated.ClientAccountServiceApi.Models
{
    using System.Linq;

    public partial class IsPasswordCorrectRequest
    {
        /// <summary>
        /// Initializes a new instance of the IsPasswordCorrectRequest class.
        /// </summary>
        public IsPasswordCorrectRequest() { }

        /// <summary>
        /// Initializes a new instance of the IsPasswordCorrectRequest class.
        /// </summary>
        public IsPasswordCorrectRequest(string clientId, string password)
        {
            ClientId = clientId;
            Password = password;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ClientId == null)
            {
                throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.CannotBeNull, "ClientId");
            }
            if (Password == null)
            {
                throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.CannotBeNull, "Password");
            }
        }
    }
}
