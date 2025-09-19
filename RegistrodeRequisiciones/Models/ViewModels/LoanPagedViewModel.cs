using System.Collections.Generic;
using RegistrodeRequisiciones.Models.Entities;

namespace RegistrodeRequisiciones.Models.ViewModels
{
    public class LoanPagedViewModel
    {
        public List<Loan> Loans { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchUser { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}