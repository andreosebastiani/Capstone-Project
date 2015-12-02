USE [DATABASE_X]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spA_Customer]
	@ID int = NULL,
	@Name varchar(100) = NULL,
	@Description varchar(256) = NULL,
	@Page int = NULL,
	@PageSize int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (@Page IS NOT NULL AND @PageSize IS NOT NULL)
	BEGIN
		DECLARE @StartRow int = (@Page - 1) * @PageSize + 1,
				@EndRow int = @Page * @PageSize;

		WITH CustomersFound AS
		(
			SELECT	ROW_NUMBER() OVER (ORDER BY C.Name) RowNumber
			,		C.ID
			,		C.Name
			,		C.DisplayText
			FROM	dbo.Customer AS C
			WHERE	(@ID IS NULL OR C.ID = @ID)
				AND (@Name IS NULL OR C.Name LIKE '%' + @Name + '%')
				AND (@Description IS NULL OR C.[Description] LIKE '%' + @Description + '%')
		)
		SELECT	*
		FROM	CustomersFound
		WHERE	RowNumber BETWEEN @StartRow AND @EndRow
		ORDER BY Name
	END
	ELSE
	BEGIN
		SELECT	C.ID
		,		C.Name
		,		C.DisplayText
		FROM	dbo.Customer AS C
		WHERE	(@ID IS NULL OR C.ID = @ID)
				AND (@Name IS NULL OR C.Name LIKE '%' + @Name + '%')
				AND (@Description IS NULL OR C.[Description] LIKE '%' + @Description + '%')
		ORDER BY C.Name;
	END;

	SELECT	COUNT(*) AS TotalRows
	FROM	dbo.Customer AS C
	WHERE	(@ID IS NULL OR C.ID = @ID)
				AND (@Name IS NULL OR C.Name LIKE '%' + @Name + '%')
				AND (@Description IS NULL OR C.[Description] LIKE '%' + @Description + '%')
END
