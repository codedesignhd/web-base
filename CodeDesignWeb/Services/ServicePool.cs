using CodeDesignDtos.Validators;

namespace CodeDesign.Web.Services
{
    public class ServicePool
    {
        private readonly AppUserService _appUserService;
        private readonly IValidatationFactory _validator;
        public ServicePool(AppUserService appUserService, IValidatationFactory validator)
        {
            _appUserService = appUserService;
            _validator = validator;
        }

        public IValidatationFactory Validator => _validator;
        public AppUserService AppUserService => _appUserService;
    }
}
