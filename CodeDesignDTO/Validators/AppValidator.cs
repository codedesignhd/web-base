using FluentValidation;

namespace CodeDesign.Dtos
{
    public class AppValidator
    {
        public IValidator<RegisterUserDto> Register => new RegisterValidator();
        public IValidator<UpsertToDoDto> UpdateToDo => new UpsertToDoValidator();
    }
}
