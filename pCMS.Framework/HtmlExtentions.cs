using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Telerik.Web.Mvc.UI;

namespace pCMS.Framework
{
    public static class HtmlExtentions
    {

        public static MvcHtmlString DeleteConfirmation(this HtmlHelper helper, string id, string buttonsSelector = null) 
        {
            return DeleteConfirmation(helper,null, null, id, buttonsSelector);
        }

        // Adds an action name parameter for using other delete action names
        public static MvcHtmlString DeleteConfirmation(this HtmlHelper helper, string actionName, string controllerName, string id, string buttonsSelector = null) 
        {
            if (String.IsNullOrWhiteSpace(actionName))
                actionName = "Delete";
            if (string.IsNullOrWhiteSpace(controllerName))
                controllerName = helper.ViewContext.RouteData.GetRequiredString("controller");

            var modalId = MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-delete-confirmation").ToHtmlString();

            //there's an issue in Telerik (ScriptRegistrar.Current implemenation)
            //it's a little hack to ensure ScriptRegistrar.Current is loaded
            var test = helper.Telerik();

            #region Write click events for button, if supplied

            if (!string.IsNullOrEmpty(buttonsSelector))
            {
                var textWriter = new StringWriter();
                IClientSideObjectWriter objectWriter = new ClientSideObjectWriterFactory().Create(buttonsSelector, "click", textWriter);
                objectWriter.Start();
                textWriter.Write("function(e){e.preventDefault();openModalWindow(\"" + modalId + "\");}");
                objectWriter.Complete();
                var value = textWriter.ToString();
                ScriptRegistrar.Current.OnDocumentReadyStatements.Add(value);
            }

            #endregion

            var deleteConfirmationModel = new DeleteConfirmationModel
            {
                Id = id,
                ControllerName = controllerName,
                ActionName = actionName
            };

            var window = helper.Telerik().Window().Name(modalId)
                .Title("Confirmation")
                .Modal(true)
                .Effects(x => x.Toggle())
                .Resizable(x => x.Enabled(false))
                .Buttons(x => x.Close())
                .Visible(false)
                .Content(helper.Partial("Delete", deleteConfirmationModel).ToHtmlString()).ToHtmlString();

            return MvcHtmlString.Create(window);
        }

        public static MvcHtmlString FormConfirmation(this HtmlHelper helper, string id, string buttonsSelector = null)
        {
            return FormConfirmation(helper, null, null, id, "Are you sure?", buttonsSelector);
        }

        // Adds an action name parameter for using other delete action names
        public static MvcHtmlString FormConfirmation(this HtmlHelper helper, string actionName, string controllerName, string id, string message = null, string buttonsSelector = null)
        {
            if (String.IsNullOrWhiteSpace(actionName))
                actionName = "Delete";
            if (string.IsNullOrWhiteSpace(controllerName))
                controllerName = helper.ViewContext.RouteData.GetRequiredString("controller");

            var modalId = MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-form-confirmation").ToHtmlString();

            //there's an issue in Telerik (ScriptRegistrar.Current implemenation)
            //it's a little hack to ensure ScriptRegistrar.Current is loaded
            var test = helper.Telerik();

            #region Write click events for button, if supplied

            if (!string.IsNullOrEmpty(buttonsSelector))
            {
                var textWriter = new StringWriter();
                IClientSideObjectWriter objectWriter = new ClientSideObjectWriterFactory().Create(buttonsSelector, "click", textWriter);
                objectWriter.Start();
                textWriter.Write("function(e){e.preventDefault();openModalWindow(\"" + modalId + "\");}");
                objectWriter.Complete();
                var value = textWriter.ToString();
                ScriptRegistrar.Current.OnDocumentReadyStatements.Add(value);
            }

            #endregion

            var formConfirmationModel = new FormConfirmationModel
            {
                Id = id,
                ControllerName = controllerName,
                ActionName = actionName,
                Message = message
            };

            var window = helper.Telerik().Window().Name(modalId)
                .Title("Confirmation")
                .Modal(true)
                .Effects(x => x.Toggle())
                .Resizable(x => x.Enabled(false))
                .Buttons(x => x.Close())
                .Visible(false)
                .Content(helper.Partial("FormConfirmation", formConfirmationModel).ToHtmlString()).ToHtmlString();

            return MvcHtmlString.Create(window);
        }
    }
}