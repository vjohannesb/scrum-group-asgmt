#nullable disable

namespace SharedLibrary.Models
{
    public partial class Tag
    {
        public int TagId { get; set; }
        public int ModelId { get; set; }
        public string TagName { get; set; }
    }
}
