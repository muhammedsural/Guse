using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guse;

public class BeamValidator : AbstractValidator<Beam>
{
    public BeamValidator()
    {
        RuleFor(beam => beam.D)
            .NotEqual(0)
            .NotEmpty()
            .WithMessage("Lütfen d (efektif kesit yüksekliği) değerini girin!");
            

        RuleFor(beam => beam.H)
            .NotEmpty()
            .WithMessage("Lütfen H (Kesit yüksekliği) değerini girin!");

    }
    
}
