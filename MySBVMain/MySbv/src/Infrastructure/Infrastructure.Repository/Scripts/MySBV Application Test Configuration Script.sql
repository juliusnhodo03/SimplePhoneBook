  
  --MySBV System Configuration Deployment Script-- 
  
  UPDATE SystemConfiguration
  SET Value = 'http://mysb.sbv.co.za/'
  WHERE LookUpKey = 'SERVER_ADDRESS'

  UPDATE SystemConfiguration
  SET Value = '\\150.150.1.45\mysbv\OUT'
  WHERE LookUpKey = 'DROP_PATH'

  UPDATE SystemConfiguration
  SET Value = '\\150.150.1.45\mysbv\IN'
  WHERE LookUpKey = 'PICKUP_PATH'

  UPDATE SystemConfiguration
  SET Value = '\\150.150.1.45\mysbv\ARCHIVE'
  WHERE LookUpKey = 'ARCHIVE_PATH'

  UPDATE SystemConfiguration
  SET Value = 'mySBV.admin@sbv.co.za'
  WHERE LookUpKey = 'REJECTED_DEPOSIT_EMAIL'

  UPDATE dbo.SystemConfiguration
  SET Value = 'D:/Vault.Integration.ResponseClient/ResponseMessage'
  WHERE LookUpKey = 'RESPONSE_MESSAGE_XML_PATH'




