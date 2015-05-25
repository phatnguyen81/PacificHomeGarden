using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using pCMS.Core.Domain;

namespace pCMS.Services
{
    public interface IExportManager
    {
        byte[] ExportCustomersToXlsx(IList<PCmsUser> users);
    }
    public class ExportManager : IExportManager
    {
        private readonly IUserService _userService;

        public ExportManager(IUserService userService)
        {
            _userService = userService;
        }

        public byte[] ExportCustomersToXlsx(IList<PCmsUser> users)
        {
            //var newFile = new FileInfo(filePath);

            using (var xlPackage = new ExcelPackage())
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Customers");
                var properties = new string[]
                                     {
                                         "Username",
                                         "Email",
                                         "FullName",
                                         "Address",
                                         "IsApproved",
                                         "IsLockedOut",
                                         "TimeZoneId",
                                         "ActivationType",
                                         "PhoneNumber",
                                         "Resale",
                                         "BusinessDescription",
                                         "BusinessName",
                                         "TaxpaperId",
                                         "CreationDate"
                                     };
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }
                var row = 2;
                foreach (var user in users)
                {
                    var col = 1;

                    worksheet.Cells[row, col].Value = user.UserName;
                    col++;

                    worksheet.Cells[row, col].Value = user.Email;
                    col++;

                    worksheet.Cells[row, col].Value = user.FullName ;
                    col++;

                    worksheet.Cells[row, col].Value = user.Address;
                    col++;

                    worksheet.Cells[row, col].Value = user.IsApproved.ToString();
                    col++;

                    worksheet.Cells[row, col].Value = user.IsLockedOut.ToString();
                    col++;

                    worksheet.Cells[row, col].Value = user.TimeZoneId;
                    col++;

                    worksheet.Cells[row, col].Value = user.ActivationType;
                    col++;

                    worksheet.Cells[row, col].Value = user.PhoneNumber;
                    col++;

                    worksheet.Cells[row, col].Value = user.Resale;
                    col++;

                    worksheet.Cells[row, col].Value = user.BusinessDescription;
                    col++;

                    worksheet.Cells[row, col].Value = user.BusinessName;
                    col++;

                    worksheet.Cells[row, col].Value = user.TaxpaperId;
                    col++;

                    worksheet.Cells[row, col].Style.Numberformat.Format = @"mm\/dd\/yyyy\ hh:mm"; 
                    worksheet.Cells[row, col].Value = user.CreationDate;

                    row++;
                }
                xlPackage.Workbook.Properties.Title = string.Format("{0} customers", "abc");
                xlPackage.Workbook.Properties.Author = "abc";
                xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", "abc");
                xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", "abc");
                xlPackage.Workbook.Properties.Category = "Customers";
                xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", "abc");

                // set some extended property values
                xlPackage.Workbook.Properties.Company = "abc";

                // save the new spreadsheet
                var results = xlPackage.GetAsByteArray();
                xlPackage.Dispose();
                return results;
            }
        }
    }
}
