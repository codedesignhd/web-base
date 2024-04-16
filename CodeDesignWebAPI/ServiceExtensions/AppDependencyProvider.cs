using CodeDesign.Dtos.Validators;
using CodeDesign.WebAPI.Services;

namespace CodeDesign.WebAPI.ServiceExtensions
{
    public class AppDependencyProvider
    {
        private readonly IAuthService _auth;
        private readonly AppValidator _validator;
        public AppDependencyProvider(IAuthService auth, AppValidator validator)
        {
            _auth = auth;
            _validator = validator;
        }

        public AppValidator Validator => _validator;
        public IAuthService Auth => _auth;
    }
}
