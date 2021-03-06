using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ActivitySystem.Models;
using ActivitySystem.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ActivitySystem.Repository
{
    public class UsersRepository : BaseContext
    {
        public IEnumerable<tblUsers> GetAllUsersSuperAdmin(int AccountID)
        {
            
            return _context.tblUsers.Include(C => C.College).Include(R => R.Role).Where(U => U.IsGraduate == false && U.Id != AccountID);
        }
        public IEnumerable<tblUsers> GetAllUsersAdmin(int collegeid, int AccountID)
        {

            return _context.tblUsers.Include(C => C.College).Include(R => R.Role).Where(U => U.IsGraduate == false && U.CollegeId == collegeid && U.Id != AccountID);
        }


        public int UserInsert(tblUsers userInfo)
        {
            try
            {
                tblUsers userInfoByKfuEmail = _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.KfuEmail == userInfo.KfuEmail);
                if (userInfoByKfuEmail != null)
                    return 2; // user is already there
                userInfo.GuId = Guid.NewGuid();
                userInfo.IsActive = false;
                if(userInfo.RoleId == 1)
                {
                    userInfo.CollegeId = null;
                }
                _context.Add(userInfo);
                _context.SaveChanges();
                return 1; // Add Successfuly
            }
            catch
            {
                return 0; //Failed
            }
        }
        public IEnumerable<tblRoles> GetAllRoles()
        {
            return _context.tblRoles;
        }

        public IEnumerable<tblRoles> GetAllRolesForAdmin()
        {
            return _context.tblRoles.Where(R => R.Id != 1);
        }
        public IEnumerable<tblUsers> GetAllStudents()
        {
            return _context.tblUsers.Where(U => U.RoleId == 4 && U.IsActive == true && U.IsGraduate == false);
        }
        public IEnumerable<tblUsers> GetAllInstuctors()
        {
            return _context.tblUsers.Where(U => U.RoleId == 3 && U.IsActive == true && U.IsGraduate == false);
        }
        public IEnumerable<tblUsers> GetAllAdmins()
        {
            return _context.tblUsers.Where(U => U.RoleId == 1 && U.IsActive == true && U.IsGraduate == false);
        }
        public IEnumerable<tblColleges> GetAllColleges()
        {
            return _context.tblColleges;
        }
        public tblUsers GetUserInfoByEmail(string Email)
        {
            return _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.KfuEmail == Email);
        }
        public tblUsers GetUserByGuId(Guid? guid)
        {
            return _context.tblUsers.Include(C => C.College).Include(R => R.Role).AsNoTracking().SingleOrDefault(U => U.GuId == guid); 
        }
        public tblUsers GetUserById(int id)
        {
            return _context.tblUsers.Include(C => C.College).Include(R => R.Role).SingleOrDefault(U => U.Id == id);
        }
        public int CheckUserPassword(int id, string Password)
        {
            tblUsers UserInfo = _context.tblUsers.SingleOrDefault(U => U.Password == Password && U.Id == id);
            if (UserInfo == null)
                return 0;
            else
                return 1;
        }
        public int DeactivateUser(tblUsers UserInfo)
        {
            try
            {
                UserInfo.Password = "";
                UserInfo.Name = "";
                UserInfo.IsActive = false;
                _context.Update(UserInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int ChangeUserName(int id, string username)
        {
            try { 
            tblUsers UserInfo = _context.tblUsers.SingleOrDefault(U => U.Id == id);
            UserInfo.Name = username;
            _context.Update(UserInfo);
            _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public tblUsers GetAccountsForLogin(tblUsers userinfo,string Password)
        {
            return _context.tblUsers.Include(R => R.Role).SingleOrDefault(U => U.KfuEmail == userinfo.KfuEmail && U.Password == Password);
        }
        public int ActivateUser(tblUsers userinfo,string Password)
        {
            try
            {
                tblUsers userInfoByKfuEmail = _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.KfuEmail == userinfo.KfuEmail);
                userInfoByKfuEmail.Name = userinfo.Name;
                userInfoByKfuEmail.Password = Password;
                userInfoByKfuEmail.IsActive = true;
                _context.Update(userInfoByKfuEmail);
                _context.SaveChanges();
                return 1; // Update Successfuly
            }
            catch
            {
                return 0; // Update Failed
            }
        }

        
        public int ForgatPassword(tblUsers userinfo,string Password)
        {
            try
            {
                tblUsers userInfoByKfuEmail = _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.GuId == userinfo.GuId);
                userInfoByKfuEmail.Password = Password;
                _context.Update(userInfoByKfuEmail);
                _context.SaveChanges();
                return 1; // Update Successfuly
            }
            catch
            {
                return 0; // Update Failed
            }
        }
        public tblUsers checkemailuserforRecoverPassword(string KfuEmail)
        {
            // check the activate
            tblUsers studentInfo = GetUserInfoByEmail(KfuEmail);
            if (studentInfo != null)
            {
                return studentInfo;
            }

            return null;
        }

        public int UpdateUserInfoByAdmin(tblUsers UserInfo)
        {
            try
            {
                tblUsers UserByGuId = GetUserByGuId(UserInfo.GuId);
                UserByGuId.Name = UserInfo.Name;
                UserByGuId.KfuEmail = UserInfo.KfuEmail;
                UserByGuId.RoleId = UserInfo.RoleId;
                UserByGuId.CollegeId = UserInfo.CollegeId;
                _context.Entry(UserByGuId).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int ChangePassword(int AccountID, string password)
        {
            try
            {
                tblUsers UserInfoByID = GetUserById(AccountID);
                UserInfoByID.Password = password;
                _context.Update(UserInfoByID);
                _context.SaveChanges();
                return 1; 
            }
            catch
            {
                return 0;
            }
            
        }
        public int DeleteAccount(Guid? Id)
        {
            try
            {
                tblUsers UserInfoByGuid = GetUserByGuId(Id);
                UserInfoByGuid.IsGraduate = true;
                _context.Update(UserInfoByGuid);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }

        }
    }
}
