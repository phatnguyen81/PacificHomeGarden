using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pCMS.Framework
{
    public class DeleteConfirmationModel
    {
        public string Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
    public class FormConfirmationModel
    {
        public string Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Message { get; set; }
    }
}