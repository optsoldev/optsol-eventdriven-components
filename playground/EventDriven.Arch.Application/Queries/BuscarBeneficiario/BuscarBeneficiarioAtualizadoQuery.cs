using EventDriven.Arch.Domain.Beneficiarios.Projections;
using Optsol.EventDriven.Components.Core.Application;

namespace EventDriven.Arch.Application.Queries.BuscarBeneficiario;

public record BuscarBeneficiarioAtualizadoQuery(Guid Id) : IQuery<BeneficiarioQueryResponse>;

public record BeneficiarioQueryResponse(string?PrimeiroNome, string? SegundoNome)
{
    public static implicit operator BeneficiarioQueryResponse(BeneficiarioAtualizado entity)
    {
        return new BeneficiarioQueryResponse(entity.PrimeiroNome, entity.SegundoNome);
    }
}



