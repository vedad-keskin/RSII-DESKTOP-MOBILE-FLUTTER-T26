namespace eCommerce.Model.Responses;

public class CouponUsageResponse
{
    public int Id { get; set; }
    public int CouponId { get; set; }
    public int UserId { get; set; }
    public DateTime UsedAt { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal? OriginalAmount { get; set; }
    public decimal? FinalAmount { get; set; }
}
