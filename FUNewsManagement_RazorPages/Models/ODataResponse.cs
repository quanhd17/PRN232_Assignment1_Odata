namespace FUNewsManagement_RazorPages.Models
{
    public class ODataResponse<T>
    {
        public List<T> Value { get; set; } = new();
    }

}
