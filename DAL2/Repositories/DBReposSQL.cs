using DAL2.Interfaces;
using DAL2;
using DAL2.Interfaces;

using System;
using DAL2.Repositories;

namespace DAL2.Repositories
{
    public class DbReposSQL : IDbRepos
    {
        private Model1 db;
        private ProjectRepositorySQL projectRepository;
        private ContractRepositorySQL contractRepository;
        private EmployeeRepositorySQL employeeRepository;
        private DepartmentRepositorySQL departmentRepository;
        private SpecialtyRepositorySQL specialtyRepository;
        private ParticipationRepositorySQL participationRepository;
        private ReportRepositorySQL reportRepository;

        public DbReposSQL()
        {
            db = new Model1(); // Your actual DbContext name
        }

        public IRepository<project> Projects
        {
            get
            {
                if (projectRepository == null)
                    projectRepository = new ProjectRepositorySQL(db);
                return projectRepository;
            }
        }

        public IRepository<contract> Contracts
        {
            get
            {
                if (contractRepository == null)
                    contractRepository = new ContractRepositorySQL(db);
                return contractRepository;
            }
        }

        public IRepository<employee> Employees
        {
            get
            {
                if (employeeRepository == null)
                    employeeRepository = new EmployeeRepositorySQL(db);
                return employeeRepository;
            }
        }

        public IRepository<department> Departments
        {
            get
            {
                if (departmentRepository == null)
                    departmentRepository = new DepartmentRepositorySQL(db);
                return departmentRepository;
            }
        }

        public IRepository<specialty> Specialties
        {
            get
            {
                if (specialtyRepository == null)
                    specialtyRepository = new SpecialtyRepositorySQL(db);
                return specialtyRepository;
            }
        }

        public IRepository<participation> Participations
        {
            get
            {
                if (participationRepository == null)
                    participationRepository = new ParticipationRepositorySQL(db);
                return participationRepository;
            }
        }

        public IReportsRepository Reports
        {
            get
            {
                if (reportRepository == null)
                    reportRepository = new ReportRepositorySQL(db);
                return reportRepository;
            }
        }

        public int Save()
        {
            return db.SaveChanges();
        }
    }
}