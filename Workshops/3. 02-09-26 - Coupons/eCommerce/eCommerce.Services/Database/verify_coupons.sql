-- Run in your coupon database after Update-Database.
SET NOCOUNT ON;
SET XACT_ABORT OFF;

BEGIN TRANSACTION;
BEGIN TRY
    IF (SELECT COUNT(*) FROM dbo.Coupons WHERE Code IN
        (N'SAVE', N'WELCOME', N'EXPIRED', N'FUTURE', N'INACTIVE', N'LIMIT')) <> 6
        THROW 50000, 'Coupon seed data is missing.', 1;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.CouponUsages AS usage
        JOIN dbo.Coupons AS coupon ON coupon.Id = usage.CouponId
        JOIN dbo.Users AS customer ON customer.Id = usage.UserId
        WHERE coupon.Code = N'LIMIT' AND customer.Username = N'customer1'
          AND usage.DiscountAmount = 10)
        THROW 50000, 'Coupon usage seed data is missing.', 1;

    BEGIN TRY
        UPDATE dbo.Coupons SET Code = N'SAVE' WHERE Code = N'WELCOME';
        THROW 50000, 'Duplicate coupon code was accepted.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() NOT IN (2601, 2627) THROW;
    END CATCH;

    BEGIN TRY
        INSERT INTO dbo.CouponUsages (CouponId, UserId, UsedAt, DiscountAmount)
        SELECT CouponId, UserId, UsedAt, DiscountAmount
        FROM dbo.CouponUsages WHERE Id = 1;
        THROW 50000, 'Duplicate coupon usage was accepted.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() NOT IN (2601, 2627) THROW;
    END CATCH;

    BEGIN TRY
        INSERT INTO dbo.CouponUsages (CouponId, UserId, UsedAt, DiscountAmount)
        VALUES (-1, 4, SYSUTCDATETIME(), 10);
        THROW 50000, 'Usage of a nonexistent coupon was accepted.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() <> 547 THROW;
    END CATCH;

    BEGIN TRY
        DELETE FROM dbo.Coupons WHERE Code = N'LIMIT';
        THROW 50000, 'Deletion of a used coupon was accepted.', 1;
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() <> 547 THROW;
    END CATCH;

    ROLLBACK TRANSACTION;
    PRINT 'PASS: coupon seeds, unique codes, one use per user, foreign keys, and restricted deletion.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
