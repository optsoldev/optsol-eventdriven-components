using EventDriven.Arch.Domain.Beneficiarios;
using Optsol.EventDriven.Components.Core.Application;

namespace EventDriven.Arch.Application.Queries.BuscarBeneficiario;

public record BuscarBeneficiarioQuery(Guid Id) : IQuery<BeneficiarioQueryResponse>;

public record BeneficiarioQueryResponse(string?PrimeiroNome, string? SegundoNome) : IQueryResponse
{
    public static implicit operator BeneficiarioQueryResponse(Beneficiario entity)
    {
        return new BeneficiarioQueryResponse(entity.PrimeiroNome, entity.SegundoNome);
    }
}



