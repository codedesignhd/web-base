using CodeDesign.DTO.Validators;

namespace CodeDesign.Web.Services
{
    public class DependencyContainer
    {
        private readonly AppUserService _appUserService;
        private readonly AppValidator _validator;
        public DependencyContainer(AppUserService appUserService, AppValidator validator)
        {
            _appUserService = appUserService;
            _validator = validator;
        }

        public AppValidator Validator => _validator;
        public AppUserService AppUserService => _appUserService;
    }
}
