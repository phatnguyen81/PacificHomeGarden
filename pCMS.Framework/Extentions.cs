﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;
using Telerik.Web.Mvc.UI.Fluent;

namespace pCMS.Framework
{
    public static class Extentions
    {
        public static GridBoundColumnBuilder<T> Centered<T>(this GridBoundColumnBuilder<T> columnBuilder) where T : class
        {
            return columnBuilder.HtmlAttributes(new { align = "center" })
                            .HeaderHtmlAttributes(new { style = "text-align:center;" });
        }

        public static GridBoundColumnBuilder<T> Right<T>(this GridBoundColumnBuilder<T> columnBuilder) where T : class
        {
            return columnBuilder.HtmlAttributes(new { align = "right" })
                            .HeaderHtmlAttributes(new { style = "text-align:right;" });
        }

        public static GridTemplateColumnBuilder<T> Centered<T>(this GridTemplateColumnBuilder<T> columnBuilder) where T : class
        {
            return columnBuilder.HtmlAttributes(new { align = "center" })
                            .HeaderHtmlAttributes(new { style = "text-align:center;" });
        }
        public static byte[] GetPictureBits(this HttpPostedFileBase postedFile)
        {
            var fs = postedFile.InputStream;
            var size = postedFile.ContentLength;
            var img = new byte[size];
            fs.Read(img, 0, size);
            return img;
        }

        public static IEnumerable<T> ForCommand<T>(this IEnumerable<T> current, GridCommand command)
        {
            var queryable = current.AsQueryable() as IQueryable;
            if (command.FilterDescriptors.Any())
            {
                queryable = queryable.Where(command.FilterDescriptors.AsEnumerable()).AsQueryable() as IQueryable;
            }

            IList<SortDescriptor> temporarySortDescriptors = new List<SortDescriptor>();

            if (!command.SortDescriptors.Any() && queryable.Provider.IsEntityFrameworkProvider())
            {
                // The Entity Framework provider demands OrderBy before calling Skip.
                var sortDescriptor = new SortDescriptor
                {
                    Member = queryable.ElementType.FirstSortableProperty()
                };
                command.SortDescriptors.Add(sortDescriptor);
                temporarySortDescriptors.Add(sortDescriptor);
            }

            if (command.GroupDescriptors.Any())
            {
                command.GroupDescriptors.Reverse().Each(groupDescriptor =>
                {
                    var sortDescriptor = new SortDescriptor
                    {
                        Member = groupDescriptor.Member,
                        SortDirection = groupDescriptor.SortDirection
                    };

                    command.SortDescriptors.Insert(0, sortDescriptor);
                    temporarySortDescriptors.Add(sortDescriptor);
                });
            }

            if (command.SortDescriptors.Any())
            {
                queryable = queryable.Sort(command.SortDescriptors);
            }

            return queryable as IQueryable<T>;
        }

        public static IEnumerable<T> PagedForCommand<T>(this IEnumerable<T> current, GridCommand command)
        {
            return current.Skip((command.Page - 1) * command.PageSize).Take(command.PageSize);
        }

        public static bool IsEntityFrameworkProvider(this IQueryProvider provider)
        {
            return provider.GetType().FullName == "System.Data.Objects.ELinq.ObjectQueryProvider";
        }

        public static bool IsLinqToObjectsProvider(this IQueryProvider provider)
        {
            return provider.GetType().FullName.Contains("EnumerableQuery");
        }

        public static string FirstSortableProperty(this Type type)
        {
            PropertyInfo firstSortableProperty = type.GetProperties().Where(property => property.PropertyType.IsPredefinedType()).FirstOrDefault();

            if (firstSortableProperty == null)
            {
                throw new NotSupportedException("Cannot find property to sort by.");
            }

            return firstSortableProperty.Name;
        }

        internal static bool IsPredefinedType(this Type type)
        {
            return PredefinedTypes.Any(t => t == type);
        }

        public static readonly Type[] PredefinedTypes = {
            typeof(Object),
            typeof(Boolean),
            typeof(Char),
            typeof(String),
            typeof(SByte),
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Convert)
        };
    }
}