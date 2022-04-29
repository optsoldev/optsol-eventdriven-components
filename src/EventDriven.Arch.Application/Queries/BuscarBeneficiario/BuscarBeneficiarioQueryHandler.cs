using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using MediatR;

namespace EventDriven.Arch.Application.Queries.BuscarBeneficiario;

public class BuscarBeneficiarioQueryHandler : IRequestHandler<BuscarBeneficiarioQuery, BeneficiarioQueryResponse>
{
    private readonly IBeneficiarioReadRepository _repository;

    public BuscarBeneficiarioQueryHandler(IBeneficiarioReadRepository repository)
    {
        _repository = repository;
    }
    
    public Task<BeneficiarioQueryResponse> Handle(BuscarBeneficiarioQuery request, CancellationToken cancellationToken)
    {
        BeneficiarioQueryResponse response = new Beneficiario(_repository.GetById(request.Id));

        return Task.FromResult(response);
    }
}