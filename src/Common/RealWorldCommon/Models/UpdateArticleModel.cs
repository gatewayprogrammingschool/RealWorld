// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace RealWorldCommon.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class UpdateArticleModel
    {
        /// <summary>
        /// Initializes a new instance of the UpdateArticle class.
        /// </summary>
        public UpdateArticleModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UpdateArticle class.
        /// </summary>
        public UpdateArticleModel(string title = default(string), string description = default(string), string body = default(string))
        {
            Title = title;
            Description = description;
            Body = body;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

    }
}