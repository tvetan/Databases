namespace VendorLastReport.Client
{
    public class Product
    {
        //      "product-id" : 1,
        //"product-name" : "Beer “Zagorka”",
        //"vendor-name" : "Zagorka Corp.",
        //"total-quantity-sold" : 673,
        //"total-incomes" : 609.24
        public int Id { get; set; }

        public string Name { get; set; }

        public string VendorName { get; set; }

        public int QuantitySold { get; set; }

        public decimal TotalIncome { get; set; }

        public override string ToString()
        {
            return this.Id + "-" + this.Name;
        }
    }
}