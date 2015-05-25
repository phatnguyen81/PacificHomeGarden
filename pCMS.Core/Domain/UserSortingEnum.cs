using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pCMS.Core.Domain
{
    public enum UserSortingEnum
    {
        UserNameAsc = 0,
        UserNameDesc,
        FullNameAsc,
        FullNameDesc,
        EmailAsc,
        EmailDesc,
        IsApprovedAsc,
        IsApprovedDesc,
        IsLockedOutAsc,
        IsLockedOutDesc,
        CreationDateAsc,
        CreationDateDesc
    }
}
