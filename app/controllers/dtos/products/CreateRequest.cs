namespace app.controllers.dtos.products
{
    public struct CreateRequest
    {
        public required string Name { get; set; }
        public string? Sku { get; set; }
        public required decimal Price { get; set; }
    }
}
