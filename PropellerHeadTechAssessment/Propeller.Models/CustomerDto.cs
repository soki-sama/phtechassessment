using System.Text.Json.Serialization;

namespace Propeller.Models
{
    public class CustomerDto
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("notes")]
        public IEnumerable<NoteDto> Notes { get; set; } = new List<NoteDto>();

        [JsonPropertyName("contacts")]
        public IEnumerable<ContactDto> Contacts { get; set; } = new List<ContactDto>();

    }
}