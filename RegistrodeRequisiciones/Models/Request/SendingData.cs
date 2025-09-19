namespace RegistrodeRequisiciones.Models.Request
{
    public class SendingData
    {
        public long ApplicantId { get; set; }
        public long ResponsibleId { get; set; }

        public string Article { get; set; } = string.Empty;
        public string Observations { get; set; } = string.Empty;

        public string SerialNumber { get; set; } = string.Empty;
        public string TypeOfEquipment { get; set; } = string.Empty;
        public string brand { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
    }
}
