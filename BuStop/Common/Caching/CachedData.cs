// --- WebApi/Common/Caching/CachedData.cs ---
namespace WebApi.Common.Caching
{
    // Usamos <T> para que pueda guardar cualquier tipo de lista o DTO
    public record CachedData<T>
    {
        public T Data { get; init; } = default!;
        public string ETag { get; init; } = string.Empty;
    }
}