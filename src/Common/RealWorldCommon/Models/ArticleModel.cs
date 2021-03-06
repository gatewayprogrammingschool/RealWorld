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

    public partial class ArticleModel
    {
        /// <summary>
        /// Initializes a new instance of the Article class.
        /// </summary>
        public ArticleModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Article class.
        /// </summary>
        public ArticleModel(string slug, string title, string description, string body, IList<string> tagList, System.DateTime createdAt, System.DateTime updatedAt, bool favorited, int favoritesCount, ProfileModel author)
        {
            Slug = slug;
            Title = title;
            Description = description;
            Body = body;
            TagList = tagList;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Favorited = favorited;
            FavoritesCount = favoritesCount;
            Author = author;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }

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

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tagList")]
        public IList<string> TagList { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "createdAt")]
        public System.DateTime CreatedAt { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "updatedAt")]
        public System.DateTime UpdatedAt { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "favorited")]
        public bool Favorited { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "favoritesCount")]
        public int FavoritesCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "author")]
        public ProfileModel Author { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Slug == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Slug");
            }
            if (Title == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Title");
            }
            if (Description == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Description");
            }
            if (Body == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Body");
            }
            if (TagList == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "TagList");
            }
            if (Author == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Author");
            }
            if (Author != null)
            {
                Author.Validate();
            }
        }
    }
}
