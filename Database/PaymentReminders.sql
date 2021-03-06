-- SELECT * FROM  dbo.PaymentReminders
ALTER View dbo.PaymentReminders
AS

SELECT A.AgreementReference, 
		A.NextDueDate,
		A.ArrearsState,
		A.MIArrearsState,
		ABA.BespokeArrearsState,
		C.Forename,
		C.Surname,
		C.MobilePhoneSTDCode,
		C.MobilePhoneNumber
  FROM [BMFLReporting].[dbo].[Agreements] A
  INNER JOIN [BMFLReporting].dbo.Agreement_Bespoke_Arrears ABA ON A.AgreementReference = ABA.AgreementReference
  INNER JOIN [BMFLReporting].dbo.Customers C ON A.CustomerId = C.CustomerId
  WHERE AgreementStatus = 1
  AND MIArrearsState <> 0
  AND NextDueDate = DATEADD(DAY,1,CAST(GETDATE() AS DATE))
  AND NOT EXISTS (SELECT * FROM [BMFLReporting].dbo.Agreement_Classifications AC 
					WHERE A.AgreementReference = AC.AgreementReference 
					AND AC.AgreementClassification IN (SELECT AgreementClassification FROM [BMFLReporting].dbo.Agreement_Classification_Types WHERE [Description] IN ('Voluntary Termination', 'Voluntary Surrender', 'Default Notice Issued', 'Arrangement To Pay'))
					AND AC.EffectiveFrom_Date < GETDATE()
					AND GETDATE() < ISNULL(AC.EffectiveTo_Date, GETDATE()+1)
					)
