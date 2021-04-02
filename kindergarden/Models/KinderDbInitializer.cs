using System;
using System.Data.Entity;

namespace kindergarden.Models
{
    public class KinderDbInitializer : CreateDatabaseIfNotExists<KinderModelContext>
    {
        protected override void Seed(KinderModelContext context)
        {
            int schoolId = insertFirstSchool(context);
            int masterId = insertFirstMaster(context, schoolId);
            int adminId = insertFirstAdmin(context, schoolId);
            int teacherId = insertFirstTeacher(context, schoolId);
            int parentId = insertFirstParent(context, schoolId);
            insertMasterAddress(context, masterId);
            insertAdminAddress(context, adminId);
            insertTeacherAddress(context, teacherId);
           // insertParentAddress(context, parentId);
            base.Seed(context);
        }

        private void insertParentAddress(KinderModelContext context, int parentId)
        {
            Address address = new Address();
            address.Number = "007";
            address.PersonId = parentId;
            address.PostCode = "007";
            address.State = "007";
            address.Street = "007";
            address.City = "007";
          //  context.Addresses.Add(address);
            context.SaveChanges();
        }

        private int insertFirstParent(KinderModelContext context, int schoolId)
        {
            Person first = new Person();
            first.CreatedDate = DateTime.Now;
           // first.Gsm = "123";
            first.IsActive = true;
            first.IsParent = true;
            first.LastName = "parent";
            first.Name = "parent";
            first.Pass = "123";
            first.Email = "parent@kita365.de";
            first.SchoolId = schoolId;
            context.person.Add(first);
            context.SaveChanges();
            int parentId = first.Id;
            return parentId;
        }

        private void insertTeacherAddress(KinderModelContext context, int teacherId)
        {
            Address address = new Address();
            address.Number = "007";
            address.PersonId = teacherId;
            address.PostCode = "007";
            address.State = "007";
            address.Street = "007";
            address.City = "007";
          //  context.Addresses.Add(address);
            context.SaveChanges();
        }

        private int insertFirstTeacher(KinderModelContext context, int schoolId)
        {
            Person first = new Person();
            first.CreatedDate = DateTime.Now;
           // first.Gsm = "123";
            first.IsActive = true;
            first.IsTeacher = true;
            first.LastName = "teacher";
            first.Name = "teacher";
            first.Pass = "123";
            first.Email = "teacher@kita365.de";
            first.SchoolId = schoolId;
            context.person.Add(first);
            context.SaveChanges();
            int teacherId = first.Id;
            return teacherId;
        }

        private void insertAdminAddress(KinderModelContext context, int adminId)
        {
            Address address = new Address();
            address.Number = "007";
            address.PersonId = adminId;
            address.PostCode = "007";
            address.State = "007";
            address.Street = "007";
            address.City = "007";
          //  context.Addresses.Add(address);
            context.SaveChanges();
        }

        private int insertFirstAdmin(KinderModelContext context, int schoolId)
        {
            Person first = new Person();
            first.CreatedDate = DateTime.Now;
           // first.Gsm = "123";
            first.IsActive = true;
            first.IsAdmin = true;
            first.IsMaster = false;
            first.LastName = "admin";
            first.Name = "admin";
            first.Pass = "123";
            first.Email = "admin@kita365.de";
            first.SchoolId = schoolId;
            context.person.Add(first);
            context.SaveChanges();
            int adminId = first.Id;
            return adminId;
        }

        private void insertMasterAddress(KinderModelContext context, int masterId)
        {
            Address address = new Address();
            address.Number = "007";
            address.PersonId = masterId;
            address.PostCode = "007";
            address.State = "007";
            address.Street = "007";
            address.City = "007";
           // context.Addresses.Add(address);
            context.SaveChanges();
        }

        private int insertFirstMaster(KinderModelContext context, int schoolId)
        {
            Person first = new Person();
            first.CreatedDate = DateTime.Now;
          //  first.Gsm = "123";
            first.IsActive = true;
            first.IsAdmin = false;
            first.IsMaster = true;
            first.LastName = "durmus";
            first.Name = "mehmet";
            first.Pass = "123";
            first.Email = "master@kita365.de";
            first.SchoolId = schoolId;
            context.person.Add(first);
            context.SaveChanges();
            int masterId = first.Id;
            return masterId;


        }

        private int insertFirstSchool(KinderModelContext context)
        {
            School school = new School();
            school.City = "Istanbul";
            school.CreatedDate = DateTime.Now;
            school.Email = "asd@asd.com";
            school.IsActive = false;
            school.Name = "System";
            school.Number = "123";
            school.Phone = "123";
            school.PostalCode = "34000";
            school.State = "Besiktas";
            school.Street = "HafizMustafa";
            school.schoolGuid = Guid.NewGuid().ToString();
            context.School.Add(school);
            context.SaveChanges();
            int SchoolId = school.SchoolId;
            return SchoolId;
        }
    }
}