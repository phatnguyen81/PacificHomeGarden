using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Core.Domain;
using pCMS.Data;

namespace pCMS.Services
{
    public interface IUserService
    {
        PCmsUser GetUser(string username);
        Guid CreateTokenResetPassword(string username);
        Guid GetTokenResetPassword(string username);
        string ResetPassword(string username);
        void ChangePassword(string username, string newpassword);
        List<PCmsUser> FindUserByEmail(string email, UserSortingEnum userSorting, int pageIndex, int pageSize, out int totalRecord);
        List<PCmsUser> FindUserByName(string username, UserSortingEnum userSorting, int pageIndex, int pageSize, out int totalRecord);

        void Add(PCmsUser user);
        void Update(PCmsUser user);
    }
    public class UserService : IUserService, IDisposable
    {
        private readonly IDalContext _context;

        public UserService(IDalContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
        }

        public PCmsUser GetUser(string username)
        {
            return _context.Users.Find(username);
        }

        public Guid CreateTokenResetPassword(string username)
        {
            return _context.Users.CreateTokenResetPassword(username);
        }

        public Guid GetTokenResetPassword(string username)
        {
            return _context.Users.GetTokenResetPassword(username);
        }

        public string ResetPassword(string username)
        {
            return _context.Users.ResetPassowrd(username);
        }

        public void ChangePassword(string username, string newpassword)
        {
            _context.Users.ChangePassowrd(username, newpassword); 
        }

        public List<PCmsUser> FindUserByEmail(string email, UserSortingEnum userSorting, int pageIndex, int pageSize, out int totalRecord)
        {
            return _context.Users.FindUserByEmail(email, userSorting, pageIndex, pageSize, out totalRecord);
        }

        public List<PCmsUser> FindUserByName(string username, UserSortingEnum userSorting, int pageIndex, int pageSize, out int totalRecord)
        {
            return _context.Users.FindUserByName(username, userSorting, pageIndex, pageSize, out totalRecord);
        }

        public void Add(PCmsUser user)
        {
            _context.Users.Create(user);
        }

        public void Update(PCmsUser user)
        {
            _context.Users.Update(user);
        }
    }
}
