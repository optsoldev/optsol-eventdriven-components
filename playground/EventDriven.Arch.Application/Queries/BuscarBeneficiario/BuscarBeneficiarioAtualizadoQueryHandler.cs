using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using EventDriven.Arch.Domain.Beneficiarios.Repositories;
using MediatR;

namespace EventDriven.Arch.Application.Queries.BuscarBeneficiario;

public class BuscarBeneficiarioAtualizadoQueryHandler : IRequestHandler<BuscarBeneficiarioAtualizadoQuery, BeneficiarioQueryResponse>
{
    private readonly IBeneficiarioAtualizadoReadProjectionRepository _repository;

    public BuscarBeneficiarioAtualizadoQueryHandler(IBeneficiarioAtualizadoReadProjectionRepository repository)
    {
        _repository = repository;
    }
    
    public Task<BeneficiarioQueryResponse> Handle(BuscarBeneficiarioAtualizadoQuery request, CancellationToken cancellationToken)
    {
        BeneficiarioQueryResponse response = _repository.GetById(request.Id);

        return Task.FromResult(response);
    }
}