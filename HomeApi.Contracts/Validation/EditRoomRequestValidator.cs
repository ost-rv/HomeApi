using FluentValidation;
using HomeApi.Contracts.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Contracts.Validation
{
    /// <summary>
    /// Класс-валидатор запросов обновления комнаты
    /// </summary>
    public class EditRoomRequestValidator : AbstractValidator<EditRoomRequest>
    {
        /// <summary>
        /// Метод-конструктор, устанавливающий правила
        /// </summary>
        public EditRoomRequestValidator()
        {
            RuleFor(x => x.NewArea).InclusiveBetween(10,100).When(r => r.NewArea.HasValue);
        }
    }
}
