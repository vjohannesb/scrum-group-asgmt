#nullable disable

namespace SharedLibrary.Models
{
    public partial class ModelTag
    {
        public int ModelId { get; set; }
        public int TagId { get; set; }

        public virtual ProductModel Model { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
