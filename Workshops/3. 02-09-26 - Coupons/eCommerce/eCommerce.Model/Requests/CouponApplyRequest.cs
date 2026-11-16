using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests;

public class CouponApplyRequest
{
    public List<CheckoutLineRequest> Items { get; set; } = new();

    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;
}
