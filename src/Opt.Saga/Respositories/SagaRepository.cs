namespace Opt.Saga.Core.Respositories
{
    public enum SagaStatus
    {
        Success,
        Failure,
        Executing
    }
    public class SagaActivity
    {
        public SagaActivity(Guid
            transactionId, string Stepname, string Flowname, SagaStatus SagaStatus, DateTime startedAt)
        {
            this.Id = transactionId;
            this.Stepname = Stepname;
            this.Flowname = Flowname;
            this.SagaStatus = SagaStatus;
            this.StartedAt = startedAt;
        }
        public SagaActivity()
        {

        }

        public Guid Id { get; set; }
        public string Stepname { get; set; }
        public string Flowname { get; set; }
        public SagaStatus SagaStatus { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
    }
    public class MongoSettings { }


}
