using System;
using System.ComponentModel.DataAnnotations;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class AlbumListModel
    {
        public GridModel<AlbumModel> Albums { get; set; }

    }
    
    public class AlbumModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Album.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Album.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Content.Album.Fields.Description")]
        public string Description { get; set; }

        [ResourceDisplayName("Admin.Content.Album.Fields.IsPublished")]
        public bool IsPublished { get; set; }
    }

    public class AlbumItemModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Album.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Album.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Content.Album.Fields.Description")]
        public string Description { get; set; }

        [ResourceDisplayName("Admin.Content.Album.Fields.IsPublished")]
        public bool IsPublished { get; set; }

        public class PictureListModel
        {
            [Editable(false)]
            public Guid PictureId { get; set; }

            public Guid AlbumId { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Content.AlbumPicture.Fields.MineType")]
            public string MineType { get; set; }

            [ResourceDisplayName("Admin.Content.AlbumPicture.Fields.Description")]
            public string Description { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Content.AlbumPicture.Fields.PictureUrl")]
            public string PictureUrl { get; set; }

            [UIHint("DisplayOrder")]
            [ResourceDisplayName("Admin.Content.AlbumPicture.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }
    }
}