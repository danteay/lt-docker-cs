namespace app.controllers.dtos.products
{
    public struct ProductItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
    }
}
