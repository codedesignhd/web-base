using CodeDesign.Dtos.Validators;
using CodeDesign.WebAPI.Services;

namespace CodeDesign.WebAPI.ServiceExtensions
{
    public class DependencyContainer
    {
        private readonly IAuthService _auth;
        private readonly ICodeDesignValidatorFactory _validator;
        private readonly IFileService _file;
        private readonly IWebHostEnvironment _env;
        public DependencyContainer(IAuthService auth, ICodeDesignValidatorFactory validator, IFileService file, IWebHostEnvironment env)
        {
            _auth = auth;
            _validator = validator;
            _file = file;
            _env = env;
        }

        public ICodeDesignValidatorFactory Validator => _validator;
        public IAuthService Auth => _auth;
        public IFileService File => _file;
        public IWebHostEnvironment Enviroment => _env;
    }
}
