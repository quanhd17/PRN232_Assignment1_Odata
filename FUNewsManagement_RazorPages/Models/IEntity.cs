namespace FUNewsManagement_RazorPages.Models
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
