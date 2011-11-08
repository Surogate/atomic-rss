
namespace atomic.rss.Web.BD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies ArticlesMetadata as the class
    // that carries additional metadata for the Articles class.
    [MetadataTypeAttribute(typeof(Articles.ArticlesMetadata))]
    public partial class Articles
    {

        // This class allows you to attach custom attributes to properties
        // of the Articles class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ArticlesMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ArticlesMetadata()
            {
            }

            public Channels Channels { get; set; }

            public int Channels_ID { get; set; }

            public DateTime Date { get; set; }

            public string Description { get; set; }

            public string GUID { get; set; }

            public int Id { get; set; }

            public string Link { get; set; }

            public string Title { get; set; }

            public EntityCollection<Users> Users { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies ChannelsMetadata as the class
    // that carries additional metadata for the Channels class.
    [MetadataTypeAttribute(typeof(Channels.ChannelsMetadata))]
    public partial class Channels
    {

        // This class allows you to attach custom attributes to properties
        // of the Channels class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ChannelsMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ChannelsMetadata()
            {
            }

            public EntityCollection<Articles> Articles { get; set; }

            public string Author { get; set; }

            public DateTime Date { get; set; }

            public string Description { get; set; }

            public int Id { get; set; }

            public string Language { get; set; }

            public string Link { get; set; }

            public string Title { get; set; }

            public int UpdateFrequency { get; set; }

            public EntityCollection<Users> Users { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies UsersMetadata as the class
    // that carries additional metadata for the Users class.
    [MetadataTypeAttribute(typeof(Users.UsersMetadata))]
    public partial class Users
    {

        // This class allows you to attach custom attributes to properties
        // of the Users class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UsersMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UsersMetadata()
            {
            }

            public EntityCollection<Articles> Articles { get; set; }

            public EntityCollection<Channels> Channels { get; set; }

            public string Email { get; set; }

            public int Id { get; set; }

            public bool IsAdmin { get; set; }

            public string Passwords { get; set; }
        }
    }
}
