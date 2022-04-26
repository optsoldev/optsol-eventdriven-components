using EventDriven.Arch.Domain;
using EventDriven.Arch.Domain.Beneficiarios;
using MediatR;

namespace EventDriven.Arch.Application.Commands.CriarBeneficiarios;

public class CriarBeneficiarioCommandHandler : IRequestHandler<CriarBeneficiarioCommand, Unit>
{
    private readonly IBeneficiarioWriteRepository _beneficiarioRepository;

    public CriarBeneficiarioCommandHandler(IBeneficiarioWriteRepository beneficiarioRepository)
    {
        _beneficiarioRepository = beneficiarioRepository;
    }
    
    public Task<Unit> Handle(CriarBeneficiarioCommand request, CancellationToken cancellationToken)
    {
        var beneficiario = new Beneficiario(request.PrimeiroNome, request.SegundoNome);

        if (beneficiario.IsInvalid)
        {
            Publish(beneficiario.Errors);
        }
        else
        {
            _beneficiarioRepository.Commit(beneficiario);
        }
        
        return Task.FromResult(new Unit());
    }

    private void Publish(IList<IEvent> beneficiarioErrors)
    {
        throw new NotImplementedException();
    }
}

public interface IMessageBus
{
    Task Publish(IEnumerable<IEvent> events);
}