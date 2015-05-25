using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using HtmlAgilityPack;

namespace pCMS.Core.Utils
{
    public class StringHelpers
    {
        public static string MakeSEOTitle(string input)
        {

            input = RemoveAccents(input);
            input = ConvertToUnSign(input);

            input = Regex.Replace(input.Trim(), @"\W", " "); //replace special chars
            input = Regex.Replace(input.Trim(), @"\s{2,}", " "); //replace double space
            input = input.Trim().Replace(" ", "-").ToLower();

            return input; //return - delimited title
        }
        public static string RemoveAccents(string input)
        {
            var normalized = input.Normalize(NormalizationForm.FormKD);
            var removal = Encoding.GetEncoding
                (Encoding.ASCII.CodePage,
                 new EncoderReplacementFallback(""),
                 new DecoderReplacementFallback(""));
            var bytes = removal.GetBytes(normalized);
            return Encoding.ASCII.GetString(bytes);
        }
        public static string ConvertToUnSign(string s)
        {
            var stFormD = s.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var t in stFormD)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static string ConvertDbToDisplay(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return content;
            var webHelper = DependencyResolver.Current.GetService<IWebHelper>();
            content = content.Replace("{$HOST$}", webHelper.GetRootUrl());
            return content;
        }

        public static string ConvertContentToDb(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return content;
            var webHelper = DependencyResolver.Current.GetService<IWebHelper>();
            var html = new HtmlDocument();
            html.LoadHtml(content);
            var imgList = html.DocumentNode.SelectNodes("//img[@src]");
            if (imgList == null) return content;
            foreach (var node in imgList)
            {
                if (node.Attributes != null
                    && node.Attributes["type"] != null
                    && node.Attributes["type"].Value == "internal"
                    && node.Attributes["src"] != null)
                {
                    var src = node.Attributes["src"].Value;
                    src = src.Replace(webHelper.GetRootUrl(), "{$HOST$}");
                    node.Attributes["src"].Value = src;
                }
            }
            return html.DocumentNode.OuterHtml;
        }
    }
}