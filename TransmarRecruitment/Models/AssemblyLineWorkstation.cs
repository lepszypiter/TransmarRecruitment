using System.ComponentModel.DataAnnotations;

namespace TransmarRecruitment.Models
{
    public class AssemblyLineWorkstation
    {
        [Key]
        public int Id { get; set; }

        public int AssemblyLineId { get; set; }
        public AssemblyLine? AssemblyLine { get; set; }

        public int WorkstationId { get; set; }
        public Workstation? Workstation { get; set; }

        // Position to preserve ordering in assembly line
        public int Position { get; set; }
    }
}
