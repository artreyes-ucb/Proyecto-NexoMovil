using FluentValidation;
using NexoMovil.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexoMovil.Services.Validators
{
    public class ActualizarProductoDtoValidator : AbstractValidator<ProductDto>
    {
        public ActualizarProductoDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del producto es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("El modelo es obligatorio.")
                .MaximumLength(100).WithMessage("El modelo no debe exceder los 100 caracteres.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("La marca es obligatoria.")
                .MaximumLength(50).WithMessage("La marca no puede exceder los 50 caracteres.");

            RuleFor(x => x.Stock)
              .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
        }
    }
}
