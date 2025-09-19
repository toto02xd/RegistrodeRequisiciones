namespace RegistrodeRequisiciones.Models.Entities
{
    public class Loan
    {
        public long Id { get; set; }
        public long ApplicantId { get; set; }
        public long ResponsibleId { get; set; }
        public string Article { get; set; } = string.Empty;
        public string Observations { get; set; } = string.Empty;
        public string TypeOfEquipment { get; set; } = string.Empty;
        public string brand { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        public virtual User? Applicant { get; set; }
        public virtual User? Responsible {  get; set; }
    }
}
