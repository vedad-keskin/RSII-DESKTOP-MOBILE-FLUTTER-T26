# Option 1: apply through the existing coupon service

Base branch: `Coupons-Part5` (`6a4dea5`). Endpoint: `POST /Coupon/Apply`.

## Recording and copy sources

1. Copy `eCommerce.Model/Requests/CheckoutRequest.cs` to `CouponApplyRequest.cs`. Keep `Items` and the existing `CheckoutLineRequest`; replace shipping fields with the coupon `Code`. Add the small three-amount `CouponApplyResponse`.
2. In `CouponService`, copy constructor injection of `IAuthenticatedUserAccessor` from `ProductReviewService`. Its registration already exists in `Program.cs`.
3. Build `ApplyAsync` using `ProductReviewService.InsertAsync`: current user, coupon lookup, duplicate-user check, entity initialization, `Add` and `SaveChangesAsync`. Adapt the validity conditions from `CouponResponse.IsCouponValid` and the usage query pattern already in `CouponService`.
4. Copy the product lookup and price-times-quantity calculation from `OrderService.CheckoutAsync`. Copy its transaction pattern, using Serializable isolation to keep usage checks and insertion together. The discount formula and three returned amounts are the coupon-specific additions. This method only creates `CouponUsageIB180079`; it does not create orders or change stock.
5. Add the declaration to `ICouponService`. Copy the custom action in `OrdersController.Checkout` to `CouponController.Apply`, including authorization, and change its types and service call.
6. Copy `OrderProvider.checkout` to `CouponProvider.apply`: change URL, body and return the decoded amount map.
7. In `cart_list_screen.dart`, copy the input/button pattern from `coupon_list_screen.dart`. Copy the cart-item payload from its existing `_checkout`. Reuse `alertBox`, the existing amount formatting, and the cart listener/disposal pattern from `layouts/container_screen.dart` to clear the displayed calculation when items change.

Each successful click consumes one use immediately. Repeating it as the same customer returns a message. Amounts use database product prices and are rounded to two decimal places for the discount. Existing checkout continues with its original behavior.

No database migration, connection-string change or new provider registration is required.

## Checks

```powershell
dotnet build eCommerce.WebAPI/eCommerce.WebAPI.csproj -c Release
dotnet run --project eCommerce.WebAPI -c Release --no-build --launch-profile http
python verify_coupon_application.py http://localhost:5126
```

The Python check uses the existing Windows SQL client through PowerShell, requires the seeded `customer1` and `customer2` accounts (`Test123`), and removes only its own temporary coupons/usages.

From `UI/ecommerce_mobile`:

```powershell
flutter test test/coupon_application_test.dart
```
