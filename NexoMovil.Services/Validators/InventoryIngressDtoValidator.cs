using FluentValidation;
using NexoMovil.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexoMovil.Services.Validators
{
    public class InventoryIngressDtoValidator : AbstractValidator<InventoryIngressDto>
    {
        public InventoryIngressDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("El ID del producto es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
        }
    }
}
