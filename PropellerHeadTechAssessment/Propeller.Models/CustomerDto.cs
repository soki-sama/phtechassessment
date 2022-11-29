namespace Propeller.Models
{
    public class CustomerDto
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public IEnumerable<NoteDto> Notes { get; set; } = new List<NoteDto>();
    }
}