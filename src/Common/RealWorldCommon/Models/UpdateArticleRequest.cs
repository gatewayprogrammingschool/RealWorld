// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace RealWorldCommon.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class UpdateArticleRequest
    {
        /// <summary>
        /// Initializes a new instance of the UpdateArticleRequest class.
        /// </summary>
        public UpdateArticleRequest()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UpdateArticleRequest class.
        /// </summary>
        public UpdateArticleRequest(UpdateArticleModel article)
        {
            Article = article;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "article")]
        public UpdateArticleModel Article { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Article == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Article");
            }
        }
    }
}
