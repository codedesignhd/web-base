using CodeDesignDtos.Validators;
using CodeDesignWebAPI.Services.Auth;
using CodeDesignWebAPI.Services.Files;

namespace CodeDesignWebAPI.Extensions
{
    public class ServicesPool
    {
        private readonly IAuthService _auth;
        private readonly ICDValidatorFactory _validator;
        private readonly IFileService _file;
        private readonly IWebHostEnvironment _env;
        public ServicesPool(IAuthService auth, ICDValidatorFactory validator, IFileService file, IWebHostEnvironment env)
        {
            _auth = auth;
            _validator = validator;
            _file = file;
            _env = env;
        }

        public ICDValidatorFactory Validator => _validator;
        public IAuthService Auth => _auth;
        public IFileService File => _file;
        public IWebHostEnvironment Enviroment => _env;
    }
}
