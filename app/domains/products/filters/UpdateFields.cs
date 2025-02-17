namespace app.domains.products.filters
{
    public class UpdateFields
    {
        public string? Name { get; set; }
        public string? Sku { get; set; }
        public decimal? Price { get; set; }

        public void Apply(ref Product product)
        {
            if (!string.IsNullOrEmpty(Name))
                product.Name = Name;
            if (!string.IsNullOrEmpty(Sku))
                product.Sku = Sku;
            if (Price.HasValue)
                product.Price = Price.Value;
        }
    }
}
