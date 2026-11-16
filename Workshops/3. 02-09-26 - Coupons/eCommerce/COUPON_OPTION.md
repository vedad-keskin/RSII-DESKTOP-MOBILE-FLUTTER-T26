# Option 2: apply by inserting through coupon-usage CRUD infrastructure

Base branch: `Coupons-Part5` (`6a4dea5`). Endpoint: `POST /CouponUsage`.

## Recording and copy sources

1. Copy the request/response/search/interface/controller/validator structure already used for coupons and product reviews. Rename the classes to `CouponUsage...`. The existing `CouponUsageIB180079` entity and DbSet are reused.
2. Copy `CheckoutRequest` into `CouponUsageInsertRequest`: keep `Items` and `CheckoutLineRequest`, replace shipping fields with `Code`. The update request only needs `Id` because updating historical usage is rejected.
3. Copy `ProductReviewService` to build `CouponUsageService`: keep its authenticated-user injection, per-user read filtering, ownership check, and overridden `InsertAsync`. Adapt the duplicate check and entity initialization to coupon usages. The usual insert/update validator classes satisfy `BaseCRUDService`'s constructor; register them and the new service in `Program.cs` using the existing coupon registrations as the copy source.
4. Inside `InsertAsync`, copy product lookup, price-times-quantity calculation and the transaction pattern from `OrderService.CheckoutAsync`. Use Serializable isolation to keep usage checks and insertion together. Adapt coupon validity conditions from `CouponResponse.IsCouponValid`. The discount formula and returned original/final amounts are the coupon-specific additions.
5. Copy the generic controller declaration from `ProductReviewsController` with the new usage types. The inherited POST calls `InsertAsync`; there is no custom Apply controller action. Read endpoints show only the logged-in user's history. Update/delete overrides reject changes so a usage cannot be erased to redeem the coupon again.
6. Copy `coupon.dart`'s JSON model pattern into `coupon_usage.dart` and generate `coupon_usage.g.dart`. Copy `ProductReviewProvider` into `CouponUsageProvider`; its inherited `insert()` performs the HTTP request. Register it in `main.dart` by copying an existing provider line.
7. In `cart_list_screen.dart`, copy the code input/button pattern from `coupon_list_screen.dart`, the item payload from the existing `_checkout`, and the insert call pattern from `add_review_screen.dart`. Reuse `alertBox`, existing amount formatting, and the cart-listener/disposal pattern from `layouts/container_screen.dart`.

The POST response includes all three amounts and the new usage identity. `OriginalAmount` and `FinalAmount` are nullable in read responses because the existing usage table only stores `DiscountAmount`; this example does not add database columns for historical cart totals. A successful POST always fills all three amounts.

Each successful click consumes one use immediately. Existing checkout continues with its original behavior. No database migration or connection-string change is required.

## Checks

```powershell
dotnet build eCommerce.WebAPI/eCommerce.WebAPI.csproj -c Release
dotnet run --project eCommerce.WebAPI -c Release --no-build --launch-profile http
python verify_coupon_application.py http://localhost:5126
```

The Python check uses the existing Windows SQL client through PowerShell, requires the seeded `customer1` and `customer2` accounts (`Test123`), and removes only its own temporary coupons/usages. It also checks history ownership and rejected history edits/deletes.

From `UI/ecommerce_mobile`:

```powershell
dart run build_runner build --build-filter=lib/models/coupon_usage.g.dart
flutter test test/coupon_application_test.dart
```
