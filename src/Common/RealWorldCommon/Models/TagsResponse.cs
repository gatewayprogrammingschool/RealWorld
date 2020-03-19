// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace RealWorldCommon.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class TagsResponse
    {
        /// <summary>
        /// Initializes a new instance of the TagsResponse class.
        /// </summary>
        public TagsResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TagsResponse class.
        /// </summary>
        public TagsResponse(IList<string> tags)
        {
            Tags = tags;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public IList<string> Tags { get; set; }


        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "counts")]
        public IList Counts { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Tags == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Tags");
            }
        }
    }
}
