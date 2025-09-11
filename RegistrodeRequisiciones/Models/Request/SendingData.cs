namespace RegistrodeRequisiciones.Models.Request
{
    public class SendingData
    {
        public long ApplicantId { get; set; }
        public long ResponsibleId { get; set; }
        public string Article { get; set; } = string.Empty;
        public string Observations { get; set; } = string.Empty;

    }
}
