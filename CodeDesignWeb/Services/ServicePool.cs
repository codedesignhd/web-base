using CodeDesignDtos.Validators;

namespace CodeDesign.Web.Services
{
    public class ServicePool
    {
        private readonly AppUserService _appUserService;
        private readonly ICDValidatorFactory _validator;
        public ServicePool(AppUserService appUserService, ICDValidatorFactory validator)
        {
            _appUserService = appUserService;
            _validator = validator;
        }

        public ICDValidatorFactory Validator => _validator;
        public AppUserService AppUserService => _appUserService;
    }
}
