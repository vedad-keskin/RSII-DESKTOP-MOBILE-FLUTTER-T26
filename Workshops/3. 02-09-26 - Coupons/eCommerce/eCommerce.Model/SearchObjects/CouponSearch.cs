namespace eCommerce.Model.SearchObjects
{
    public class CouponSearch : BaseSearchObject
    {
        public string? Code { get; set; }
        public bool? IsActive { get; set; }
    }
}
