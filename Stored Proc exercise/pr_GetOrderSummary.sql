-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE pr_GetOrderSummary 
	-- Add the parameters for the stored procedure here
	@StartDate datetime2(7),
	@EndDate datetime2(7),
	@EmployeeID int null,
	@CustomerID nchar(5) null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	;WITH GroupedOrders as (SELECT 
			emp.EmployeeID,
			shi.CompanyName as 'ShipperCompanyName',
			cus.CompanyName as 'CustomerCompanyName',
			count(ord.OrderID) as 'NumberOfOders',
			ord.OrderDate,
			sum(ord.Freight) as 'TotalFreightCost',
			count(odd.ProductId) as 'NumberOfDifferentProducts',
			sum((odd.UnitPrice * odd.Quantity) - odd.Discount) as 'TotalOrderValue'
		FROM
			Employees emp
		LEFT JOIN
			Orders ord on ord.EmployeeID = emp.EmployeeID
		LEFT JOIN
			Customers cus on cus.CustomerID = ord.CustomerID
		LEFT JOIN
			Shippers shi on shi.ShipperID = ord.ShipVia
		LEFT JOIN
			[Order Details] odd on odd.OrderID = ord.OrderID
		WHERE
			ord.OrderDate >= @StartDate
			AND ord.OrderDate <= @EndDate
			AND case when @EmployeeID is null then 1 when emp.EmployeeID = @EmployeeID then 1 end = 1
			AND case when @CustomerID is null then 1 when cus.CustomerID = @CustomerID then 1 end = 1
		GROUP BY
			ord.OrderDate,
			emp.EmployeeID,
			cus.CompanyName,
			shi.CompanyName
	)

    -- Insert statements for procedure here
	SELECT 
		emp.TitleOfCourtesy + ' ' + emp.FirstName + ' ' + emp.LastName as 'EmployeeFullName', 
		gos.ShipperCompanyName as 'Shipper company name',
		gos.CustomerCompanyName as 'Customer company name',
		gos.NumberOfOders as 'Number of orders',
		gos.OrderDate as 'Date',
		TotalFreightCost as 'Total freight cost',
		NumberOfDifferentProducts as 'Number of different products',
		TotalOrderValue as 'Total order value'
	FROM 
		GroupedOrders gos
	JOIN 
		Employees emp on emp.EmployeeID = gos.EmployeeID
END
GO
