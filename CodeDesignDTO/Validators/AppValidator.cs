using CodeDesign.DTO.Dtos.TaiKhoan;
using CodeDesign.DTO.Dtos.ToDo;
using FluentValidation;

namespace CodeDesign.DTO.Validators
{
    public class AppValidator
    {
        public IValidator<RegisterUserDto> Register => new RegisterValidator();
        public IValidator<UpsertToDoDto> UpdateToDo => new UpsertToDoValidator();
    }
}
