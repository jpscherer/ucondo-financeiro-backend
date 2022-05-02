using FluentValidation;
using Ucondo_Financeiro_Api.Contratos;
using Ucondo_Financeiro_Dominio;

namespace Ucondo_Financeiro_Api.Validators
{
    public class ContaRateioValidator : AbstractValidator<ContaRateioContrato>
    {
        public ContaRateioValidator()
        {
            RuleFor(contaRateio => contaRateio.Nome).NotNull().NotEmpty().Length(1, 50)
                .WithMessage(MensagensDeUsuario.CAMPO_NOME_OBRIGATORIO);

            RuleFor(contaRateio => contaRateio.Codigo).NotNull().GreaterThan(0).LessThan(1000)
                .WithMessage(MensagensDeUsuario.CAMPO_CODIGO_FORA_DO_INTERVALO_PERMITIDO);
        }
    }
}
