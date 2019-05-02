using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using RegularApi.Dao;
using RegularApi.Domain.Model;

namespace RegularApi.Services
{
    public class ApplicationSetupService
    {
        private readonly ILogger<ApplicationSetupService> _logger;
        private readonly IApplicationDao _applicationDao;

        public ApplicationSetupService(ILoggerFactory loggerFactory, IApplicationDao applicationDao)
        {
            _logger = loggerFactory.CreateLogger<ApplicationSetupService>();
            _applicationDao = applicationDao;
        }

        public async Task<Either<string, Application>> SaveApplicationSetupAsync(Application application)
        {
            var applicationHolder = await _applicationDao.SaveApplicationSetup(application);

            if (applicationHolder.IsNone)
            {
                return "Application setup can't be stored now, please try again";
            }

            var value = applicationHolder.AsEnumerable().First();

            return value;
        }
    }
}