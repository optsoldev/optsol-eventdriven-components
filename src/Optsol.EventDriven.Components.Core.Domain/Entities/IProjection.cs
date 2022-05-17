
namespace Optsol.EventDriven.Components.Core.Domain.Entities
{
    public interface IProjection
    {
        public Guid Id { get; }
        public DateTime CreatedDate { get; }

    }
}
