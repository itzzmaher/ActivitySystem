using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivitySystem.Models;
using ActivitySystem.Repository.Common;
using Microsoft.EntityFrameworkCore;
namespace ActivitySystem.Repository
{
    public class ActivityRepository : BaseContext
    {

        public int ActivityInsert(tblActivities ActivityInfo, int AccountId)
        {
            try
            {
                ActivityInfo.GuId = Guid.NewGuid();
                ActivityInfo.CreatorId = AccountId;
                ActivityInfo.CreatedDate = DateTime.Now;
                ActivityInfo.IsActive = true;
                ActivityInfo.IsOpen = false;
                //LogInsert(ActivityInfo);
                _context.Add(ActivityInfo);
                _context.SaveChanges();
                return 1; // Add Successfuly
            }
            catch
            {
                return 0; //Failed
            }
        }
        public int UpdateActivityInstructor(tblActivities ActivityInfo)
        {
            try
            {
                tblActivities ActivityByGuid = GetActivityByGuId(ActivityInfo.GuId);
                ActivityByGuid.ActivityName = ActivityInfo.ActivityName;
                ActivityByGuid.MaxStudents = ActivityInfo.MaxStudents;
                ActivityByGuid.SemesterId = ActivityInfo.SemesterId;
                ActivityByGuid.StartDate = ActivityInfo.StartDate;
                ActivityByGuid.EndDate = ActivityInfo.EndDate;
                ActivityByGuid.RegisterStartDate = ActivityInfo.RegisterStartDate;
                ActivityByGuid.RegisterEndDate = ActivityInfo.RegisterEndDate;
                ActivityByGuid.Content = ActivityInfo.Content;
                ActivityLogInsert(ActivityByGuid);
                _context.Update(ActivityByGuid);
                _context.SaveChanges();
                return 1; // Updated Successfuly
            }
            catch (Exception st)
            {
                return 0; // Update Failed
            }
        }
        public int UpdateActivityAdmin(tblActivities ActivityInfo)
        {
            try
            {
                tblActivities ActivityByGuid = GetActivityByGuId(ActivityInfo.GuId);
                ActivityByGuid.ActivityName = ActivityInfo.ActivityName;
                ActivityByGuid.MaxStudents = ActivityInfo.MaxStudents; 
                ActivityByGuid.SupervisorId = ActivityInfo.SupervisorId;
                ActivityByGuid.SemesterId = ActivityInfo.SemesterId;
                ActivityByGuid.StartDate = ActivityInfo.StartDate;
                ActivityByGuid.EndDate = ActivityInfo.EndDate;
                ActivityByGuid.RegisterStartDate = ActivityInfo.RegisterStartDate;
                ActivityByGuid.RegisterEndDate = ActivityInfo.RegisterEndDate;
                ActivityByGuid.Content = ActivityInfo.Content;
                ActivityLogInsert(ActivityByGuid);
                _context.Entry(ActivityByGuid).State = EntityState.Modified;
                _context.SaveChanges();
                return 1; // Updated Successfuly
            }
            catch (Exception st)
            {
                return 0; // Update Failed
            }
        }
        public IEnumerable<tblActivities> GetAllActivities()
        {
            return _context.tblActivities.Include(U => U.Supervisor).Include(S =>S.Semester);
        }
        public IEnumerable<tblSemesters> GetAllSemestersForRegisteration()
        {
            return _context.tblSemesters.Where(S => S.IsDone != true);
        }
        public IEnumerable<tblSemesters> GetAllSemesters()
        {
            return _context.tblSemesters;
        }
        public IEnumerable<tblActivities> GetAllActiveActivities(int AccountId)
        {
            return _context.tblActivities.Include(S => S.Supervisor).Where(U => U.Supervisor.Id == AccountId);
        }
        public IEnumerable<tblActivities> GetAllOpenActivites()
        {
            return _context.tblActivities.Where(A => A.IsActive == true);
        }
        public IEnumerable<tblActivities> GetAllOpenActivities(int AccountId)
        {

            //tblRegisterations Reginfo = _context.tblRegisterations.Where(U => U.UserId == AccountId).ToList();
            return _context.tblActivities.Include(S => S.Supervisor).Include(S => S.Semester).Where
                (A => A.IsOpen == true && A.IsActive == true && A.Semester.IsActive == true && (A.RegisterStartDate < DateTime.Today && A.RegisterEndDate > DateTime.Today));
        }
        public tblActivities GetActivityByID(int Id)
        {
            return _context.tblActivities.AsNoTracking().SingleOrDefault(U => U.Id == Id);
        }
        public int ActivityLogInsert(tblActivities ActivityInfo)
        {
            try
            {
                tblActivityLogs LogInfo = new tblActivityLogs();
                LogInfo.ActivityId = ActivityInfo.Id;
                LogInfo.UpdateDate = DateTime.Now;
                LogInfo.UserId = ActivityInfo.CreatorId;
                _context.Add(LogInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0; //Failed
            }
        }
        public tblActivities GetActivityByGuId(Guid? guid)
        {
            return _context.tblActivities.Include(S => S.Supervisor).AsNoTracking().SingleOrDefault(U => U.GuId == guid);
        }
        public tblActivities GetActivityByRegisterGuId(Guid? guid)
        {
            tblRegisterations RegInfo = _context.tblRegisterations.AsNoTracking().SingleOrDefault(U => U.GuId == guid);
            return _context.tblActivities.AsNoTracking().Include(S => S.Supervisor).SingleOrDefault(U => U.Id == RegInfo.ActivityId);
        }
        public IEnumerable<tblRegisterations> GetAllRegisteredStudents(Guid? guid)
        {
            tblActivities ActivityInfo = _context.tblActivities.AsNoTracking().SingleOrDefault(U => U.GuId == guid);
            return _context.tblRegisterations.AsNoTracking().Include(A => A.Activity).Include(U => U.User).Include(S => S.StatusReg).Where(A => A.ActivityId == ActivityInfo.Id && (A.StatusRegId == 2));
        }
        public int StudentRegisterHistory(Guid? guid, int AccountId)
        {
            tblActivities ActivityInfo = _context.tblActivities.AsNoTracking().SingleOrDefault(U => U.GuId == guid);
            int RegisterInfoByUserNameCount = _context.tblRegisterations.Where(R => R.UserId == AccountId && R.ActivityId == ActivityInfo.Id).ToList().Count();
            return RegisterInfoByUserNameCount;
        }
        public int StudentRegister(Guid? guid, int Accountid )
        {
            try
            {
                tblActivities ActivityInfo = _context.tblActivities.AsNoTracking().SingleOrDefault(U => U.GuId == guid);
                int RegisterInfoByUserNameCount = _context.tblRegisterations.Where(R => R.UserId == Accountid && R.ActivityId == ActivityInfo.Id).ToList().Count();
                if (RegisterInfoByUserNameCount != 0)
                    return 2;

                tblRegisterations RegisterInfo = new tblRegisterations();
                RegisterInfo.ActivityId = ActivityInfo.Id;
                RegisterInfo.RegisterationTime = DateTime.Now;
                RegisterInfo.StatusRegId = 1;
                RegisterInfo.UserId = Accountid;
                RegisterInfo.GuId = Guid.NewGuid();
                _context.Add(RegisterInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int CheckNumber(Guid? guid)
        {
            tblActivities ActivityInfo = _context.tblActivities.AsNoTracking().SingleOrDefault(U => U.GuId == guid);
            int RegiaterCount = _context.tblRegisterations.Where(U => U.ActivityId == ActivityInfo.Id && (U.StatusRegId == 2 || U.StatusRegId == 1)).Count();
            if ((ActivityInfo.MaxStudents - RegiaterCount) <= 0)
                return 0;
            else
                return ActivityInfo.MaxStudents - RegiaterCount;
        }
        public int StudentNumber(Guid? guid)
        {
            tblActivities ActivityInfo = _context.tblActivities.AsNoTracking().SingleOrDefault(U => U.GuId == guid);
            int RegiaterCount = _context.tblRegisterations.Where(U => U.ActivityId == ActivityInfo.Id && (U.StatusRegId == 2 || U.StatusRegId == 1)).Count();

            return RegiaterCount;
        }
        public string GetCurrentSemester()
        {
            tblSemesters Sem = _context.tblSemesters.SingleOrDefault(S => S.IsActive == true);
            return Sem.SemesterName;
        }
        public int CheckRegState()
        {
            tblUsers UserInfo = _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.Id == 1);
            int check = _context.tblRegisterations.Where(U => U.UserId == UserInfo.Id && (U.StatusRegId == 1 || U.StatusRegId == 2 || U.StatusRegId == 3)).Count();
            if (check == 0)
                return 0;
            else
                return 1;
        }
        public IEnumerable<tblRegisterations> GetAllWaitingActivitiesForInstructor(int AccountId)
        {
            return _context.tblRegisterations.Include(U => U.User).Include(A => A.Activity).Include(SG => SG.StatusReg).
            Where(S => S.StatusRegId == 1 || S.StatusRegId == 2 );
        }
        public IEnumerable<tblRegisterations> GetAllWaitingActivitiesForStudent(int AccountId)
        {

            //return _context.tblRegisterations.Include(U => U.User).Include(A => A.Activity).Include(SG => SG.StatusReg).
            //Where(S => S.StatusRegId == 1 && S.Activity.RegisterStartDate < DateTime.Now && S.Activity.RegisterEndDate > DateTime.Now && S.UserId == AccountId
            //|| S.StatusRegId == 2 && S.Activity.RegisterStartDate < DateTime.Now && S.Activity.RegisterEndDate > DateTime.Now && S.UserId == AccountId);
            return _context.tblRegisterations.Include(U => U.User).Include(A => A.Activity).Include(SG => SG.StatusReg).
           Where(S => S.UserId == AccountId);
        }
        public IEnumerable<tblRegisterations> GetAllStudentRegisterations(int AccountId)
        {

            return _context.tblRegisterations.Include(U => U.User).Include(A => A.Activity).Include(S => S.Activity.Supervisor).Include(SR => SR.StatusReg).Where(A => A.UserId == AccountId);
        }

        public int ActivityApproval(Guid? guid, int statusid)
        {
            try
            {
                tblRegisterations RegInfo = _context.tblRegisterations.AsNoTracking().SingleOrDefault(U => U.GuId == guid);
                RegisterationLogs(RegInfo);
                RegInfo.StatusRegId = statusid;
                _context.Update(RegInfo);
                _context.SaveChanges();
                if (statusid == 2)
                    return 1;
                else
                    return 2;
            }
            catch
            {
                return 0;
            }
        }
        public int ChangeStatus(Guid? guid)
        {
            try
            {
                tblActivities ActivityInfo = _context.tblActivities.AsNoTracking().SingleOrDefault(U => U.GuId == guid);
                if (ActivityInfo.IsOpen == true)
                {
                    ActivityInfo.IsOpen = false;
                }
                else
                {
                    ActivityInfo.IsOpen = true;
                }

                _context.Update(ActivityInfo);
                _context.SaveChanges();
                if (ActivityInfo.IsOpen == true)
                    return 1;
                else
                    return 2;
            }
            catch
            {
                return 0;
            }
        }
        public int CancelRegister(Guid? id, int AccountId)
        {
            try
            {
                tblUsers UserInfo = _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.Id == AccountId);
                tblRegisterations RegInfo = _context.tblRegisterations.AsNoTracking().SingleOrDefault(U => U.GuId == id && UserInfo.Id == AccountId);
                RegisterationLogs(RegInfo);
                RegInfo.StatusRegId = 4;
                _context.Update(RegInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int RemoveStudent(Guid? id)
        {
            try
            {

                tblRegisterations RegInfo = _context.tblRegisterations.AsNoTracking().SingleOrDefault(U => U.GuId == id);
                RegInfo.StatusRegId = 5;
                _context.Update(RegInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int ChangeSt(Guid? id)
        {
            try
            {

                tblActivities ActivityInfo = _context.tblActivities.AsNoTracking().SingleOrDefault(U => U.GuId == id);
                if (ActivityInfo.IsActive == true)
                {
                    ActivityInfo.IsActive = false;
                }
                else
                {
                    ActivityInfo.IsActive = true;
                }
                _context.Update(ActivityInfo);
                _context.SaveChanges();
                if (ActivityInfo.IsActive == true)
                    return 1;
                else
                    return 2;

            }
            catch
            {
                return 0;
            }
        }
        public int AddSemester(tblSemesters SemInfo)
        {
            try
            {
                tblSemesters SemCheck = _context.tblSemesters.SingleOrDefault(S => S.SemesterName == SemInfo.SemesterName);
                if (SemCheck != null)
                    return 2;
                else
                {
                    SemInfo.GuId = Guid.NewGuid();
                    SemInfo.IsActive = false;
                    SemInfo.IsDone = false;
                    _context.Add(SemInfo);
                    _context.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }

        }
 
        public int SemesterActivation(Guid? id,string status)
        {
            try
            {
                tblSemesters SemInfo = _context.tblSemesters.SingleOrDefault(S=>S.GuId == id);
                IList <tblSemesters> SemCheck = GetAllSemesters().ToList();
                foreach(var sem in SemCheck)
                {
                    if (sem.IsActive == true)
                        sem.IsActive = false;
                }
                if (status == "Activate")
                    SemInfo.IsActive = true;
                _context.Update(SemInfo);
                _context.SaveChanges();
                if (SemInfo.IsActive == true)
                    return 1;
                else
                    return 2;
            }
            catch
            {
                return 0;
            }
        }
        public int RegisterationLogs(tblRegisterations RegInfo)
        {
            try { 
                tblRegisterationsLogs LogInfo = new tblRegisterationsLogs();
                LogInfo.RegisterationId = RegInfo.Id;
                LogInfo.UserId = RegInfo.UserId;
                LogInfo.StatusId = RegInfo.StatusRegId;
                LogInfo.UpdateDate = DateTime.Now;
                _context.Add(LogInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int SemesterDelete(Guid? id)
        {
            try
            {
                tblSemesters SemInfo = _context.tblSemesters.SingleOrDefault(S => S.GuId == id);
                SemInfo.IsDone = true;
                _context.Update(SemInfo);
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