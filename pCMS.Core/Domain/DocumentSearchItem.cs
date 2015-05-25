using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pCMS.Core.Domain
{
    public enum DocumentType
    {
        News,
        ProductPicture
    }
    public class DocumentSearchItem
    {
        public Guid Id { get; set; }
        public DocumentType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Keywords { get; set; }
        public Guid ParentId { get; set; }
    }
}
