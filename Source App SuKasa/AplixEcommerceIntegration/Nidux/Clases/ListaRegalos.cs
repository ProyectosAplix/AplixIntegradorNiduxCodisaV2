using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    public partial class ListaRegalos
    {
        public Giftlist Giftlist { get; set; }
    }

    public partial class Giftlist
    {
        public int wish_id { get; set; }
        public int OwnerId { get; set; }
        public string RandomCode { get; set; }
        public string ListName { get; set; }
        public EcoFriendly Shipping { get; set; }
        public string eventDate { get; set; }
        public int modeList { get; set; }
        public EcoFriendly ListType { get; set; }
        public EcoFriendly EcoFriendly { get; set; }
        public EcoFriendly EnableExtraProductsBuy { get; set; }
        public EcoFriendly EnableAnonymousBuyer { get; set; }
        public int ApprovedShipping { get; set; }
        public int UserDirection { get; set; }
        public string ThanksMsg { get; set; }
        public string PublicMsg { get; set; }
        public EcoFriendly Status { get; set; }
        public int extraOwnerId { get; set; }
        public Products Products { get; set; }
    }

    public partial class EcoFriendly
    {
        public int id { get; set; }
        public string Details { get; set; }
    }

    public partial class Products
    {
        public System.Collections.Generic.Dictionary<int, Available> Available { get; set; }
        //public Missing Missing { get; set; }
    }

    public partial class Available
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int product_code { get; set; }
        public string ProductPrice { get; set; }
        public int ProductSale { get; set; }
        public int ProductTax { get; set; }
        public int ProductType { get; set; }
        public int WishedAmount { get; set; }
        public int ProductVariationId { get; set; }
        public int Given { get; set; }
        public int BuyersDetails { get; set; }
        public int AddBy { get; set; }
        public int Favorite { get; set; }
        public int Stock { get; set; }
        public int Status { get; set; }
        public string ProductUrl { get; set; }
        public int SelectableAmount { get; set; }
        public string CalculatedTotal { get; set; }
        public double FinalPrice { get; set; }
        public double OldPrice { get; set; }
        public int? VariationId { get; set; }
        public double? PriceWhenWished { get; set; }
        public string WishDetailsJson { get; set; }
        public int? Indexes { get; set; }
        public string VariationSubstring { get; set; }
    }
}
