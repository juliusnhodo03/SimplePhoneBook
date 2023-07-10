using Infrastructure.Repository.Database;

namespace Infrastructure.Repository.Migrations
{
	public static class DbHelpers
	{
		#region CleanUp

		public const string CleanUp =
        (@"
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Hyphen_BatchReport]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_Hyphen_BatchReport]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_VaultPartialPayment_Header]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_VaultPartialPayment_Header]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DepositSlip_Header]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_DepositSlip_Header]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_VaultDeposit_Processing]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_VaultDeposit_Processing]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TransactionSummary]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_TransactionSummary]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udf_UserRoleIds]'))
                DROP FUNCTION [dbo].[udf_UserRoleIds]		

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DepositSlip_Details]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_DepositSlip_Details]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CashOrderedAndPackedDetails]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_CashOrderedAndPackedDetails]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CashSubmittedAndVerifiedDetails]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_CashSubmittedAndVerifiedDetails]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CashOrderSlip_Header]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_CashOrderSlip_Header]
				
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CashOrderSlip_Details]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_CashOrderSlip_Details]				

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RptCashDepositAuditTrailDetails]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[sp_RptCashDepositAuditTrailDetails]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DepositSlip_Subreport]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_DepositSlip_Subreport]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DropSlip_Subreport]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_DropSlip_Subreport]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Deposits_with_containerTotal]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_Deposits_with_containerTotal]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CashProcessingSlip_Header]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_CashProcessingSlip_Header]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_VarianceDropSlip_Subreport]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_VarianceDropSlip_Subreport]

            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_VaultReport]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[usp_VaultReport]
				
            IF EXISTS (SELECT * FROM sys.indexes WHERE name='UI_CashDepositTransactionReferenceNumber' AND object_id = OBJECT_ID('[dbo].[CashDeposit]'))
                ALTER TABLE [dbo].[CashDeposit] DROP CONSTRAINT [UI_CashDepositTransactionReferenceNumber]

            IF EXISTS (SELECT * FROM sys.indexes WHERE name='UI_CashOrderransactionReferenceNumber' AND object_id = OBJECT_ID('[dbo].[CashOrder]'))
                ALTER TABLE [dbo].[CashOrder] DROP CONSTRAINT [UI_CashOrderransactionReferenceNumber] 
			
			IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_RoleReport_webpages_Roles')
			BEGIN
				ALTER TABLE [dbo].[RoleReport]  WITH CHECK ADD  CONSTRAINT [FK_RoleReport_webpages_Roles] FOREIGN KEY([RoleId])
				REFERENCES [dbo].[webpages_Roles] ([RoleId])

				ALTER TABLE [dbo].[RoleReport] CHECK CONSTRAINT [FK_RoleReport_webpages_Roles]
			END
        ");

		#endregion		
        
        #region VaultPaymentDepositSlip_Header

        public const string VaultPartialPaymentHeader =
        (@"    
                CREATE PROCEDURE [dbo].[usp_VaultPartialPayment_Header]
                        @VaultPartialPaymentId INT = 0,
						@IsPayment VARCHAR(8) = 'true'
                AS
                BEGIN
                        -- SET NOCOUNT ON added to prevent extra result sets from
                        -- interfering with SELECT statements.
                        SET NOCOUNT ON;

						IF(@IsPayment = 'true')
						BEGIN
							SELECT	P.VaultPartialPaymentId,
									A.AccountHolderName,
									B.Name BankName,
									AccountNumber = STUFF (A.AccountNumber , 1 , 7 , '*****' ) ,
									P.TotalToBePaid,
									[Date] = CONVERT(VARCHAR(10), CONVERT (datetime, P.TransactionDate, 104), 105),
									[Time] = CONVERT(VARCHAR(8), CONVERT (datetime, P.TransactionDate, 104), 108),
									P.PaymentReference										                                 
							FROM dbo.VaultPartialPayment P INNER JOIN dbo.Account A
									ON P.BeneficiaryCode = A.BeneficiaryCode INNER JOIN dbo.Bank B 
									ON B.BankId = A.BankId
							WHERE VaultPartialPaymentId = @VaultPartialPaymentId
							ORDER BY P.VaultPartialPaymentId
						END
						ELSE
						BEGIN
							SELECT	D.CashDepositId,
									A.AccountHolderName,
									B.Name BankName,
									AccountNumber = STUFF (A.AccountNumber , 1 , 7 , '*****' ) ,
									TotalToBePaid = D.VaultAmount,
									[Date] = CONVERT(VARCHAR(10), CONVERT (datetime, D.CitDateTime, 104), 105),
									[Time] = CONVERT(VARCHAR(8), CONVERT (datetime, D.CitDateTime, 104), 108),
									PaymentReference = S.DepositReference										                                 
							FROM dbo.CashDeposit D INNER JOIN dbo.Account A
									ON D.AccountId = A.AccountId INNER JOIN dbo.Bank B 
									ON B.BankId = A.BankId INNER JOIN SITE S
									ON S.SiteId = D.SiteId
							WHERE CashDepositId = @VaultPartialPaymentId
							ORDER BY D.CashDepositId						
						END
                END");

		#endregion

		#region DepositSlip_Header

		public const string DepositSlipHeader =
		(@"    
            CREATE PROCEDURE [dbo].[usp_DepositSlip_Header]
	                        @CashDepositId INT = 0
                        AS
                        BEGIN
	                        -- SET NOCOUNT ON added to prevent extra result sets from
	                        -- interfering with SELECT statements.
	                        SET NOCOUNT ON;

	                        SELECT C.ContainerId,
		                           D.Name DepositTypeName, 
		                           Cash.TransactionReference AS DepositReference, 
		                           Cash.Narrative AS DepositDescription, 
		                           Cash.CreateDate AS DepositStartDate, 
		                           S.Name AS SiteName, 
								   S.CitCode,
		                           CapturedByFullName = U.FirstName + ' ' + U.LastName,
		                           IsOriginalPrinted = 0,
		                           SerialNumber = C.SerialNumber,
								   C.IsPrimaryContainer,
								   NumberOfDrops = (SELECT COUNT(ConDrop.ContainerDropId) 
														FROM Container Con INNER JOIN ContainerDrop ConDrop ON Con.ContainerId = ConDrop.ContainerId AND ConDrop.IsNotDeleted = 1
															WHERE Con.ContainerId = C.ContainerId)
	                        FROM CashDeposit AS Cash INNER JOIN
		                         DepositType AS D ON Cash.DepositTypeId = D.DepositTypeId INNER JOIN
		                         Container C ON C.CashDepositId = Cash.CashDepositId AND C.IsNotDeleted = 1 INNER JOIN
		                         Site AS S ON Cash.SiteId = S.SiteId INNER JOIN
		                         Merchant AS M ON S.MerchantId = M.MerchantId INNER JOIN
		                         [User] AS U ON Cash.CreatedById = U.UserId
	                        WHERE Cash.CashDepositId = @CashDepositId AND Cash.IsNotDeleted = 1
                            ORDER BY C.ContainerId
                        END");

		#endregion

		#region TransactionSummary

		public const string TransactionSummary =
		(@"             
            CREATE PROCEDURE [dbo].[usp_TransactionSummary]
	                        @CashDepositId INT = 0
                        AS
                        BEGIN
	                        -- SET NOCOUNT ON added to prevent extra result sets from
	                        -- interfering with SELECT statements.
	                        SET NOCOUNT ON;

							SELECT 
								   CN.ContainerId,
								   DepositType.Name AS DepositTypeName,
								   D.ReferenceNumber AS DepositReference,
								   D.Narrative AS DepositDescription,
								   C.SubmitDateTime AS DepositDate, 
								   S.Name AS SiteName, 
								   S.CitCode,
								   CapturedByFullName = U.FirstName + ' ' + U.LastName,
								   IsOriginalPrinted = 0,
								   SerialNumber = (CASE DepositType.LookUpKey 
														WHEN 'MULTI_DROP' THEN D.BagSerialNumber
														ELSE  CN.SerialNumber
												   END),
								   CN.IsPrimaryContainer,
								   NumberOfDrops = (SELECT COUNT(ConDrop.ContainerDropId) 
														FROM Container Con INNER JOIN ContainerDrop ConDrop ON Con.ContainerId = ConDrop.ContainerId AND ConDrop.IsNotDeleted = 1
															WHERE Con.ContainerId = CN.ContainerId),
								   D.Amount
							FROM  CashDeposit C INNER JOIN
								  Container CN ON C.CashDepositId = CN.CashDepositId AND CN.IsNotDeleted = 1 INNER JOIN
								  ContainerDrop D ON CN.ContainerId = D.ContainerId AND D.IsNotDeleted = 1 INNER JOIN
								  DepositType ON C.DepositTypeId = DepositType.DepositTypeId INNER JOIN
								  [Site] S ON C.SiteId = S.SiteId INNER JOIN
								  [User] U ON C.CreatedById = U.UserId
							WHERE C.CashDepositId = @CashDepositId AND C.IsNotDeleted = 1
							ORDER BY CN.ContainerId
          END");

		#endregion

		#region UserRoleIds

		public const string UserRoleIds =
		(@" 
			CREATE FUNCTION [dbo].[udf_UserRoleIds] 
			(		
				@UserID int	
			)
			RETURNS TABLE 
			AS
			RETURN 
			(
				SELECT [RoleId]
				  FROM [dbo].[webpages_UsersInRoles]
				  WHERE [UserId] = @UserID
			)
		");

		#endregion

		#region Deposits_with_containerTotal

		public const string DepositsWithContainerTotal =
		(@"    
            CREATE PROCEDURE [dbo].[usp_Deposits_with_containerTotal]
	                        @CashDepositId INT = 0
                        AS
                        BEGIN
	                        -- SET NOCOUNT ON added to prevent extra result sets from
	                        -- interfering with SELECT statements.
	                        SET NOCOUNT ON;
							
							DECLARE @PrimarySerialNumber VARCHAR(20) = (SELECT SerialNumber FROM Container 
																		WHERE CashDepositId = @CashDepositId AND IsPrimaryContainer = 'True')

																	
	
							DECLARE @Result_Data TABLE 
							(
								ContainerId INT,  
								DepositTypeName VARCHAR(100),
								DepositReference VARCHAR(100),
								DepositDescription VARCHAR(100),
								DepositStartDate SMALLDATETIME,
								SiteName VARCHAR(100), 
								CitCode VARCHAR(100), 
								CapturedByFullName VARCHAR(100), 
								IsOriginalPrinted BIT, 
								SerialNumber VARCHAR(100),
								IsPrimaryContainer BIT,
								PrimarySerialNumber VARCHAR(100),
								DepositDate SMALLDATETIME,
								ContainerTotal FLOAT
							)



							INSERT INTO @Result_Data
								SELECT C.ContainerId,
									   D.Name AS DepositTypeName, 
									   Cash.TransactionReference AS DepositReference, 
									   Cash.Narrative AS DepositDescription, 
									   Cash.CreateDate AS DepositStartDate, 
									   S.Name AS SiteName, 
									   S.CitCode,
									   U.FirstName + ' ' + U.LastName AS CapturedByFullName,
									   IsOriginalPrinted = 0,
									   C.SerialNumber,
									   C.IsPrimaryContainer,
									   @PrimarySerialNumber AS IsPrimaryContainer,
									   Cash.SubmitDateTime AS DepositDate,
									   C.Amount AS ContainerTotal
								FROM CashDeposit AS Cash INNER JOIN
									 DepositType AS D ON Cash.DepositTypeId = D.DepositTypeId INNER JOIN
									 Container C ON C.CashDepositId = Cash.CashDepositId INNER JOIN
									 Site AS S ON Cash.SiteId = S.SiteId INNER JOIN
									 Merchant AS M ON S.MerchantId = M.MerchantId INNER JOIN
									 [User] AS U ON Cash.CreatedById = U.UserId
								WHERE Cash.CashDepositId = @CashDepositId AND C.Amount > 0
								ORDER BY C.ContainerId	
								
								
								SELECT * FROM @Result_Data	
                        END");

		#endregion

		#region DepositSlip_Details

		public const string DepositSlipDetails =
		(@"
            CREATE PROCEDURE [dbo].[usp_DepositSlip_Details]
	                    @CashDepositId INT = 0
                    AS
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
	
		                    DECLARE @MinContainerId INT = (SELECT MIN(ContainerId) FROM Container WHERE CashDepositId = @CashDepositId)
		                    DECLARE @MaxContainerId INT = (SELECT MAX(ContainerId) FROM Container WHERE CashDepositId = @CashDepositId)	
		                    DECLARE @TotalContainerAmount INT = 0		


		                    DECLARE @Denominations TABLE 
		                    (
			                    DenominationId INT,  
								DenominationName VARCHAR(20), 
			                    ValueInCents INT, 
								DenominationTypeName VARCHAR(20), 
			                    DenominationCount INT, 
								DenominationValue FLOAT,
			                    ContainerId INT,
								TotalContainerAmount FLOAT
		                    )

		                    DECLARE @Report_Data TABLE 
		                    (
			                    DenominationId INT,  
								DenominationName VARCHAR(20), 
			                    ValueInCents INT, 
								DenominationTypeName VARCHAR(20), 
			                    DenominationCount INT, 
								DenominationValue FLOAT,
			                    ContainerId INT,
								TotalContainerAmount FLOAT
		                    )


		                    /* LOOP Containers*/ 
		                    WHILE (@MinContainerId <= @MaxContainerId)
		                    BEGIN
		                            --ENUMERATE ALL DENOMINATION
				                    INSERT INTO @Denominations
					                    SELECT DISTINCT D.DenominationId, D.Name, D.ValueInCents, T.Name, 0, 0, NULL, 0
					                    FROM Denomination D INNER JOIN DenominationType T ON D.DenominationTypeId = T.DenominationTypeId

				                    --UPDATE CONTAINER DROP	
				                    UPDATE D
					                    SET D.DenominationCount = Data.Count,
					                        D.DenominationValue = Data.Value
				                    FROM 
				                    (		
					                    SELECT C.ContainerId, 
						                        I.DenominationId,  
						                        SUM(I.[Count]) AS [Count], 
						                        SUM(I.Value) [Value]
					                    FROM Container C INNER JOIN
						                        ContainerDrop D ON C.ContainerId = D.ContainerId INNER JOIN
						                        ContainerDropItem I ON D.ContainerDropId = I.ContainerDropId
					                    WHERE C.ContainerId = @MinContainerId
					                    GROUP BY C.ContainerId,
						                        I.DenominationId
				                    ) Data INNER JOIN @Denominations D ON D.DenominationId = Data.DenominationId

									SET @TotalContainerAmount = (SELECT DISTINCT ISNULL(Amount, 0) FROM Container WHERE ContainerId = @MinContainerId)

				                    UPDATE D
					                   SET D.ContainerId = @MinContainerId,
									       D.TotalContainerAmount = @TotalContainerAmount										   
				                    FROM @Denominations D

				
				                    INSERT INTO @Report_Data SELECT * FROM @Denominations
				
				                    DELETE FROM @Denominations

			                    /* INCREMENT ContainerId BY 1*/ 
			                    SET @MinContainerId = (SELECT MIN(ContainerId) FROM Container WHERE ContainerId > @MinContainerId)
		                    END


		                    SELECT * FROM @Report_Data
								WHERE TotalContainerAmount > 0
								GROUP BY ContainerId,DenominationId
		                            ,DenominationName
			                        ,ValueInCents
			                        ,DenominationTypeName
			                        ,DenominationCount
			                        ,DenominationValue
									,TotalContainerAmount
                    END");

		#endregion

		#region DepositSlip_Subreport

		public const string DepositSlipSubreport =
		(@"
            CREATE PROCEDURE [dbo].[usp_DepositSlip_Subreport]
	                    @ContainerId INT = 0
                    AS
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;

		                    DECLARE @Denominations TABLE 
		                    (
			                    DenominationId INT,  
								DenominationName VARCHAR(20), 
			                    ValueInCents INT, 
								DenominationTypeName VARCHAR(20), 
			                    DenominationCount INT, 
								DenominationValue FLOAT,
			                    ContainerId INT,
								IsPrimaryContainer BIT
		                    )

		                    DECLARE @Report_Data TABLE 
		                    (
			                    DenominationId INT,  
								DenominationName VARCHAR(20), 
			                    ValueInCents INT, 
								DenominationTypeName VARCHAR(20), 
			                    DenominationCount INT, 
								DenominationValue FLOAT,
			                    ContainerId INT,
								IsPrimaryContainer BIT
		                    )

							
		                    DECLARE @IsPrimaryContainer INT =  (SELECT DISTINCT IsPrimaryContainer FROM Container WHERE ContainerId = @ContainerId)

		                    --ENUMERATE ALL DENOMINATION
				            INSERT INTO @Denominations
					            SELECT DISTINCT D.DenominationId, D.Name, D.ValueInCents, T.Name, 0, 0, @ContainerId, @IsPrimaryContainer
					            FROM Denomination D INNER JOIN DenominationType T ON D.DenominationTypeId = T.DenominationTypeId

				            --UPDATE CONTAINER DROP	
				            UPDATE D
					            SET D.DenominationCount = Data.Count,
					                D.DenominationValue = Data.Value
				            FROM 
				            (		
					            SELECT C.ContainerId, 
						                I.DenominationId,  
						                SUM(I.[Count]) AS [Count], 
						                SUM(I.Value) [Value]
					            FROM Container C INNER JOIN
						                ContainerDrop D ON C.ContainerId = D.ContainerId INNER JOIN
						                ContainerDropItem I ON D.ContainerDropId = I.ContainerDropId
					            WHERE C.ContainerId = @ContainerId
					            GROUP BY C.ContainerId,
						                I.DenominationId
				            ) Data INNER JOIN @Denominations D ON D.DenominationId = Data.DenominationId
				
				            INSERT INTO @Report_Data SELECT * FROM @Denominations
				

							SELECT *	FROM @Report_Data
							GROUP BY ContainerId, DenominationId, DenominationName, ValueInCents, DenominationTypeName, DenominationCount, DenominationValue, IsPrimaryContainer
							ORDER BY ContainerId
                    END");

		#endregion

		#region DropSlip_Subreport

		public const string DropSlipSubreport =
			(@"
                CREATE PROCEDURE [dbo].[usp_DropSlip_Subreport]
	                    @ContainerDropId INT = 0
                    AS
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						
		                    DECLARE @Denominations TABLE 
		                    (
			                    DenominationId INT,  
								DenominationName VARCHAR(20), 
			                    ValueInCents INT, 
								DenominationTypeName VARCHAR(20), 
			                    DenominationCount INT, 
								DenominationValue FLOAT, 
								DropBagSerialNumber VARCHAR(20)
		                    )							

		                    --ENUMERATE ALL DENOMINATION
				            INSERT INTO @Denominations
					            SELECT DISTINCT D.DenominationId, D.Name, D.ValueInCents, T.Name, 0, 0, NULL
					            FROM Denomination D INNER JOIN DenominationType T ON D.DenominationTypeId = T.DenominationTypeId

				            --UPDATE CONTAINER DROP	
				            UPDATE D
					            SET D.DenominationCount = Data.[Count],
					                D.DenominationValue = Data.[Value],
									D.DropBagSerialNumber = Data.[DropBagSerialNumber]
				            FROM 
				            (		
					            SELECT C.ContainerId, 
						                I.DenominationId,  
						                I.[Count], 
						                I.Value,
										C.SerialNumber AS DropBagSerialNumber
					            FROM Container C INNER JOIN
						                ContainerDrop D ON C.ContainerId = D.ContainerId INNER JOIN
						                ContainerDropItem I ON D.ContainerDropId = I.ContainerDropId AND I.IsNotDeleted = 1
					            WHERE D.ContainerDropId = @ContainerDropId AND D.IsNotDeleted = 1

				            ) Data INNER JOIN @Denominations D ON D.DenominationId = Data.DenominationId
				
							SELECT *	FROM @Denominations
							GROUP BY DenominationId, DenominationName, ValueInCents, DenominationTypeName, DenominationCount, DenominationValue, DropBagSerialNumber
							ORDER BY ValueInCents DESC
                    END
            ");

		#endregion

		#region Create Transaction Numbers Unique Constraints

		public const string DbCashDepositConstraints =
			(@" ALTER TABLE dbo.CashDeposit ADD CONSTRAINT
	            UI_CashDepositTransactionReferenceNumber UNIQUE NONCLUSTERED 
	            (
	            TransactionReference
	            ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ");

		public const string DbCashOrderConstraints =
			(@" ALTER TABLE dbo.CashOrder ADD CONSTRAINT
	            UI_CashOrderransactionReferenceNumber UNIQUE NONCLUSTERED 
	            (
	            ReferenceNumber
	            ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ");

		#endregion

		#region CashPRocessingSlip_Header

		public const string CashProcessingSlipHeader =
		(@"
            CREATE PROCEDURE [dbo].[usp_CashProcessingSlip_Header]
	                        @CashDepositId INT = 0
                        AS
                        BEGIN
	                        -- SET NOCOUNT ON added to prevent extra result sets from
	                        -- interfering with SELECT statements.
	                        SET NOCOUNT ON;

	                        SELECT C.ContainerId,
		                           D.Name AS DepositTypeName, 
		                           Cash.TransactionReference AS DepositReference, 
		                           Cash.Narrative AS DepositDescription, 
		                           Cash.ProcessedDateTime AS VerifiedDate, 
		                           S.Name AS SiteName, 
								   S.CitCode,

								   ProcessedByFullName = (SELECT TOP 1 FirstName + ' ' + LastName FROM [User] WHERE [User].UserId = Cash.ProcessedById),
								   VarianceConfirmedByFullnames = (SELECT TOP 1 FirstName + ' ' + LastName FROM [User] WHERE [User].UserId = Cash.SupervisorId),
		                           NumberOfCopiesPrinted = 1,
		                           SerialNumber = C.SerialNumber,
								   C.IsPrimaryContainer,

								   NumberOfDrops = (SELECT COUNT(ConDrop.ContainerDropId) 
														FROM Container Con INNER JOIN ContainerDrop ConDrop ON Con.ContainerId = ConDrop.ContainerId
															WHERE Con.ContainerId = C.ContainerId)

	                        FROM CashDeposit AS Cash INNER JOIN
		                         DepositType AS D ON Cash.DepositTypeId = D.DepositTypeId INNER JOIN
		                         Container C ON C.CashDepositId = Cash.CashDepositId INNER JOIN
		                         Site AS S ON Cash.SiteId = S.SiteId INNER JOIN
		                         Merchant AS M ON S.MerchantId = M.MerchantId
	                        WHERE Cash.CashDepositId = @CashDepositId
                            ORDER BY C.ContainerId
                        END
        ");

		#endregion

		#region VarianceDropSlip_Subreport

		public const string VarienceDropSlipSubreport =
			(@"       
            CREATE PROCEDURE [dbo].[usp_VarianceDropSlip_Subreport]
	                    @ContainerDropId INT = 0
                    AS
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						
		                    DECLARE @Denominations TABLE 
		                    (
			                    DenominationId INT,  
								DenominationName VARCHAR(20), 
			                    ValueInCents INT, 
								DenominationTypeName VARCHAR(20), 
			                    DenominationCount INT, 
								DenominationValue FLOAT, 
								DropBagSerialNumber VARCHAR(20),
																
			                    CashValueVerified FLOAT, 
			                    DenominationCountVerified INT, 								 
			                    CashValueVariance FLOAT, 
			                    CountVariance INT
		                    )
							
							DECLARE @CashDepositId AS INT
							DECLARE @DeviceId AS INT							
							
							 SELECT TOP 1 @DeviceId = CD.DeviceId
										, @CashDepositId = CD.CashDepositId 
							FROM  CashDeposit CD INNER JOIN Container C ON CD.CashDepositId = C.CashDepositId INNER JOIN ContainerDrop D ON D.ContainerId = C.ContainerId
							WHERE D.ContainerDropId = @ContainerDropId

							
							--ENUMERATE ALL DENOMINATION
							INSERT INTO @Denominations
								SELECT DISTINCT D.DenominationId, D.Name, D.ValueInCents, T.Name, 0, 0, NULL, 0, 0, 0, 0
								FROM Denomination D INNER JOIN DenominationType T ON D.DenominationTypeId = T.DenominationTypeId
					

							--UPDATE CONTAINER DROP	
							UPDATE D
								SET D.DenominationCount = Data.[Count],
									D.DenominationValue = Data.[Value],
									D.DropBagSerialNumber = Data.[DropBagSerialNumber],

									D.CashValueVerified = (SELECT CASE Data.[HasDiscrepancy] WHEN 0 THEN Data.[Value] ELSE Data.[DiscrepancyValue] END),
									D.[DenominationCountVerified] = (SELECT CASE Data.[HasDiscrepancy] WHEN 0 THEN Data.[Count] ELSE Data.[DiscrepancyCount] END)	
							FROM 
							(		
							   SELECT C.ContainerId, 
									I.DenominationId,  
									I.[Count], 
									I.Value,
									C.SerialNumber AS DropBagSerialNumber,
									ISNULL(I.DiscrepancyCount, 0) DiscrepancyCount,
									ISNULL(I.DiscrepancyValue, 0) DiscrepancyValue,
									D.HasDiscrepancy									
							   FROM Container C LEFT OUTER JOIN
									ContainerDrop D ON C.ContainerId = D.ContainerId AND D.IsNotDeleted=1 LEFT OUTER JOIN
									ContainerDropItem I ON D.ContainerDropId = I.ContainerDropId AND I.IsNotDeleted=1
								WHERE D.ContainerDropId = @ContainerDropId AND C.IsNotDeleted=1

							) Data INNER JOIN @Denominations D ON D.DenominationId = Data.DenominationId

									
									
							UPDATE @Denominations
							SET CashValueVariance = (SELECT CASE CashValueVerified - DenominationValue
																WHEN 0 THEN 0 
																ELSE CashValueVerified - DenominationValue 
															END),
							CountVariance = (SELECT CASE DenominationCountVerified - DenominationCount
																WHEN 0 THEN 0 
																ELSE ABS(DenominationCountVerified - DenominationCount)
															END)	


							-- DENOMINATIONS
							SELECT *	FROM @Denominations
							ORDER BY ValueInCents DESC
                    END
		");

		#endregion

		#region CashOrdering_Header

		public const string CashOrderSlipHeader =
			(@"    
            CREATE PROCEDURE [dbo].[usp_CashOrderSlip_Header]
	                        @CashOrderId INT = 0
                        AS
                        BEGIN
	                        -- SET NOCOUNT ON added to prevent extra result sets from
	                        -- interfering with SELECT statements.
	                        SET NOCOUNT ON;
							SELECT        
									Site.Name AS SiteName, 
									Site.CitCode, 
									CashOrder.OrderDate AS OrderEndDate, 
									CashOrder.ReferenceNumber AS TransactionReferenceNumber, 
									CashOrder.ContainerNumberWithCashForExchange, 
									CashOrder.EmptyContainerOrBagNumber, 
									CashOrder.DeliveryDate, 
									CashOrderType.Name AS CashOrderTypeName, 
									UserName = [User].FirstName + ' ' + [User].LastName
							FROM CashOrder INNER JOIN
								 CashOrderContainer ON CashOrder.CashOrderContainerId = CashOrderContainer.CashOrderContainerId INNER JOIN
								 Site ON CashOrder.SiteId = Site.SiteId INNER JOIN
								 Merchant ON Site.MerchantId = Merchant.MerchantId INNER JOIN
								 CashOrderType ON CashOrder.CashOrderTypeId = CashOrderType.CashOrderTypeId LEFT OUTER JOIN
								 [User] ON CashOrder.CreatedById = [User].UserId

	                        WHERE CashOrder.CashOrderId = @CashOrderId
                        END");

		#endregion

		#region CashOrdering_Details

		public const string CashOrderingDetails =
			(@" 
                CREATE PROCEDURE [dbo].[usp_CashOrderSlip_Details]
	                        @CashOrderId INT = 0
                        AS
                        BEGIN
	                        -- SET NOCOUNT ON added to prevent extra result sets from
	                        -- interfering with SELECT statements.
	                        SET NOCOUNT ON;
						
		                    DECLARE @Denominations TABLE 
		                    ( 
									DenominationId INT,
									CashOrderId INT, 
									DenominationName VARCHAR(20),  
									DenominationTypeName VARCHAR(20),
									ValueInCents INT,  
									IsCashRequiredInExchange BIT, 
									IsCashForwardedForExchange BIT,  
									CashDenomination INT, 
									ForwardedCount INT, 
									ForwardedValue FLOAT,
									RequiredCount INT, 
									RequiredValue FLOAT
		                    )

							
				            INSERT INTO @Denominations
					            SELECT DISTINCT D.DenominationId, 0, D.Name, T.Name, D.ValueInCents, 0, 0, ValueInCents/100, 0, 0, 0,0
					            FROM Denomination D INNER JOIN DenominationType T ON D.DenominationTypeId = T.DenominationTypeId


								
				            --UPDATE CONTAINER DROP	
				            UPDATE D
					            SET D.CashOrderId = C.CashOrderId,
					                D.IsCashRequiredInExchange = C.IsCashRequiredInExchange,
									D.IsCashForwardedForExchange = C.IsCashForwardedForExchange,
									D.CashDenomination = C.CashDenomination,
									
									D.ForwardedCount = (SELECT CASE C.IsCashForwardedForExchange WHEN 0 THEN 0 ELSE C.[Count] END),
									D.RequiredCount = (SELECT CASE C.IsCashRequiredInExchange WHEN 0 THEN 0 ELSE C.[Count] END),
									
									D.ForwardedValue = (SELECT CASE C.IsCashForwardedForExchange WHEN 0 THEN 0 ELSE C.[Value] END),
									D.RequiredValue = (SELECT CASE C.IsCashRequiredInExchange WHEN 0 THEN 0 ELSE C.[Value] END)						 		
				            FROM 
				            (		
									SELECT  I.DenominationId,    
											O.CashOrderId, 
											D.IsCashRequiredInExchange, 
											D.IsCashForwardedForExchange, 
											I.CashOrderContainerDropId, 
											I.ValueInCents/100 AS CashDenomination, 
											I.Count, 
											I.Value
									FROM CashOrderContainerDropItem AS I INNER JOIN
										 CashOrderContainerDrop AS D ON I.CashOrderContainerDropId = D.CashOrderContainerDropId INNER JOIN
										 CashOrderContainer AS C ON D.CashOrderContainerId = C.CashOrderContainerId INNER JOIN
										 CashOrder AS O ON C.CashOrderContainerId = O.CashOrderContainerId INNER JOIN Denomination M
										 ON I.DenominationId = M.DenominationId INNER JOIN DenominationType T ON T.DenominationTypeId = M.DenominationTypeId
									WHERE O.CashOrderId = @CashOrderId

				            ) C INNER JOIN @Denominations D ON C.DenominationId = D.DenominationId
				
				         
						    SELECT * from @Denominations
                        END
            ");

		#endregion

		#region CashOrderedAndPackedDetails

		public const string CashOrderedAndPackedDetails =
			(@"      
            CREATE PROCEDURE [dbo].[usp_CashOrderedAndPackedDetails]
	                    @ContainerDropId INT = 0
                    AS
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						
		                    DECLARE @Denominations TABLE 
		                    (
			                    DenominationId INT,  
								DenominationName VARCHAR(20), 
			                    ValueInCents INT, 
								DenominationTypeName VARCHAR(20), 
																
			                    CashValueSubmitted FLOAT, 	
			                    CashCountSubmitted INT,
																
			                    CashValueVerified FLOAT, 	
			                    CashCountVerified INT,
																
			                    CashValueOrdered FLOAT, 	
			                    CashCountOrdered INT,
																
			                    CashValuePacked FLOAT, 	
			                    CashCountPacked INT,
								IsCashForwardedForExchange BIT 
		                    )
							

		                    --ENUMERATE ALL DENOMINATION
				            INSERT INTO @Denominations
					            SELECT DISTINCT D.DenominationId, D.Name, D.ValueInCents, T.Name, 0, 0, 0, 0, 0, 0, 0, 0, 0
					            FROM Denomination D INNER JOIN DenominationType T ON D.DenominationTypeId = T.DenominationTypeId

							

				            --UPDATE CONTAINER DROP	
				            UPDATE D
					            SET D.CashCountOrdered = Data.CashCountOrdered,
					                D.CashValueOrdered = Data.CashValueOrdered,

					                D.CashCountPacked = Data.PackedCount,
					                D.CashValuePacked = Data.PackedValue							 		
				            FROM 
				            (		
					            SELECT C.CashOrderContainerId, 
						                I.DenominationId,  
						                I.[Count] CashCountOrdered, 
						                I.Value CashValueOrdered,
										I.PackedCount,
										I.PackedValue,
										I.VerifiedValue,
										I.VerifiedCount,
										d.IsCashForwardedForExchange									
					            FROM CashOrderContainer C INNER JOIN
						                CashOrderContainerDrop D ON C.CashOrderContainerId = D.CashOrderContainerId INNER JOIN
						                CashOrderContainerDropItem I ON D.CashOrderContainerDropId = I.CashOrderContainerDropId
					            WHERE D.CashOrderContainerDropId = @ContainerDropId AND D.IsCashRequiredInExchange = 1

				            ) Data INNER JOIN @Denominations D ON D.DenominationId = Data.DenominationId				

							SELECT *	FROM @Denominations
							ORDER BY ValueInCents DESC
                    END");

		#endregion

		#region CashSubmittedAndVerifiedDetails

		public const string CashSubmittedAndVerifiedDetails =
			(@"
                CREATE PROCEDURE [dbo].[usp_CashSubmittedAndVerifiedDetails]
	                    @ContainerDropId INT = 0
                    AS
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						
		                    DECLARE @Denominations TABLE 
		                    (
			                    DenominationId INT,  
								DenominationName VARCHAR(20), 
			                    ValueInCents INT, 
								DenominationTypeName VARCHAR(20), 
																
			                    CashValueSubmitted FLOAT, 	
			                    CashCountSubmitted INT,
																
			                    CashValueVerified FLOAT, 	
			                    CashCountVerified INT,
																
			                    CashValueOrdered FLOAT, 	
			                    CashCountOrdered INT,
																
			                    CashValuePacked FLOAT, 	
			                    CashCountPacked INT,
								IsCashForwardedForExchange BIT 
		                    )
							

		                    --ENUMERATE ALL DENOMINATION
				            INSERT INTO @Denominations
					            SELECT DISTINCT D.DenominationId, D.Name, D.ValueInCents, T.Name, 0, 0, 0, 0, 0, 0, 0, 0, 0
					            FROM Denomination D INNER JOIN DenominationType T ON D.DenominationTypeId = T.DenominationTypeId

							

				            --UPDATE CONTAINER DROP	
				            UPDATE D
					            SET D.CashCountSubmitted = Data.CashCountSubmitted,
					                D.CashValueSubmitted = Data.CashValueSubmitted,

					                D.CashCountVerified = Data.VerifiedCount,
					                D.CashValueVerified = Data.VerifiedValue

					                --D.CashCountOrdered = Data.[Count],
					                --D.CashValueOrdered = Data.[Value],

					                --D.CashCountPacked = Data.PackedCount,
					                --D.CashValuePacked = Data.PackedValue							 		
				            FROM 
				            (		
					            SELECT C.CashOrderContainerId, 
						                I.DenominationId,  
						                I.[Count] CashCountSubmitted, 
						                I.Value CashValueSubmitted,
										I.PackedCount,
										I.PackedValue,
										I.VerifiedValue,
										I.VerifiedCount,
										d.IsCashForwardedForExchange									
					            FROM CashOrderContainer C INNER JOIN
						                CashOrderContainerDrop D ON C.CashOrderContainerId = D.CashOrderContainerId INNER JOIN
						                CashOrderContainerDropItem I ON D.CashOrderContainerDropId = I.CashOrderContainerDropId
					            WHERE D.CashOrderContainerDropId = @ContainerDropId AND D.IsCashForwardedForExchange = 1

				            ) Data INNER JOIN @Denominations D ON D.DenominationId = Data.DenominationId
				

							SELECT *	FROM @Denominations
							ORDER BY ValueInCents DESC
                    END 
            ");

		#endregion

		#region CashDepositAuditTrailDetails

		public const string CashDepositAuditTrailDetails =
			(@"
                CREATE PROCEDURE [dbo].[sp_RptCashDepositAuditTrailDetails] 
					  @UserID AS INT 
					, @FromDate AS DATE 
					, @ToDate AS DATE 
					, @Merchant AS VARCHAR(MAX) 
					, @Site AS VARCHAR(MAX) 
					, @CITCode AS VARCHAR(MAX) 
					, @CashCentre AS VARCHAR(MAX) 
					, @ContainerNo AS VARCHAR(MAX) 
					, @ProductType AS VARCHAR(MAX)
					, @TransactionType AS VARCHAR(MAX)
					, @Amount AS DECIMAL

					AS

					BEGIN
						-- SET NOCOUNT ON added to prevent extra result sets from
						-- interfering with SELECT statements.
						SET NOCOUNT ON;

						WITH CashDepositDrop AS (
							SELECT    ConD.ContainerDropId
									, Cond.ContainerId

									, DR.[Name] DiscrepancyReason
									, S.[Name] DropStatus
									, ConD.Narrative
									, ConD.ReferenceNumber
									, ConD.BagSerialNumber
									, ConD.Amount
									, ConD.DiscrepancyAmount
									, ConD.ActualAmount
									, ConD.Comment
									, ConD.Number
									, EC.[Name] Error
									, ConD.SettlementDateTime
									, ConD.SendDateTime
									, UC.FirstName + ' ' + UC.LastName ConDCreateUserName	
							------------------------------------------------------------------------
									, Den.[Description] DenominationTypeDescription
									, Den.ValueInCents
									, DenT.[Name] DenominationType
									, Den.DenominationId
							FROM ContainerDrop ConD LEFT JOIN DiscrepancyReason DR 
														ON ConD.DiscrepancyReasonId = DR.DiscrepancyReasonId
													LEFT JOIN [Status] S 
														ON ConD.StatusId = S.StatusId
													LEFT JOIN ErrorCode EC 
														ON ConD.ErrorCodeId = EC.ErrorCodeId
													LEFT JOIN [User] UC
														ON ConD.CreatedById = UC.UserId
													, Denomination Den LEFT JOIN DenominationType DenT ON Den.DenominationTypeId = DenT.DenominationTypeId
							WHERE ConD.IsNotDeleted = 1
						)

						, CashDepositDetail AS (
							SELECT    CDD.ContainerDropId
									, CDD.ContainerId
									, CDD.DiscrepancyReason
									, CDD.DropStatus
									, CDD.Narrative
									, CDD.ReferenceNumber
									, CDD.BagSerialNumber
									, CDD.Amount
									, CDD.DiscrepancyAmount
									, CDD.ActualAmount
									, CDD.Comment
									, CDD.Number
									, CDD.Error
									, CDD.SettlementDateTime
									, CDD.SendDateTime
									, CDD.ConDCreateUserName	
							------------------------------------------------------------------------
									, CDD.DenominationTypeDescription
									, CDD.ValueInCents
									, CDD.DenominationType
									, CDD.DenominationId
									, CDI.[Count] DepositCount
									, CDI.Value DepositValue
									, CDI.DiscrepancyCount
									, CDI.DiscrepancyValue
									, CDI.ActualCount
									, CDI.ActualValue
									, CreatedByCDI.FirstName + ' ' + CreatedByCDI.LastName CDICreateUserName
							FROM CashDepositDrop CDD LEFT JOIN ContainerDropItem CDI
															ON CDD.ContainerDropId = CDI.ContainerDropId
															AND CDD.DenominationId = CDI.DenominationId
															AND CDI.IsNotDeleted = 1
													 LEFT JOIN [User] CreatedByCDI ON CDI.CreatedById = CreatedByCDI.UserId
						)


						SELECT	  CD.CashDepositId
								, CD.ProductTypeId
								, DT.[Name] DepositType
								, CD.CreateDate DepositStart
								, CD.SubmitDateTime DepositEnd
								, M.[Name] Merchant
								, S.[Name] [Site]
								, CD.SiteId
								, City.[Name] City
								, S.CitCode CitCode
								, B.[Name] SettlementBank
								, A.AccountNumber	SettlementAccNo
								, CASE WHEN CD.DeviceId IS NULL THEN S.CitCode ELSE D.SerialNumber END DeviceSerialNo
								, PT.[Name] Product 
								, CD.Narrative DepositNarrative
								, UniqueTransactionReference = CASE DT.[LookUpKey] WHEN 'MULTI_DEPOSIT' THEN  CDDT.ReferenceNumber ELSE CD.TransactionReference  END			
								, DepositStatus = CASE DT.[LookUpKey] WHEN 'MULTI_DEPOSIT' THEN  CDDT.DropStatus ELSE SD.[Name] END --, SD.[Name] DepositStatus
								, C.SealNumber ContainerSerialNumber
								, C.SerialNumber ContainerSealNumber		
								, CD.DepositedAmount TotalDeclaredValue
								, CD.ActualAmount TotalVerifiedValue
								, CD.ActualAmount TotalSettlementAmount
								, CD.DiscrepancyAmount TotalDepositDiscrepancy
								, C.ContainerId
								, CT.[Name] ContainerType
								, C.ReferenceNumber BagReferenceNumber
								, CD.ProcessedDateTime VerifiedDateAndTime
								, RIGHT('0' + convert(varchar(5),DateDiff(s, CD.SubmitDateTime,CD.ProcessedDateTime)/3600),2)+':'+RIGHT('0' + convert(varchar(5),DateDiff(s, CD.SubmitDateTime,CD.ProcessedDateTime)%3600/60),2)+':'+RIGHT('0' + convert(varchar(5),(DateDiff(s, CD.SubmitDateTime,CD.ProcessedDateTime)%60)),2) ProcessedDuration
								, CD.SettledDateTime TransactionSettlementDate
								, CASE WHEN PT.LookUpKey = 'MYSBV_VAULT' AND ST.LookUpKey = 'REAL_DAY_SETTLEMENT' 
									THEN RIGHT('0' + convert(varchar(5),DateDiff(s, CD.SubmitDateTime,CD.SettledDateTime)/3600),2)+':'+RIGHT('0' + convert(varchar(5),DateDiff(s, CD.SubmitDateTime,CD.SettledDateTime)%3600/60),2)+':'+RIGHT('0' + convert(varchar(5),(DateDiff(s, CD.SubmitDateTime,CD.SettledDateTime)%60)),2) 
									WHEN PT.LookUpKey = 'MYSBV_VAULT' AND ST.LookUpKey <>'REAL_DAY_SETTLEMENT' 
									THEN RIGHT('0' + convert(varchar(5),DateDiff(s, CD.ProcessedDateTime,CD.SettledDateTime)/3600),2)+':'+RIGHT('0' + convert(varchar(5),DateDiff(s, CD.ProcessedDateTime,CD.SettledDateTime)%3600/60),2)+':'+RIGHT('0' + convert(varchar(5),(DateDiff(s, CD.ProcessedDateTime,CD.SettledDateTime)%60)),2) 
									ELSE RIGHT('0' + convert(varchar(5),DateDiff(s, CD.ProcessedDateTime,CD.SettledDateTime)/3600),2)+':'+RIGHT('0' + convert(varchar(5),DateDiff(s, CD.ProcessedDateTime,CD.SettledDateTime)%3600/60),2)+':'+RIGHT('0' + convert(varchar(5),(DateDiff(s, CD.ProcessedDateTime,CD.SettledDateTime)%60)),2) 
									END SettlementDuration --Time difference from when deposit was made against time when money was banked 
								, CD.SendDateTime TransactionLodgeDateTime		
								, SetT.[Name] SettlementType --Net settlement or Gross settlement
								, ST.[Name] SettlementTime --Sameday or Next day settlement
								, UP.FirstName + ' ' + UP.LastName TellerName
								, US.FirstName + ' ' + US.LastName SupervisorName
								, CC.Name CashCentre
								--, S.CitCode
								, CitC.[Name] CITCarrier
								, TT.[Name] TransactionType
								, ISNULL(UC.FirstName + ' ' + UC.LastName, UC.UserName) [UserName]	
								, Cur.[Name] Currency
								, C.Amount ContainerTotalDeclaredValue
								, C.ActualAmount ContainerTotalVerifiedValue
								, C.ActualAmount ContainerSettlementAmount
								, C.DiscrepancyAmount ContainerDepositDiscrepancy
								--, C.IsPrimaryContainer
								--------------------------------------------------------------------
								, CDDT.BagSerialNumber
								, CDDT.Narrative
								, CDDT.ReferenceNumber
								, CDDT.Amount DeclareAmount
								, CDDT.ActualAmount 
								, CDDT.DiscrepancyAmount
								, CDDT.Error
								, CDDT.Comment		
								, CDDT.Number
								, CDDT.DenominationId
								, CDDT.DenominationType
								, ISNULL(CDDT.DenominationTypeDescription, (SELECT TOP 1 [Description] FROM Denomination ORDER BY ValueInCents DESC)) DenominationTypeDescription
								, ISNULL(CDDT.ValueInCents, (SELECT TOP 1 ValueInCents FROM Denomination ORDER BY ValueInCents DESC)) SortOrder
								, CDDT.DropStatus
								, CDDT.DepositCount
								, CDDT.DepositValue
								, CDDT.DiscrepancyCount
								, CDDT.DiscrepancyValue
								, CDDT.ActualCount
								, CDDT.ActualValue
						FROM CashDeposit CD LEFT JOIN DepositType DT
												ON CD.DepositTypeId = DT.DepositTypeId
											LEFT JOIN [Site] S
													ON CD.SiteId = S.SiteId
											LEFT JOIN Merchant M
													ON S.MerchantId = M.MerchantId
											LEFT JOIN City 
													ON S.CityId = City.CityId
											LEFT JOIN Device D
													ON CD.DeviceId = D.DeviceId
											LEFT JOIN ProductType PT
													ON CD.ProductTypeId = PT.ProductTypeId
											LEFT JOIN Container C
													ON CD.CashDepositId = C.CashDepositId
													AND C.IsNotDeleted = 1
											LEFT JOIN ContainerType CT
													ON C.ContainerTypeId = CT.ContainerTypeId
											LEFT JOIN [Status] SD
													ON CD.StatusId = SD.StatusId
											LEFT JOIN Account A 
													ON CD.AccountId = A.AccountId
											LEFT JOIN Bank B
													ON A.BankId = B.BankId
											LEFT JOIN [User] UP
													ON CD.ProcessedById = UP.UserId
											LEFT JOIN [User] US
													ON CD.SupervisorId = US.UserId
											LEFT JOIN CashCenter CC
													ON S.CashCenterId = CC.CashCenterId
											LEFT JOIN CitCarrier CitC
													ON S.CitCarrierId = CitC.CitCarrierId
											LEFT JOIN TransactionType TT
													ON A.TransactionTypeId = TT.TransactionTypeId	
											LEFT JOIN [User] UC
													ON CD.CreatedById = UC.UserId
											LEFT JOIN Province Pro
													ON City.ProvinceId = Pro.ProvinceId
											LEFT JOIN Country CTY
													ON Pro.CountryId = CTY.CountryId
											LEFT JOIN Currency Cur
													ON CTY.CountryId = Cur.CountryId
											LEFT JOIN CashDepositDetail CDDT
													ON C.ContainerId = CDDT.ContainerId
						--Get Service Type and Settlement Type--
											LEFT JOIN Product P
													ON PT.ProductTypeId = P.ProductTypeId
													AND CD.SiteId = P.SiteId
													AND CD.CreateDate BETWEEN P.ImplementationDate AND P.TerminationDate
											LEFT JOIN ServiceType ST
													ON P.ServiceTypeId = ST.ServiceTypeId
											LEFT JOIN SettlementType SetT
													ON P.SettlementTypeId = SetT.SettlementTypeId
					

						WHERE CD.IsNotDeleted = 1
						AND CD.SiteId IN (SELECT SiteId FROM [dbo].[fnRptUserSites] (@UserID))
						AND CONVERT(DATE,CD.CreateDate) BETWEEN @FromDate and @ToDate
						-- AC.ClusterName = CASE WHEN @Cluster = 'All' THEN AC.ClusterName ELSE @Cluster END
						--AND M.[Name] IN (SELECT * FROM [dbo].[fnRptStringToTable] (@Merchant, ',',1)) 
						AND M.[Name] = CASE WHEN @Merchant = 'All' THEN M.[Name] ELSE @Merchant END
						AND S.[Name] IN (SELECT * FROM [dbo].[fnRptStringToTable] (@Site, ',',1))  
						--AND S.CitCode IN (SELECT * FROM [dbo].[fnRptStringToTable] (@CITCode, ',',1)) 
						AND S.CitCode = CASE WHEN @CITCode = 'All' THEN S.CitCode ELSE @CITCode END
						AND CC.[Name] IN (SELECT * FROM [dbo].[fnRptStringToTable] (@CashCentre, ',',1)) 
						AND C.SerialNumber LIKE CASE WHEN @ContainerNo IS NULL OR @ContainerNo = '' THEN C.SerialNumber  ELSE (@ContainerNo + '%') END
						AND PT.[Name] IN (SELECT * FROM [dbo].[fnRptStringToTable] (@ProductType, ',',1))  
						AND TT.[Name] IN (SELECT * FROM [dbo].[fnRptStringToTable] (@TransactionType, ',',1)) 
						AND ((CD.DepositedAmount <= CASE WHEN @Amount = 0.00 THEN CD.DepositedAmount  ELSE @Amount END AND CD.DepositedAmount >= CASE WHEN @Amount = 0.00 THEN CD.DepositedAmount  ELSE @Amount END) OR (CAST(RIGHT(CD.DepositedAmount,LEN(CD.DepositedAmount)) AS VARCHAR(250)) LIKE '%' + CAST(LEFT(@Amount,LEN(@Amount)) AS VARCHAR(250))  + '%'))

						ORDER BY CD.CreateDate, CD.TransactionReference

						END 
            ");

        #endregion

        #region Vault Report

        public const string VaultReport =
            (@"
            CREATE PROCEDURE [dbo].[usp_VaultReport] 
                     @FromDate AS DATE,
                     @ToDate AS DATE
            AS
            BEGIN

                   -- SET NOCOUNT ON added to prevent extra result sets from
                   -- interfering with SELECT statements.
                   SET NOCOUNT ON;

                           DECLARE @VaultTransactions TABLE
                           (
                                  [VaultTransactionXmlId] [int] NULL,
                                  BeneficiaryCode VARCHAR(MAX) NULL,
                                  CitCode VARCHAR(MAX) NULL,
                                  DeviceSerial VARCHAR(MAX) NULL,
                                  TransactionDate VARCHAR(MAX) NULL,
                                  TotalValue FLOAT NULL,
                                  BagSerialNumber VARCHAR(MAX) NULL,
                                  [StatusId] [int] NULL,
                                  ErrorMessages VARCHAR(MAX) NULL
                           )


                           DECLARE @DumpedFailedVault TABLE
                           (
                                  [VaultTransactionXmlId] [int] NULL,
                                  [StatusId] [int] NOT NULL,
                                  [XmlMessage] [nvarchar](MAX) NULL
                           )

                           INSERT INTO @DumpedFailedVault
                           SELECT [VaultTransactionXmlId], [StatusId], [XmlMessage]
                           FROM VaultTransactionXml
                           ORDER BY [VaultTransactionXmlId]


                           DECLARE @MinXmlId INT = (SELECT MIN(VaultTransactionXmlId) FROM @DumpedFailedVault)
                           DECLARE @MaxXmlId INT = (SELECT MAX(VaultTransactionXmlId) FROM @DumpedFailedVault)

                           WHILE @MinXmlId <= @MaxXmlId
                           BEGIN
                                  DECLARE @xml xml = (SELECT TOP 1 dbo.fn_parse_json2xml([XmlMessage]) FROM VaultTransactionXml WHERE [VaultTransactionXmlId] = @MinXmlId)
                                  DECLARE @StatusId INT = (SELECT TOP 1 StatusId FROM VaultTransactionXml WHERE [VaultTransactionXmlId] = @MinXmlId)
                                  DECLARE @TransactionDate VARCHAR(50) = (SELECT TOP 1 CONVERT(VARCHAR(50), TransactionDate, 120)  FROM VaultTransactionXml WHERE [VaultTransactionXmlId] = @MinXmlId)
                                  DECLARE @Errors XML = (SELECT TOP 1 ErrorMessages FROM VaultTransactionXml WHERE [VaultTransactionXmlId] = @MinXmlId)

								  DECLARE @ErrorMsgTable TABLE 
								  (
								  	  Id INT NOT NULL IDENTITY,
								      ErrorMessage VARCHAR(max)
								  )

								  INSERT INTO @ErrorMsgTable
								  SELECT xmlData.Col.value('.[1]','varchar(max)') AS ErrorMessage
								  FROM @Errors.nodes('//DataHolderOfListOfDataHolderOfInt32/Object/DataHolderOfInt32/DataString') xmlData(Col);

								  DECLARE @ErrorMessage VARCHAR(MAX)
								  DECLARE @minId INT = (SELECT MIN(id) FROM @ErrorMsgTable)
								  DECLARE @MaxId INT = (SELECT MAX(id) FROM @ErrorMsgTable)

								  WHILE @minId <= @minId
									BEGIN
										(SELECT @ErrorMessage = COALESCE(@ErrorMessage + '', ' ') + ErrorMessage FROM @ErrorMsgTable WHERE id = @minId)
										 SET @minId = (SELECT MIN(id) FROM @ErrorMsgTable WHERE id > @minId)
								  END

                                  INSERT INTO @VaultTransactions
                                  SELECT @MinXmlId,
                                            @xml.value('(/BeneficiaryCode)[1]', 'varchar(50)') BeneficiaryCode,
                                            @xml.value('(/CitCode)[1]', 'varchar(50)') CitCode,
                                            @xml.value('(/DeviceSerial)[1]', 'varchar(50)') DeviceSerial,
                                            @TransactionDate TransactionDate,
                                            @xml.value('(/Currencies/Denominations/TotalValue)[1]', 'varchar(50)') TotalValue,
                                            @xml.value('(/CollectionUnits/CollectionUnit)[1]', 'varchar(50)') BagSerialNumber,
                                            @StatusId,
                                            @ErrorMessage
                                  SET @MinXmlId = (SELECT MIN(VaultTransactionXmlId) FROM @DumpedFailedVault WHERE VaultTransactionXmlId > @MinXmlId)
									
									SET @ErrorMessage = ''
									DELETE @ErrorMsgTable
						   END

                           SELECT 
                                         V.[VaultTransactionXmlId],
                                         V.[BeneficiaryCode],
                                         V.[CitCode],
                                         V.[DeviceSerial],
                                         V.[TransactionDate],
                                         V.[TotalValue],
                                         V.[BagSerialNumber],
                                         V.[ErrorMessages], 
                                         S.[Description] as [StatusDescription],
                                         L.[Name] SiteName,
                                         M.[Name] AS MerchantName
                           FROM @VaultTransactions V LEFT OUTER JOIN [Status] S ON V.StatusId = S.StatusId
                                  LEFT OUTER JOIN [Site] L ON L.CitCode = V.CitCode 
                                   LEFT OUTER JOIN Merchant M ON M.MerchantId = L.MerchantId
						   WHERE CONVERT(DATE,V.TransactionDate) BETWEEN @FromDate and @ToDate
						   ORDER BY V.[TransactionDate] DESC
            END   
            ");

        #endregion
        
        #region Hyphen Batch Report

        public const string HyphenBatchReport =
            (@"
                CREATE PROCEDURE [dbo].[usp_Hyphen_BatchReport] 
                            @FromDate AS DATE,
                            @ToDate AS DATE,
			                @Site AS VARCHAR(MAX)
                AS
                BEGIN
                        -- SET NOCOUNT ON added to prevent extra result sets from
                        -- interfering with SELECT statements.
                        SET NOCOUNT ON;

		                SELECT [BankType]
			                  ,[ClientReference]
			                  ,[Amount] = CONVERT(DECIMAL(18,2),[Amount])
			                  ,[DateActioned]
			                  ,[ClientSite]
			                  ,[MySbvReference]
			                  ,[Status]
			                  ,[UniqueUserCode]
			                  ,[BatchNumber]
			                  ,[DateSentToHyphen]
			                  ,[AccountNumber]
			                  ,[BranchCode]
			                  ,[TransactionType]
		                  FROM [dbo].[HyphenRecon]
		                WHERE ([DateSentToHyphen] BETWEEN @FromDate AND @ToDate) AND
			                   LTRIM(RTRIM([ClientSite])) IN (SELECT * FROM [dbo].[fnRptStringToTable] (@Site, ',',1)) 
		                ORDER BY [DateSentToHyphen] DESC
                END    
            ");

        #endregion

        #region Vault Deposit Processing

        public const string VaultDepositProcessing =
            (@"
            CREATE PROCEDURE [dbo].[usp_VaultDeposit_Processing]
	                    @CashDepositId INT = 0
                    AS
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						
		                DECLARE @Denominations TABLE 
		                (
			                DenominationId INT,  
							DenominationName VARCHAR(20), 
			                ValueInCents INT, 
							DenominationTypeName VARCHAR(20), 
			                DenominationCount INT, 
							DenominationValue FLOAT, 
							DropBagSerialNumber VARCHAR(20),
																
			                CashValueVerified FLOAT, 
			                DenominationCountVerified INT, 								 
			                CashValueVariance FLOAT, 
			                CountVariance INT
		                )								
							
						--ENUMERATE ALL DENOMINATION
						INSERT INTO @Denominations
							SELECT DISTINCT D.DenominationId, D.Name, D.ValueInCents, T.Name, 0, 0, NULL, 0, 0, 0, 0
							FROM Denomination D INNER JOIN DenominationType T ON D.DenominationTypeId = T.DenominationTypeId


								--UPDATE CONTAINER DROP	
								UPDATE D
									SET D.DenominationCount = Data.[Count],
										D.DenominationValue = Data.[Value],
										D.DropBagSerialNumber = Data.[SerialNumber],

										D.CashValueVerified = (SELECT CASE Data.[HasDiscrepancy] 
																	WHEN 0 THEN Data.[Value] 
																	ELSE Data.[DiscrepancyValue] END),
										D.DenominationCountVerified = (SELECT CASE Data.[HasDiscrepancy] WHEN 0 THEN Data.[Count] ELSE Data.[DiscrepancyCount] END),

										D.CashValueVariance = (SELECT CASE Data.[HasDiscrepancy] 
																	WHEN 0 THEN 0
																	ELSE 
																		Data.[DiscrepancyValue] - Data.[Value] 
																	END),

										D.CountVariance = (SELECT CASE Data.[HasDiscrepancy] 
																WHEN 0 THEN 0
																ELSE 
																	ABS(Data.[DiscrepancyCount] - Data.[Count])
																END)
								FROM 
								(		
									SELECT D.* 
										  ,V.[SerialNumber]
									FROM [dbo].[CashDeposit] CD INNER JOIN [dbo].[VaultContainer] V ON CD.CashDepositId = V.CashDepositId INNER JOIN 
									[dbo].[VaultContainerDrop] D on D.VaultContainerId = V.VaultContainerId
									WHERE CD.CashDepositId = @CashDepositId


								) Data INNER JOIN @Denominations D ON D.DenominationId = Data.DenominationId
																		

							SELECT *	FROM @Denominations
							ORDER BY ValueInCents DESC
                    END 
            ");

        #endregion

		#region Methods

		public static void AddIndexes(Context context)
		{
			CreateIndex(context, "RecordId", "AuditLog");
			CreateIndex(context, "CreateDate", "AuditLog");
			CreateIndex(context, "LastChangedDate", "AuditLog");
			CreateIndex(context, "CreatedById", "AuditLog");
			CreateIndex(context, "LastChangedById", "AuditLog");
		}

		private static void CreateIndex(Context context, string field, string table, bool unique = false)
		{
			context.Database.ExecuteSqlCommand(
				string.Format(
					"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name LIKE " + "'IX_" + table + "_%" + field +
					"%') " +
					"CREATE {0} NONCLUSTERED INDEX IX_{1}_{2} ON {1} ({3})",
					unique ? "UNIQUE " : "",
					table,
					field.Replace(",", "_"),
					field));
		}

		#endregion

	}
}