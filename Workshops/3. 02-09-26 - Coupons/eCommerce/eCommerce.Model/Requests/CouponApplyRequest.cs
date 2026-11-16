using System.ComponentModel.DataAnnotations;

namespace eCommerce.Model.Requests;

public class CouponApplyRequest
{
    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<CheckoutLineRequest> Items { get; set; } = new();
}
