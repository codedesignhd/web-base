using CodeDesign.Dtos;

namespace CodeDesign.WebAPI.Services
{
    public class AppDependencyProvider
    {
        private readonly AppUserProvider _appUserService;
        private readonly AppValidator _validator;
        public AppDependencyProvider(AppUserProvider appUserService, AppValidator validator)
        {
            _appUserService = appUserService;
            _validator = validator;
        }

        public AppValidator Validator => _validator;
        public AppUserProvider AppUserService => _appUserService;
    }
}
