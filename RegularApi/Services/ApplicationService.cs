using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using RegularApi.Dao;
using RegularApi.Domain.Model;

namespace RegularApi.Services
{
    public class ApplicationService
    {
        private readonly IApplicationDao _applicationDao;

        public ApplicationService(IApplicationDao applicationDao)
        {
            _applicationDao = applicationDao;
        }

        public async Task<Either<string, Application>> AddApplicationSetupAsync(Application application)
        {
            var applicationHolder = await _applicationDao.SaveAsync(application);

            if (applicationHolder.IsNone)
            {
                return "Application can't be stored now, please try again";
            }

            return applicationHolder.AsEnumerable().First();
        }

        public async Task<Either<string, IList<Application>>> GetAllApplicationsAsync()
        {
            var applications = await _applicationDao.GetAllAsync();

            return applications.ToList();
        }
    }
}