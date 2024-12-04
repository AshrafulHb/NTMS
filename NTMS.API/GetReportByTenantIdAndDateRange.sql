USE [NTMS]
GO
/****** Object:  StoredProcedure [dbo].[GetReportByTenantIdAndDateRange]    Script Date: 26/11/2024 21:06:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetReportByTenantIdAndDateRange]
(
    @TenantId INT,
    @StartDate DATE,
    @EndDate DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare variables
    DECLARE @TenantName NVARCHAR(MAX),
            @FlatCode NVARCHAR(MAX),
            @ElectricMeterNo NVARCHAR(MAX),
            @BillingPeriod NVARCHAR(50),
            @ElectricMeterId INT,
            @FirstReading INT,
            @LastReading INT,
            @ConsumedUnit INT,
            @IsShop BIT,
            @ElectricityCharge DECIMAL(18, 2),
            @DemandCharge DECIMAL(18, 2),
            @ServiceCharge DECIMAL(18, 2),
            @PrincipalAmount DECIMAL(18, 2),
            @ElectricityBill DECIMAL(18, 2),
            @VatPercentage DECIMAL(5, 2),
            @VatAmount DECIMAL(18, 2),
            @HouseRent DECIMAL(18, 2),
            @CleanerBill DECIMAL(18, 2),
            @GasBill DECIMAL(18, 2),
            @TotalBill DECIMAL(18, 2),
			@IsMeterActive BIT;

 SELECT @BillingPeriod = DATENAME(month, @StartDate);
    -- Retrieve tenant and flat details
    SELECT @TenantName = t.Name, @FlatCode = f.Code, @HouseRent = f.Rent
    FROM Tenants t
    JOIN Flats f ON t.Flat_Id = f.Id
    WHERE t.Id = @TenantId AND t.IsActive = 1;

    -- Retrieve electric meter details
    SELECT @ElectricMeterNo = e.MeterNumber, @ElectricMeterId = e.Id, @IsMeterActive = e.IsActive
    FROM EMeters e
    JOIN Flats f ON e.Flat_Id = f.Id
    JOIN Tenants t ON f.Id = t.Flat_Id
    WHERE t.Id = @TenantId AND t.IsActive = 1;

    -- Retrieve meter readings
 --  IF @IsMeterActive = 1
  --  BEGIN
    SELECT @FirstReading = MIN(r.PreviousReading), @LastReading = MAX(r.CurrentReading)
    FROM EReadings r
    WHERE r.EMeter_Id = @ElectricMeterId
      AND r.StartDate >= @StartDate AND r.EndDate <= @EndDate;

    SET @ConsumedUnit = @LastReading - @FirstReading;
--	 END
 --   ELSE
 --   BEGIN
 --       SET @FirstReading = 0;
 --       SET @LastReading = 0;
  --      SET @ConsumedUnit = 0;
 --  END

    -- Determine if the flat is a shop
    SET @IsShop = CASE WHEN CHARINDEX('SHOP', @FlatCode) = 1 THEN 1 ELSE 0 END;

    -- Retrieve rates and calculate electricity charge
    DECLARE @Rate1 DECIMAL(18, 2), @Rate2 DECIMAL(18, 2), @Rate3 DECIMAL(18, 2), @Rate4 DECIMAL(18, 2);
    DECLARE @To1 INT, @To2 INT, @To3 INT;
    DECLARE @RemainingUnits INT = @ConsumedUnit;

    SELECT TOP 1 @Rate1 = Rate1, @Rate2 = Rate2, @Rate3 = Rate3, @Rate4 = Rate4,
                 @To1 = To1, @To2 = To2, @To3 = To3,
                 @DemandCharge = DemandCharge, @ServiceCharge = ServiceCharge, @VatPercentage = Vat
    FROM EBillingRules;

    IF @IsShop = 1
    BEGIN
        SET @ElectricityCharge = @ConsumedUnit * (SELECT TOP 1 MinimumCharge FROM EBillingRules);
    END
    ELSE
    BEGIN
        DECLARE @UnitsInSlab1 INT = 0, @UnitsInSlab2 INT = 0, @UnitsInSlab3 INT = 0, @UnitsInSlab4 INT = 0;

        IF @RemainingUnits > 0 BEGIN SET @UnitsInSlab1 = CASE WHEN @RemainingUnits > @To1 THEN @To1 ELSE @RemainingUnits END; SET @RemainingUnits = @RemainingUnits - @UnitsInSlab1; END
        IF @RemainingUnits > 0 BEGIN SET @UnitsInSlab2 = CASE WHEN @RemainingUnits > (@To2 - @To1) THEN (@To2 - @To1) ELSE @RemainingUnits END; SET @RemainingUnits = @RemainingUnits - @UnitsInSlab2; END
        IF @RemainingUnits > 0 BEGIN SET @UnitsInSlab3 = CASE WHEN @RemainingUnits > (@To3 - @To2) THEN (@To3 - @To2) ELSE @RemainingUnits END; SET @RemainingUnits = @RemainingUnits - @UnitsInSlab3; END
        IF @RemainingUnits > 0 BEGIN SET @UnitsInSlab4 = @RemainingUnits; END

        SET @ElectricityCharge = 
            (@UnitsInSlab1 * @Rate1) +
            (@UnitsInSlab2 * @Rate2) +
            (@UnitsInSlab3 * @Rate3) +
            (@UnitsInSlab4 * @Rate4);
    END     

	-- Set charges to zero if meter is inactive
    IF @ElectricMeterNo IS Null OR @IsMeterActive =0
    BEGIN
        SET @ElectricityCharge = 0;
        SET @DemandCharge = 0;
       SET @ServiceCharge = 0;
        SET @PrincipalAmount = 0;
        SET @VatAmount = 0;
        SET @ElectricityBill = 0;
    END
    ELSE
	BEGIN
  
      -- Calculate totals
    SET @PrincipalAmount = @ElectricityCharge + @DemandCharge + @ServiceCharge;
    SET @VatAmount = @PrincipalAmount * @VatPercentage / 100;
    SET @ElectricityBill = @PrincipalAmount + @VatAmount;
	END


	  -- Retrieve utility costs
 IF @IsShop = 1
    BEGIN
        SET @GasBill = 0;
		SET @CleanerBill = 0;
    END
	ELSE
    BEGIN
    SELECT @CleanerBill = ISNULL((SELECT Cost FROM UtilityOptions WHERE [Name] = 'Cleaner'), 0);
    SELECT @GasBill = ISNULL((SELECT Cost FROM UtilityOptions WHERE [Name] = 'Gas'), 0);
 END
 SET @TotalBill =ROUND( @ElectricityBill + @GasBill + @CleanerBill + @HouseRent,0);


    SELECT
        ISNULL(@TenantName, '') AS TenantName,
        ISNULL(@FlatCode, '') AS FlatCode,
        ISNULL(@ElectricMeterNo, '') AS ElectricMeterNo,
        CONVERT(VARCHAR, @StartDate, 103) AS BillStartDate,
        CONVERT(VARCHAR, @EndDate, 103) AS BillEndDate,
        CONVERT(VARCHAR(10), @BillingPeriod) AS BillingPeriod,
        CONVERT(VARCHAR(10), @LastReading) AS ElectricMeterCurrentReading,
        CONVERT(VARCHAR(10), @FirstReading) AS ElectricMeterPreviousReading,
        CONVERT(VARCHAR(10), @ConsumedUnit) AS ConsumedElectricUnit,
        FORMAT(@ElectricityCharge, 'N') AS ElectricityCharge,
        FORMAT(@DemandCharge, 'N') AS DemandCharge,
        FORMAT(@ServiceCharge, 'N') AS MeterRent,
        FORMAT(@PrincipalAmount, 'N') AS PrincipalAmount,
        FORMAT(@VatAmount, 'N') AS Vat,
        FORMAT(@ElectricityBill, 'N') AS ElectricityBill,
        FORMAT(@HouseRent, 'N') AS HouseRent,
        FORMAT(@GasBill, 'N') AS GasBill,
        FORMAT(@CleanerBill, 'N') AS CleanerBill,
        FORMAT(@TotalBill, 'N') AS Total;
END;