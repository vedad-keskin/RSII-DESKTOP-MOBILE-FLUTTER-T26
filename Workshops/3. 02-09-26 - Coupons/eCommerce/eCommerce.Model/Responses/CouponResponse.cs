namespace eCommerce.Model.Responses
{
    public class CouponResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int MaxUses { get; set; }
        public bool IsActive { get; set; }
        public int UsageCount { get; set; }

        public bool IsCurrentlyValid => IsActive && ValidFrom <= DateTime.UtcNow && ValidTo >= DateTime.UtcNow && UsageCount < MaxUses;
        //{
        //    get
        //    {
        //        var now = DateTime.UtcNow;
        //        return IsActive && ValidFrom <= now && now <= ValidTo && UsageCount < MaxUses;
        //    }
        //}
    }
}
