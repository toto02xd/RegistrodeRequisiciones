namespace RegistrodeRequisiciones.Models.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public virtual ICollection<Loan> ApplicantLoans { get; set; } = new List<Loan>();
        public virtual ICollection<Loan> ResponsibleLoans { get; set; } = new List<Loan>();
    }
}
