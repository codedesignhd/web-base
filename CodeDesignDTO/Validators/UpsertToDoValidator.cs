using CodeDesign.Utilities;
using FluentValidation;
namespace CodeDesign.Dtos
{
    public class UpsertToDoValidator : AbstractValidator<UpsertToDoDto>
    {
        public UpsertToDoValidator()
        {
            RuleFor(x => x.title)
                .NotNull().WithMessage("Tên công việc không được để trống");
            RuleFor(x => x.ngay_ket_thuc)
                  .Custom((ngay_ket_thuc, context) =>
                  {
                      if (!DateTimeUtils.IsValidDate(ngay_ket_thuc))
                      {
                          context.AddFailure("Ngày kết thúc không đúng định dạng");
                      }
                  });
        }
    }
}
