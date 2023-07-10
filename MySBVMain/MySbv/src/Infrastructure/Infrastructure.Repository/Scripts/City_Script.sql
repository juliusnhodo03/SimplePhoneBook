USE [MySBV]
GO

--clear table
DELETE FROM [MySBV].[dbo].[City]


--INSERT DATA
SET IDENTITY_INSERT [dbo].[City] ON 

GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (2, 1, N'EastRand', N'East Rand', N'EASTRAND', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (3, 1, N'Midrand', N'Midrand', N'MIDRAND', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (4, 1, N'Vereeniging', N'Vereeniging', N'VEREENIGING', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (5, 1, N'Pretoria', N'Pretoria', N'PRETORIA', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (6, 7, N'Rustenburg', N'Rustenburg', N'RUSTENBURG', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (7, 6, N'Witbank', N'Witbank', N'WITBANK', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (8, 6, N'Nelspruit', N'Nelspruit', N'NELSPRUIT', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (9, 5, N'Polokwane', N'Polokwane', N'POLOKWANE', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (10, 6, N'Burgesfort', N'Burgesfort', N'BURGESFORT', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (11, 5, N'Louistrichard', N'Louistrichard', N'LOUISTRICHARD', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (12, 4, N'Durban', N'Durban', N'DURBAN', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (13, 4, N'Richardsbay', N'Richardsbay', N'RICHARDSBAY', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (14, 1, N'Pietermaritzburg', N'Pietermaritzburg', N'PIETERMARITZBURG', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (15, 4, N'PortShepstone', N'Port Shepstone', N'PORTSHEPSTONE', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (16, 2, N'EastLondon', N'East London', N'EASTLONDON', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (17, 4, N'Mthatha', N'Mthatha', N'MTHATHA', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (18, 2, N'PortElizabeth', N'Port Elizabeth', N'PORTELIZABETH', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (19, 9, N'George', N'George', N'GEORGE', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (20, 9, N'CapeTown', N'Cape Town', N'CAPETOWN', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (21, 8, N'Kimberley', N'Kimberley', N'KIMBERLEY', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (22, 10, N'Maseru', N'Maseru', N'MASERU', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (23, 9, N'Worcester', N'Worcester', N'WORCESTER', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (24, 4, N'Newcastle', N'Newcastle', N'NEWCASTLE', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (25, 4, N'Greyville', N'Greyville', N'GREYVILLE', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (26, 3, N'Bloemfontein', N'Bloemfontein', N'BLOMFONTEIN', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (662, 9, N'Aan de Doorns', N'Aan de Doorns', N'AAN_DE_DOORNS', 1, 1, CAST(0x0000A2ED00E045AC AS DateTime), 1, CAST(0x0000A2ED00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (663, 5, N'Aberdeen', N'Aberdeen', N'ABERDEEN', 1, 1, CAST(0x0000A2ED00F0C06C AS DateTime), 1, CAST(0x0000A2ED00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (664, 3, N'Aberfeldy', N'Aberfeldy', N'ABERFELDY', 1, 1, CAST(0x0000A2ED01013B2C AS DateTime), 1, CAST(0x0000A2ED01013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (665, 9, N'Abbotsdale', N'Abbotsdale', N'ABBOTSDALE', 1, 1, CAST(0x0000A2ED0111B5EC AS DateTime), 1, CAST(0x0000A2ED0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (666, 6, N'Acornhoek', N'Acornhoek', N'ACORNHOEK', 1, 1, CAST(0x0000A2ED012230AC AS DateTime), 1, CAST(0x0000A2ED012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (667, 5, N'Adelaide', N'Adelaide', N'ADELAIDE', 1, 1, CAST(0x0000A2ED0132AB6C AS DateTime), 1, CAST(0x0000A2ED0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (668, 5, N'Adendorp', N'Adendorp', N'ADENDORP', 1, 1, CAST(0x0000A2ED0143262C AS DateTime), 1, CAST(0x0000A2ED0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (669, 5, N'Addo', N'Addo', N'ADDO', 1, 1, CAST(0x0000A2ED0153A0EC AS DateTime), 1, CAST(0x0000A2ED0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (670, 2, N'Afguns', N'Afguns', N'AFGUNS', 1, 1, CAST(0x0000A2ED01641BAC AS DateTime), 1, CAST(0x0000A2ED01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (671, 8, N'Aggeneys', N'Aggeneys', N'AGGENEYS', 1, 1, CAST(0x0000A2ED0174966C AS DateTime), 1, CAST(0x0000A2ED0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (672, 9, N'L''Agulhas or Agulhas', N'L''Agulhas or Agulhas', N'L''AGULHAS OR_AGULHAS', 1, 1, CAST(0x0000A2ED0185112C AS DateTime), 1, CAST(0x0000A2ED0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (673, 4, N'Ahrens', N'Ahrens', N'AHRENS', 1, 1, CAST(0x0000A2EE000A09EC AS DateTime), 1, CAST(0x0000A2EE000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (674, 1, N'Akasia', N'Akasia', N'AKASIA', 1, 1, CAST(0x0000A2EE001A84AC AS DateTime), 1, CAST(0x0000A2EE001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (675, 9, N'Albertinia', N'Albertinia', N'ALBERTINIA', 1, 1, CAST(0x0000A2EE002AFF6C AS DateTime), 1, CAST(0x0000A2EE002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (676, 1, N'Alberton', N'Alberton', N'ALBERTON', 1, 1, CAST(0x0000A2EE003B7A2C AS DateTime), 1, CAST(0x0000A2EE003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (677, 7, N'Albertshoek', N'Albertshoek', N'ALBERTSHOEK', 1, 1, CAST(0x0000A2EE004BF4EC AS DateTime), 1, CAST(0x0000A2EE004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (678, 5, N'Alderley', N'Alderley', N'ALDERLEY', 1, 1, CAST(0x0000A2EE005C6FAC AS DateTime), 1, CAST(0x0000A2EE005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (679, 4, N'Aldinville', N'Aldinville', N'ALDINVILLE', 1, 1, CAST(0x0000A2EE006CEA6C AS DateTime), 1, CAST(0x0000A2EE006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (680, 8, N'Alexander Bay', N'Alexander Bay', N'ALEXANDER_BAY', 1, 1, CAST(0x0000A2EE007D652C AS DateTime), 1, CAST(0x0000A2EE007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (681, 5, N'Alexandria', N'Alexandria', N'ALEXANDRIA', 1, 1, CAST(0x0000A2EE008DDFEC AS DateTime), 1, CAST(0x0000A2EE008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (682, 1, N'Alexandra', N'Alexandra', N'ALEXANDRA', 1, 1, CAST(0x0000A2EE009E5AAC AS DateTime), 1, CAST(0x0000A2EE009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (683, 7, N'Alettasrus', N'Alettasrus', N'ALETTASRUS', 1, 1, CAST(0x0000A2EE00AED56C AS DateTime), 1, CAST(0x0000A2EE00AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (684, 5, N'Alice', N'Alice', N'ALICE', 1, 1, CAST(0x0000A2EE00BF502C AS DateTime), 1, CAST(0x0000A2EE00BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (685, 5, N'Alicedale', N'Alicedale', N'ALICEDALE', 1, 1, CAST(0x0000A2EE00CFCAEC AS DateTime), 1, CAST(0x0000A2EE00CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (686, 5, N'Aliwal North', N'Aliwal North', N'ALIWAL_NORTH', 1, 1, CAST(0x0000A2EE00E045AC AS DateTime), 1, CAST(0x0000A2EE00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (687, 3, N'Allandale', N'Allandale', N'ALLANDALE', 1, 1, CAST(0x0000A2EE00F0C06C AS DateTime), 1, CAST(0x0000A2EE00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (689, 2, N'Alldays', N'Alldays', N'ALLDAYS', 1, 1, CAST(0x0000A2EE0111B5EC AS DateTime), 1, CAST(0x0000A2EE0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (690, 3, N'Allep', N'Allep', N'ALLEP', 1, 1, CAST(0x0000A2EE012230AC AS DateTime), 1, CAST(0x0000A2EE012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (691, 4, N'Alpha', N'Alpha', N'ALPHA', 1, 1, CAST(0x0000A2EE0132AB6C AS DateTime), 1, CAST(0x0000A2EE0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (692, 7, N'Amalia', N'Amalia', N'AMALIA', 1, 1, CAST(0x0000A2EE0143262C AS DateTime), 1, CAST(0x0000A2EE0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (693, 9, N'Amalienstein', N'Amalienstein', N'AMALIENSTEIN', 1, 1, CAST(0x0000A2EE0153A0EC AS DateTime), 1, CAST(0x0000A2EE0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (694, 4, N'Amanzimtoti', N'Amanzimtoti', N'AMANZIMTOTI', 1, 1, CAST(0x0000A2EE01641BAC AS DateTime), 1, CAST(0x0000A2EE01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (695, 6, N'Amersfoort', N'Amersfoort', N'AMERSFOORT', 1, 1, CAST(0x0000A2EE0174966C AS DateTime), 1, CAST(0x0000A2EE0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (696, 6, N'Amsterdam', N'Amsterdam', N'AMSTERDAM', 1, 1, CAST(0x0000A2EE0185112C AS DateTime), 1, CAST(0x0000A2EE0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (697, 4, N'Anerley', N'Anerley', N'ANERLEY', 1, 1, CAST(0x0000A2EF000A09EC AS DateTime), 1, CAST(0x0000A2EF000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (698, 8, N'Andriesvale', N'Andriesvale', N'ANDRIESVALE', 1, 1, CAST(0x0000A2EF001A84AC AS DateTime), 1, CAST(0x0000A2EF001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (699, 3, N'Arlington', N'Arlington', N'ARLINGTON', 1, 1, CAST(0x0000A2EF002AFF6C AS DateTime), 1, CAST(0x0000A2EF002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (700, 9, N'Arniston', N'Arniston', N'ARNISTON', 1, 1, CAST(0x0000A2EF003B7A2C AS DateTime), 1, CAST(0x0000A2EF003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (701, 9, N'Ashton', N'Ashton', N'ASHTON', 1, 1, CAST(0x0000A2EF004BF4EC AS DateTime), 1, CAST(0x0000A2EF004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (702, 8, N'Askham', N'Askham', N'ASKHAM', 1, 1, CAST(0x0000A2EF005C6FAC AS DateTime), 1, CAST(0x0000A2EF005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (703, 9, N'Atlantis', N'Atlantis', N'ATLANTIS', 1, 1, CAST(0x0000A2EF006CEA6C AS DateTime), 1, CAST(0x0000A2EF006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (704, 1, N'Atteridgeville', N'Atteridgeville', N'ATTERIDGEVILLE', 1, 1, CAST(0x0000A2EF007D652C AS DateTime), 1, CAST(0x0000A2EF007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (705, 8, N'Augrabies', N'Augrabies', N'AUGRABIES', 1, 1, CAST(0x0000A2EF008DDFEC AS DateTime), 1, CAST(0x0000A2EF008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (706, 9, N'Aurora', N'Aurora', N'AURORA', 1, 1, CAST(0x0000A2EF009E5AAC AS DateTime), 1, CAST(0x0000A2EF009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (707, 9, N'Baardskeerdersbos', N'Baardskeerdersbos', N'BAARDSKEERDERSBOS', 1, 1, CAST(0x0000A2EF00AED56C AS DateTime), 1, CAST(0x0000A2EF00AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (708, 7, N'Babelegi', N'Babelegi', N'BABELEGI', 1, 1, CAST(0x0000A2EF00BF502C AS DateTime), 1, CAST(0x0000A2EF00BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (709, 4, N'Babanango', N'Babanango', N'BABANANGO', 1, 1, CAST(0x0000A2EF00CFCAEC AS DateTime), 1, CAST(0x0000A2EF00CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (710, 6, N'Badplaas', N'Badplaas', N'BADPLAAS', 1, 1, CAST(0x0000A2EF00E045AC AS DateTime), 1, CAST(0x0000A2EF00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (711, 5, N'Bailey', N'Bailey', N'BAILEY', 1, 1, CAST(0x0000A2EF00F0C06C AS DateTime), 1, CAST(0x0000A2EF00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (712, 7, N'Bakerville', N'Bakerville', N'BAKERVILLE', 1, 1, CAST(0x0000A2EF01013B2C AS DateTime), 1, CAST(0x0000A2EF01013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (713, 6, N'Balfour', N'Balfour', N'BALFOUR', 1, 1, CAST(0x0000A2EF0111B5EC AS DateTime), 1, CAST(0x0000A2EF0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (714, 5, N'Balfour', N'Balfour', N'BALFOUR', 1, 1, CAST(0x0000A2EF012230AC AS DateTime), 1, CAST(0x0000A2EF012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (715, 4, N'Balgowan', N'Balgowan', N'BALGOWAN', 1, 1, CAST(0x0000A2EF0132AB6C AS DateTime), 1, CAST(0x0000A2EF0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (716, 4, N'Ballengeich', N'Ballengeich', N'BALLENGEICH', 1, 1, CAST(0x0000A2EF0143262C AS DateTime), 1, CAST(0x0000A2EF0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (717, 4, N'Ballito', N'Ballito', N'BALLITO', 1, 1, CAST(0x0000A2EF0153A0EC AS DateTime), 1, CAST(0x0000A2EF0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (718, 6, N'Balmoral', N'Balmoral', N'BALMORAL', 1, 1, CAST(0x0000A2EF01641BAC AS DateTime), 1, CAST(0x0000A2EF01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (719, 2, N'Baltimore', N'Baltimore', N'BALTIMORE', 1, 1, CAST(0x0000A2EF0174966C AS DateTime), 1, CAST(0x0000A2EF0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (720, 4, N'Banana Beach', N'Banana Beach', N'BANANA_BEACH', 1, 1, CAST(0x0000A2EF0185112C AS DateTime), 1, CAST(0x0000A2EF0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (721, 2, N'Bandelierkop', N'Bandelierkop', N'BANDELIERKOP', 1, 1, CAST(0x0000A2F0000A09EC AS DateTime), 1, CAST(0x0000A2F0000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (722, 2, N'Bandur', N'Bandur', N'BANDUR', 1, 1, CAST(0x0000A2F0001A84AC AS DateTime), 1, CAST(0x0000A2F0001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (723, 1, N'Bank, 1', N'Bank, 1', N'BANK,_1', 1, 1, CAST(0x0000A2F0002AFF6C AS DateTime), 1, CAST(0x0000A2F0002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (724, 6, N'Bankkop', N'Bankkop', N'BANKKOP', 1, 1, CAST(0x0000A2F0003B7A2C AS DateTime), 1, CAST(0x0000A2F0003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (725, 2, N'Ba-Phalaborwa', N'Ba-Phalaborwa', N'BA-PHALABORWA', 1, 1, CAST(0x0000A2F0004BF4EC AS DateTime), 1, CAST(0x0000A2F0004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (726, 1, N'Bapsfontein', N'Bapsfontein', N'BAPSFONTEIN', 1, 1, CAST(0x0000A2F0005C6FAC AS DateTime), 1, CAST(0x0000A2F0005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (727, 5, N'Barakke', N'Barakke', N'BARAKKE', 1, 1, CAST(0x0000A2F0006CEA6C AS DateTime), 1, CAST(0x0000A2F0006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (728, 7, N'Barberspan', N'Barberspan', N'BARBERSPAN', 1, 1, CAST(0x0000A2F0007D652C AS DateTime), 1, CAST(0x0000A2F0007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (729, 6, N'Barberton', N'Barberton', N'BARBERTON', 1, 1, CAST(0x0000A2F0008DDFEC AS DateTime), 1, CAST(0x0000A2F0008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (730, 5, N'Barkly East', N'Barkly East', N'BARKLY_EAST', 1, 1, CAST(0x0000A2F0009E5AAC AS DateTime), 1, CAST(0x0000A2F0009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (731, 8, N'Barkly West', N'Barkly West', N'BARKLY_WEST', 1, 1, CAST(0x0000A2F000AED56C AS DateTime), 1, CAST(0x0000A2F000AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (732, 5, N'Baroda', N'Baroda', N'BARODA', 1, 1, CAST(0x0000A2F000BF502C AS DateTime), 1, CAST(0x0000A2F000BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (733, 5, N'Baroe', N'Baroe', N'BAROE', 1, 1, CAST(0x0000A2F000CFCAEC AS DateTime), 1, CAST(0x0000A2F000CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (734, 9, N'Barrington', N'Barrington', N'BARRINGTON', 1, 1, CAST(0x0000A2F000E045AC AS DateTime), 1, CAST(0x0000A2F000E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (735, 9, N'Barrydale', N'Barrydale', N'BARRYDALE', 1, 1, CAST(0x0000A2F000F0C06C AS DateTime), 1, CAST(0x0000A2F000F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (736, 5, N'Bathurst', N'Bathurst', N'BATHURST', 1, 1, CAST(0x0000A2F001013B2C AS DateTime), 1, CAST(0x0000A2F001013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (737, 9, N'Baviaan', N'Baviaan', N'BAVIAAN', 1, 1, CAST(0x0000A2F00111B5EC AS DateTime), 1, CAST(0x0000A2F00111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (738, 4, N'Bayala', N'Bayala', N'BAYALA', 1, 1, CAST(0x0000A2F0012230AC AS DateTime), 1, CAST(0x0000A2F0012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (739, 4, N'Bazley', N'Bazley', N'BAZLEY', 1, 1, CAST(0x0000A2F00132AB6C AS DateTime), 1, CAST(0x0000A2F00132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (740, 9, N'Beaufort West', N'Beaufort West', N'BEAUFORT_WEST', 1, 1, CAST(0x0000A2F00143262C AS DateTime), 1, CAST(0x0000A2F00143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (741, 2, N'Beauty', N'Beauty', N'BEAUTY', 1, 1, CAST(0x0000A2F00153A0EC AS DateTime), 1, CAST(0x0000A2F00153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (742, 5, N'Bedford', N'Bedford', N'BEDFORD', 1, 1, CAST(0x0000A2F001641BAC AS DateTime), 1, CAST(0x0000A2F001641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (743, 7, N'Beestekraal', N'Beestekraal', N'BEESTEKRAAL', 1, 1, CAST(0x0000A2F00174966C AS DateTime), 1, CAST(0x0000A2F00174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (744, 5, N'Behulpsaam', N'Behulpsaam', N'BEHULPSAAM', 1, 1, CAST(0x0000A2F00185112C AS DateTime), 1, CAST(0x0000A2F00185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (745, 2, N'Beitbridge', N'Beitbridge', N'BEITBRIDGE', 1, 1, CAST(0x0000A2F1000A09EC AS DateTime), 1, CAST(0x0000A2F1000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (746, 8, N'Bekker', N'Bekker', N'BEKKER', 1, 1, CAST(0x0000A2F1001A84AC AS DateTime), 1, CAST(0x0000A2F1001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (747, 2, N'Bela-Bela', N'Bela-Bela', N'BELA-BELA', 1, 1, CAST(0x0000A2F1002AFF6C AS DateTime), 1, CAST(0x0000A2F1002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (748, 6, N'Belfast', N'Belfast', N'BELFAST', 1, 1, CAST(0x0000A2F1003B7A2C AS DateTime), 1, CAST(0x0000A2F1003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (749, 5, N'Bell', N'Bell', N'BELL', 1, 1, CAST(0x0000A2F1004BF4EC AS DateTime), 1, CAST(0x0000A2F1004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (750, 5, N'Bellevue', N'Bellevue', N'BELLEVUE', 1, 1, CAST(0x0000A2F1005C6FAC AS DateTime), 1, CAST(0x0000A2F1005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (751, 9, N'Bellville', N'Bellville', N'BELLVILLE', 1, 1, CAST(0x0000A2F1006CEA6C AS DateTime), 1, CAST(0x0000A2F1006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (752, 8, N'Belmont', N'Belmont', N'BELMONT', 1, 1, CAST(0x0000A2F1007D652C AS DateTime), 1, CAST(0x0000A2F1007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (753, 4, N'Bendigo', N'Bendigo', N'BENDIGO', 1, 1, CAST(0x0000A2F1008DDFEC AS DateTime), 1, CAST(0x0000A2F1008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (754, 1, N'Benoni', N'Benoni', N'BENONI', 1, 1, CAST(0x0000A2F1009E5AAC AS DateTime), 1, CAST(0x0000A2F1009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (755, 6, N'Berbice', N'Berbice', N'BERBICE', 1, 1, CAST(0x0000A2F100AED56C AS DateTime), 1, CAST(0x0000A2F100AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (756, 9, N'Berea', N'Berea', N'BEREA', 1, 1, CAST(0x0000A2F100BF502C AS DateTime), 1, CAST(0x0000A2F100BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (757, 9, N'Bergplaas', N'Bergplaas', N'BERGPLAAS', 1, 1, CAST(0x0000A2F100CFCAEC AS DateTime), 1, CAST(0x0000A2F100CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (758, 9, N'Bergrivier', N'Bergrivier', N'BERGRIVIER', 1, 1, CAST(0x0000A2F100E045AC AS DateTime), 1, CAST(0x0000A2F100E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (759, 4, N'Bergville', N'Bergville', N'BERGVILLE', 1, 1, CAST(0x0000A2F100F0C06C AS DateTime), 1, CAST(0x0000A2F100F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (760, 5, N'Berlin', N'Berlin', N'BERLIN', 1, 1, CAST(0x0000A2F101013B2C AS DateTime), 1, CAST(0x0000A2F101013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (761, 8, N'Bermolli', N'Bermolli', N'BERMOLLI', 1, 1, CAST(0x0000A2F10111B5EC AS DateTime), 1, CAST(0x0000A2F10111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (762, 4, N'Besters', N'Besters', N'BESTERS', 1, 1, CAST(0x0000A2F1012230AC AS DateTime), 1, CAST(0x0000A2F1012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (763, 6, N'Bethal', N'Bethal', N'BETHAL', 1, 1, CAST(0x0000A2F10132AB6C AS DateTime), 1, CAST(0x0000A2F10132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (764, 7, N'Bethanie', N'Bethanie', N'BETHANIE', 1, 1, CAST(0x0000A2F10143262C AS DateTime), 1, CAST(0x0000A2F10143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (765, 5, N'Bethelsdorp', N'Bethelsdorp', N'BETHELSDORP', 1, 1, CAST(0x0000A2F10153A0EC AS DateTime), 1, CAST(0x0000A2F10153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (766, 3, N'Bethlehem', N'Bethlehem', N'BETHLEHEM', 1, 1, CAST(0x0000A2F101641BAC AS DateTime), 1, CAST(0x0000A2F101641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (767, 3, N'Bethulie', N'Bethulie', N'BETHULIE', 1, 1, CAST(0x0000A2F10174966C AS DateTime), 1, CAST(0x0000A2F10174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (768, 6, N'Bettiesdam', N'Bettiesdam', N'BETTIESDAM', 1, 1, CAST(0x0000A2F10185112C AS DateTime), 1, CAST(0x0000A2F10185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (769, 9, N'Betty''s Bay', N'Betty''s Bay', N'BETTY''S_BAY', 1, 1, CAST(0x0000A2F2000A09EC AS DateTime), 1, CAST(0x0000A2F2000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (770, 7, N'Bewley', N'Bewley', N'BEWLEY', 1, 1, CAST(0x0000A2F2001A84AC AS DateTime), 1, CAST(0x0000A2F2001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (771, 9, N'Biesiesfontein', N'Biesiesfontein', N'BIESIESFONTEIN', 1, 1, CAST(0x0000A2F2002AFF6C AS DateTime), 1, CAST(0x0000A2F2002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (772, 8, N'Biesiespoort', N'Biesiespoort', N'BIESIESPOORT', 1, 1, CAST(0x0000A2F2003B7A2C AS DateTime), 1, CAST(0x0000A2F2003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (773, 7, N'Biesiesvlei', N'Biesiesvlei', N'BIESIESVLEI', 1, 1, CAST(0x0000A2F2004BF4EC AS DateTime), 1, CAST(0x0000A2F2004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (774, 4, N'Biggarsberg', N'Biggarsberg', N'BIGGARSBERG', 1, 1, CAST(0x0000A2F2005C6FAC AS DateTime), 1, CAST(0x0000A2F2005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (775, 5, N'Bhisho', N'Bhisho', N'BHISHO', 1, 1, CAST(0x0000A2F2006CEA6C AS DateTime), 1, CAST(0x0000A2F2006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (776, 9, N'Bitterfontein', N'Bitterfontein', N'BITTERFONTEIN', 1, 1, CAST(0x0000A2F2007D652C AS DateTime), 1, CAST(0x0000A2F2007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (777, 5, N'Bityi|', N'Bityi|', N'BITYI|', 1, 1, CAST(0x0000A2F2008DDFEC AS DateTime), 1, CAST(0x0000A2F2008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (778, 3, N'Bloemfontein', N'Bloemfontein', N'BLOEMFONTEIN', 1, 1, CAST(0x0000A2F2009E5AAC AS DateTime), 1, CAST(0x0000A2F2009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (779, 7, N'Bloemhof', N'Bloemhof', N'BLOEMHOF', 1, 1, CAST(0x0000A2F200AED56C AS DateTime), 1, CAST(0x0000A2F200AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (780, 1, N'Boipatong', N'Boipatong', N'BOIPATONG', 1, 1, CAST(0x0000A2F200BF502C AS DateTime), 1, CAST(0x0000A2F200BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (781, 1, N'Boksburg', N'Boksburg', N'BOKSBURG', 1, 1, CAST(0x0000A2F200CFCAEC AS DateTime), 1, CAST(0x0000A2F200CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (782, 9, N'Bonnievale', N'Bonnievale', N'BONNIEVALE', 1, 1, CAST(0x0000A2F200E045AC AS DateTime), 1, CAST(0x0000A2F200E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (783, 5, N'Bonza Bay', N'Bonza Bay', N'BONZA_BAY', 1, 1, CAST(0x0000A2F200F0C06C AS DateTime), 1, CAST(0x0000A2F200F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (784, 1, N'Bophelong', N'Bophelong', N'BOPHELONG', 1, 1, CAST(0x0000A2F201013B2C AS DateTime), 1, CAST(0x0000A2F201013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (785, 6, N'Bosbokrand', N'Bosbokrand', N'BOSBOKRAND', 1, 1, CAST(0x0000A2F20111B5EC AS DateTime), 1, CAST(0x0000A2F20111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (786, 3, N'Boshof', N'Boshof', N'BOSHOF', 1, 1, CAST(0x0000A2F2012230AC AS DateTime), 1, CAST(0x0000A2F2012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (787, 4, N'Boston', N'Boston', N'BOSTON', 1, 1, CAST(0x0000A2F20132AB6C AS DateTime), 1, CAST(0x0000A2F20132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (788, 3, N'Bothaville', N'Bothaville', N'BOTHAVILLE', 1, 1, CAST(0x0000A2F20143262C AS DateTime), 1, CAST(0x0000A2F20143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (789, 3, N'Botshabelo', N'Botshabelo', N'BOTSHABELO', 1, 1, CAST(0x0000A2F20153A0EC AS DateTime), 1, CAST(0x0000A2F20153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (790, 1, N'Brakpan', N'Brakpan', N'BRAKPAN', 1, 1, CAST(0x0000A2F201641BAC AS DateTime), 1, CAST(0x0000A2F201641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (791, 3, N'Brandfort', N'Brandfort', N'BRANDFORT', 1, 1, CAST(0x0000A2F20174966C AS DateTime), 1, CAST(0x0000A2F20174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (792, 5, N'Braunschweig', N'Braunschweig', N'BRAUNSCHWEIG', 1, 1, CAST(0x0000A2F20185112C AS DateTime), 1, CAST(0x0000A2F20185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (793, 7, N'Bray', N'Bray', N'BRAY', 1, 1, CAST(0x0000A2F3000A09EC AS DateTime), 1, CAST(0x0000A2F3000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (794, 9, N'Bredasdorp', N'Bredasdorp', N'BREDASDORP', 1, 1, CAST(0x0000A2F3001A84AC AS DateTime), 1, CAST(0x0000A2F3001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (795, 6, N'Breyten', N'Breyten', N'BREYTEN', 1, 1, CAST(0x0000A2F3002AFF6C AS DateTime), 1, CAST(0x0000A2F3002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (796, 7, N'Brits', N'Brits', N'BRITS', 1, 1, CAST(0x0000A2F3003B7A2C AS DateTime), 1, CAST(0x0000A2F3003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (797, 8, N'Britstown', N'Britstown', N'BRITSTOWN', 1, 1, CAST(0x0000A2F3004BF4EC AS DateTime), 1, CAST(0x0000A2F3004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (798, 7, N'Broederstroom', N'Broederstroom', N'BROEDERSTROOM', 1, 1, CAST(0x0000A2F3005C6FAC AS DateTime), 1, CAST(0x0000A2F3005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (799, 1, N'Bronkhorstspruit', N'Bronkhorstspruit', N'BRONKHORSTSPRUIT', 1, 1, CAST(0x0000A2F3006CEA6C AS DateTime), 1, CAST(0x0000A2F3006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (800, 9, N'Buffelsjagbaai', N'Buffelsjagbaai', N'BUFFELSJAGBAAI', 1, 1, CAST(0x0000A2F3007D652C AS DateTime), 1, CAST(0x0000A2F3007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (801, 3, N'Bultfontein', N'Bultfontein', N'BULTFONTEIN', 1, 1, CAST(0x0000A2F3008DDFEC AS DateTime), 1, CAST(0x0000A2F3008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (802, 4, N'Bulwer', N'Bulwer', N'BULWER', 1, 1, CAST(0x0000A2F3009E5AAC AS DateTime), 1, CAST(0x0000A2F3009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (803, 5, N'Burgersdorp', N'Burgersdorp', N'BURGERSDORP', 1, 1, CAST(0x0000A2F300AED56C AS DateTime), 1, CAST(0x0000A2F300AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (804, 3, N'Boompie Alleen', N'Boompie Alleen', N'BOOMPIE_ALLEEN', 1, 1, CAST(0x0000A2F300BF502C AS DateTime), 1, CAST(0x0000A2F300BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (805, 5, N'Butterworth', N'Butterworth', N'BUTTERWORTH', 1, 1, CAST(0x0000A2F300CFCAEC AS DateTime), 1, CAST(0x0000A2F300CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (806, 5, N'Cala', N'Cala', N'CALA', 1, 1, CAST(0x0000A2F300E045AC AS DateTime), 1, CAST(0x0000A2F300E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (807, 9, N'Caledon', N'Caledon', N'CALEDON', 1, 1, CAST(0x0000A2F300F0C06C AS DateTime), 1, CAST(0x0000A2F300F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (808, 9, N'Calitzdorp', N'Calitzdorp', N'CALITZDORP', 1, 1, CAST(0x0000A2F301013B2C AS DateTime), 1, CAST(0x0000A2F301013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (809, 4, N'Calvert', N'Calvert', N'CALVERT', 1, 1, CAST(0x0000A2F30111B5EC AS DateTime), 1, CAST(0x0000A2F30111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (810, 8, N'Calvinia', N'Calvinia', N'CALVINIA', 1, 1, CAST(0x0000A2F3012230AC AS DateTime), 1, CAST(0x0000A2F3012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (811, 5, N'Cambria', N'Cambria', N'CAMBRIA', 1, 1, CAST(0x0000A2F30132AB6C AS DateTime), 1, CAST(0x0000A2F30132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (812, 8, N'Campbell', N'Campbell', N'CAMPBELL', 1, 1, CAST(0x0000A2F30143262C AS DateTime), 1, CAST(0x0000A2F30143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (813, 4, N'Camperdown', N'Camperdown', N'CAMPERDOWN', 1, 1, CAST(0x0000A2F30153A0EC AS DateTime), 1, CAST(0x0000A2F30153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (814, 9, N'Camps Bay', N'Camps Bay', N'CAMPS_BAY', 1, 1, CAST(0x0000A2F301641BAC AS DateTime), 1, CAST(0x0000A2F301641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (815, 4, N'Candover', N'Candover', N'CANDOVER', 1, 1, CAST(0x0000A2F30174966C AS DateTime), 1, CAST(0x0000A2F30174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (816, 5, N'Cannon Rocks', N'Cannon Rocks', N'CANNON_ROCKS', 1, 1, CAST(0x0000A2F30185112C AS DateTime), 1, CAST(0x0000A2F30185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (817, 5, N'Cape St. Francis', N'Cape St. Francis', N'CAPE_ST._FRANCIS', 1, 1, CAST(0x0000A2F4000A09EC AS DateTime), 1, CAST(0x0000A2F4000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (818, 9, N'Cape Town', N'Cape Town', N'CAPE_TOWN', 1, 1, CAST(0x0000A2F4001A84AC AS DateTime), 1, CAST(0x0000A2F4001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (819, 4, N'Cape Vidal', N'Cape Vidal', N'CAPE_VIDAL', 1, 1, CAST(0x0000A2F4002AFF6C AS DateTime), 1, CAST(0x0000A2F4002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (820, 1, N'Carletonville', N'Carletonville', N'CARLETONVILLE', 1, 1, CAST(0x0000A2F4003B7A2C AS DateTime), 1, CAST(0x0000A2F4003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (821, 2, N'Carlow', N'Carlow', N'CARLOW', 1, 1, CAST(0x0000A2F4004BF4EC AS DateTime), 1, CAST(0x0000A2F4004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (822, 7, N'Carlsonia', N'Carlsonia', N'CARLSONIA', 1, 1, CAST(0x0000A2F4005C6FAC AS DateTime), 1, CAST(0x0000A2F4005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (823, 8, N'Carlton', N'Carlton', N'CARLTON', 1, 1, CAST(0x0000A2F4006CEA6C AS DateTime), 1, CAST(0x0000A2F4006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (824, 8, N'Carnarvon', N'Carnarvon', N'CARNARVON', 1, 1, CAST(0x0000A2F4007D652C AS DateTime), 1, CAST(0x0000A2F4007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (825, 6, N'Carolina', N'Carolina', N'CAROLINA', 1, 1, CAST(0x0000A2F4008DDFEC AS DateTime), 1, CAST(0x0000A2F4008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (826, 4, N'Catalina Bay', N'Catalina Bay', N'CATALINA_BAY', 1, 1, CAST(0x0000A2F4009E5AAC AS DateTime), 1, CAST(0x0000A2F4009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (827, 8, N'Carolusberg', N'Carolusberg', N'CAROLUSBERG', 1, 1, CAST(0x0000A2F400AED56C AS DateTime), 1, CAST(0x0000A2F400AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (828, 5, N'Cathcart', N'Cathcart', N'CATHCART', 1, 1, CAST(0x0000A2F400BF502C AS DateTime), 1, CAST(0x0000A2F400BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (829, 4, N'Cato Ridge', N'Cato Ridge', N'CATO_RIDGE', 1, 1, CAST(0x0000A2F400CFCAEC AS DateTime), 1, CAST(0x0000A2F400CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (830, 4, N'Cedarville', N'Cedarville', N'CEDARVILLE', 1, 1, CAST(0x0000A2F400E045AC AS DateTime), 1, CAST(0x0000A2F400E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (831, 1, N'Centurion', N'Centurion', N'CENTURION', 1, 1, CAST(0x0000A2F400F0C06C AS DateTime), 1, CAST(0x0000A2F400F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (832, 9, N'Ceres', N'Ceres', N'CERES', 1, 1, CAST(0x0000A2F401013B2C AS DateTime), 1, CAST(0x0000A2F401013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (833, 5, N'Chalumna', N'Chalumna', N'CHALUMNA', 1, 1, CAST(0x0000A2F40111B5EC AS DateTime), 1, CAST(0x0000A2F40111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (834, 1, N'Chantelle', N'Chantelle', N'CHANTELLE', 1, 1, CAST(0x0000A2F4012230AC AS DateTime), 1, CAST(0x0000A2F4012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (835, 6, N'Charl Cilliers', N'Charl Cilliers', N'CHARL_CILLIERS', 1, 1, CAST(0x0000A2F40132AB6C AS DateTime), 1, CAST(0x0000A2F40132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (836, 4, N'Charlestown', N'Charlestown', N'CHARLESTOWN', 1, 1, CAST(0x0000A2F40143262C AS DateTime), 1, CAST(0x0000A2F40143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (837, 5, N'Chintsa', N'Chintsa', N'CHINTSA', 1, 1, CAST(0x0000A2F40153A0EC AS DateTime), 1, CAST(0x0000A2F40153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (838, 6, N'Chrissiesmeer', N'Chrissiesmeer', N'CHRISSIESMEER', 1, 1, CAST(0x0000A2F401641BAC AS DateTime), 1, CAST(0x0000A2F401641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (839, 7, N'Christiana', N'Christiana', N'CHRISTIANA', 1, 1, CAST(0x0000A2F40174966C AS DateTime), 1, CAST(0x0000A2F40174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (840, 2, N'Chuniespoort', N'Chuniespoort', N'CHUNIESPOORT', 1, 1, CAST(0x0000A2F40185112C AS DateTime), 1, CAST(0x0000A2F40185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (841, 9, N'Churchaven', N'Churchaven', N'CHURCHAVEN', 1, 1, CAST(0x0000A2F5000A09EC AS DateTime), 1, CAST(0x0000A2F5000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (842, 9, N'Citrusdal', N'Citrusdal', N'CITRUSDAL', 1, 1, CAST(0x0000A2F5001A84AC AS DateTime), 1, CAST(0x0000A2F5001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (843, 4, N'Clansthal', N'Clansthal', N'CLANSTHAL', 1, 1, CAST(0x0000A2F5002AFF6C AS DateTime), 1, CAST(0x0000A2F5002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (844, 9, N'Clanwilliam', N'Clanwilliam', N'CLANWILLIAM', 1, 1, CAST(0x0000A2F5003B7A2C AS DateTime), 1, CAST(0x0000A2F5003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (845, 9, N'Claremont', N'Claremont', N'CLAREMONT', 1, 1, CAST(0x0000A2F5004BF4EC AS DateTime), 1, CAST(0x0000A2F5004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (846, 3, N'Clarens', N'Clarens', N'CLARENS', 1, 1, CAST(0x0000A2F5005C6FAC AS DateTime), 1, CAST(0x0000A2F5005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (847, 5, N'Clarkebury', N'Clarkebury', N'CLARKEBURY', 1, 1, CAST(0x0000A2F5006CEA6C AS DateTime), 1, CAST(0x0000A2F5006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (848, 9, N'Clarkson', N'Clarkson', N'CLARKSON', 1, 1, CAST(0x0000A2F5007D652C AS DateTime), 1, CAST(0x0000A2F5007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (849, 4, N'Clermont', N'Clermont', N'CLERMONT', 1, 1, CAST(0x0000A2F5008DDFEC AS DateTime), 1, CAST(0x0000A2F5008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (850, 6, N'Clewer', N'Clewer', N'CLEWER', 1, 1, CAST(0x0000A2F5009E5AAC AS DateTime), 1, CAST(0x0000A2F5009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (851, 5, N'Clifford', N'Clifford', N'CLIFFORD', 1, 1, CAST(0x0000A2F500AED56C AS DateTime), 1, CAST(0x0000A2F500AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (852, 9, N'Clifton', N'Clifton', N'CLIFTON', 1, 1, CAST(0x0000A2F500BF502C AS DateTime), 1, CAST(0x0000A2F500BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (853, 3, N'Clocolan', N'Clocolan', N'CLOCOLAN', 1, 1, CAST(0x0000A2F500CFCAEC AS DateTime), 1, CAST(0x0000A2F500CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (854, 6, N'Coalville', N'Coalville', N'COALVILLE', 1, 1, CAST(0x0000A2F500E045AC AS DateTime), 1, CAST(0x0000A2F500E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (855, 5, N'Coega', N'Coega', N'COEGA', 1, 1, CAST(0x0000A2F500F0C06C AS DateTime), 1, CAST(0x0000A2F500F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (856, 8, N'Concordia', N'Concordia', N'CONCORDIA', 1, 1, CAST(0x0000A2F501013B2C AS DateTime), 1, CAST(0x0000A2F501013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (857, 5, N'Coffee Bay', N'Coffee Bay', N'COFFEE_BAY', 1, 1, CAST(0x0000A2F50111B5EC AS DateTime), 1, CAST(0x0000A2F50111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (858, 5, N'Cofimvaba', N'Cofimvaba', N'COFIMVABA', 1, 1, CAST(0x0000A2F5012230AC AS DateTime), 1, CAST(0x0000A2F5012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (859, 5, N'Coghlan', N'Coghlan', N'COGHLAN', 1, 1, CAST(0x0000A2F50132AB6C AS DateTime), 1, CAST(0x0000A2F50132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (860, 5, N'Colchester', N'Colchester', N'COLCHESTER', 1, 1, CAST(0x0000A2F50143262C AS DateTime), 1, CAST(0x0000A2F50143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (861, 5, N'Coleford', N'Coleford', N'COLEFORD', 1, 1, CAST(0x0000A2F50153A0EC AS DateTime), 1, CAST(0x0000A2F50153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (862, 8, N'Colesberg', N'Colesberg', N'COLESBERG', 1, 1, CAST(0x0000A2F501641BAC AS DateTime), 1, CAST(0x0000A2F501641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (863, 4, N'Colenso', N'Colenso', N'COLENSO', 1, 1, CAST(0x0000A2F50174966C AS DateTime), 1, CAST(0x0000A2F50174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (864, 7, N'Coligny', N'Coligny', N'COLIGNY', 1, 1, CAST(0x0000A2F50185112C AS DateTime), 1, CAST(0x0000A2F50185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (865, 8, N'Colston', N'Colston', N'COLSTON', 1, 1, CAST(0x0000A2F6000A09EC AS DateTime), 1, CAST(0x0000A2F6000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (866, 6, N'Commondale', N'Commondale', N'COMMONDALE', 1, 1, CAST(0x0000A2F6001A84AC AS DateTime), 1, CAST(0x0000A2F6001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (867, 9, N'Constantia', N'Constantia', N'CONSTANTIA', 1, 1, CAST(0x0000A2F6002AFF6C AS DateTime), 1, CAST(0x0000A2F6002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (868, 8, N'Copperton', N'Copperton', N'COPPERTON', 1, 1, CAST(0x0000A2F6003B7A2C AS DateTime), 1, CAST(0x0000A2F6003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (869, 3, N'Cornelia', N'Cornelia', N'CORNELIA', 1, 1, CAST(0x0000A2F6004BF4EC AS DateTime), 1, CAST(0x0000A2F6004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (870, 6, N'Cork', N'Cork', N'CORK', 1, 1, CAST(0x0000A2F6005C6FAC AS DateTime), 1, CAST(0x0000A2F6005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (871, 5, N'Cookhouse', N'Cookhouse', N'COOKHOUSE', 1, 1, CAST(0x0000A2F6006CEA6C AS DateTime), 1, CAST(0x0000A2F6006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (872, 5, N'Cradock', N'Cradock', N'CRADOCK', 1, 1, CAST(0x0000A2F6007D652C AS DateTime), 1, CAST(0x0000A2F6007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (873, 1, N'Cullinan', N'Cullinan', N'CULLINAN', 1, 1, CAST(0x0000A2F6008DDFEC AS DateTime), 1, CAST(0x0000A2F6008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (874, 8, N'Danielskuil', N'Danielskuil', N'DANIELSKUIL', 1, 1, CAST(0x0000A2F6009E5AAC AS DateTime), 1, CAST(0x0000A2F6009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (875, 4, N'Dannhauser', N'Dannhauser', N'DANNHAUSER', 1, 1, CAST(0x0000A2F600AED56C AS DateTime), 1, CAST(0x0000A2F600AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (876, 9, N'Darling', N'Darling', N'DARLING', 1, 1, CAST(0x0000A2F600BF502C AS DateTime), 1, CAST(0x0000A2F600BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (877, 4, N'Darnall', N'Darnall', N'DARNALL', 1, 1, CAST(0x0000A2F600CFCAEC AS DateTime), 1, CAST(0x0000A2F600CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (878, 1, N'Daveyton', N'Daveyton', N'DAVEYTON', 1, 1, CAST(0x0000A2F600E045AC AS DateTime), 1, CAST(0x0000A2F600E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (879, 8, N'De Aar', N'De Aar', N'DE_AAR', 1, 1, CAST(0x0000A2F600F0C06C AS DateTime), 1, CAST(0x0000A2F600F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (880, 3, N'Dealesville', N'Dealesville', N'DEALESVILLE', 1, 1, CAST(0x0000A2F601013B2C AS DateTime), 1, CAST(0x0000A2F601013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (881, 9, N'De Doorns', N'De Doorns', N'DE_DOORNS', 1, 1, CAST(0x0000A2F60111B5EC AS DateTime), 1, CAST(0x0000A2F60111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (882, 9, N'De Kelders', N'De Kelders', N'DE_KELDERS', 1, 1, CAST(0x0000A2F6012230AC AS DateTime), 1, CAST(0x0000A2F6012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (883, 7, N'Delareyville', N'Delareyville', N'DELAREYVILLE', 1, 1, CAST(0x0000A2F60132AB6C AS DateTime), 1, CAST(0x0000A2F60132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (884, 6, N'Delmas', N'Delmas', N'DELMAS', 1, 1, CAST(0x0000A2F60143262C AS DateTime), 1, CAST(0x0000A2F60143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (885, 8, N'Delportshoop', N'Delportshoop', N'DELPORTSHOOP', 1, 1, CAST(0x0000A2F60153A0EC AS DateTime), 1, CAST(0x0000A2F60153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (886, 3, N'Deneysville', N'Deneysville', N'DENEYSVILLE', 1, 1, CAST(0x0000A2F601641BAC AS DateTime), 1, CAST(0x0000A2F601641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (887, 7, N'Derby', N'Derby', N'DERBY', 1, 1, CAST(0x0000A2F60174966C AS DateTime), 1, CAST(0x0000A2F60174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (888, 9, N'De Rust', N'De Rust', N'DE_RUST', 1, 1, CAST(0x0000A2F60185112C AS DateTime), 1, CAST(0x0000A2F60185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (889, 5, N'Despatch', N'Despatch', N'DESPATCH', 1, 1, CAST(0x0000A2F7000A09EC AS DateTime), 1, CAST(0x0000A2F7000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (890, 1, N'Devon', N'Devon', N'DEVON', 1, 1, CAST(0x0000A2F7001A84AC AS DateTime), 1, CAST(0x0000A2F7001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (891, 3, N'Dewetsdorp', N'Dewetsdorp', N'DEWETSDORP', 1, 1, CAST(0x0000A2F7002AFF6C AS DateTime), 1, CAST(0x0000A2F7002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (892, 8, N'Dibeng', N'Dibeng', N'DIBENG', 1, 1, CAST(0x0000A2F7003B7A2C AS DateTime), 1, CAST(0x0000A2F7003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (893, 8, N'Dingleton', N'Dingleton', N'DINGLETON', 1, 1, CAST(0x0000A2F7004BF4EC AS DateTime), 1, CAST(0x0000A2F7004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (894, 5, N'Döhne', N'Döhne', N'DÖHNE', 1, 1, CAST(0x0000A2F7005C6FAC AS DateTime), 1, CAST(0x0000A2F7005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (895, 4, N'Doonside', N'Doonside', N'DOONSIDE', 1, 1, CAST(0x0000A2F7006CEA6C AS DateTime), 1, CAST(0x0000A2F7006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (896, 9, N'Doringbaai', N'Doringbaai', N'DORINGBAAI', 1, 1, CAST(0x0000A2F7007D652C AS DateTime), 1, CAST(0x0000A2F7007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (897, 5, N'Dordrecht', N'Dordrecht', N'DORDRECHT', 1, 1, CAST(0x0000A2F7008DDFEC AS DateTime), 1, CAST(0x0000A2F7008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (898, 8, N'Douglas', N'Douglas', N'DOUGLAS', 1, 1, CAST(0x0000A2F7009E5AAC AS DateTime), 1, CAST(0x0000A2F7009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (899, 4, N'Drummond', N'Drummond', N'DRUMMOND', 1, 1, CAST(0x0000A2F700AED56C AS DateTime), 1, CAST(0x0000A2F700AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (900, 1, N'Duduza', N'Duduza', N'DUDUZA', 1, 1, CAST(0x0000A2F700BF502C AS DateTime), 1, CAST(0x0000A2F700BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (901, 2, N'Duiwelskloof', N'Duiwelskloof', N'DUIWELSKLOOF', 1, 1, CAST(0x0000A2F700CFCAEC AS DateTime), 1, CAST(0x0000A2F700CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (902, 6, N'Dullstroom', N'Dullstroom', N'DULLSTROOM', 1, 1, CAST(0x0000A2F700E045AC AS DateTime), 1, CAST(0x0000A2F700E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (903, 4, N'Dundee', N'Dundee', N'DUNDEE', 1, 1, CAST(0x0000A2F700F0C06C AS DateTime), 1, CAST(0x0000A2F700F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (904, 4, N'Durban', N'Durban', N'DURBAN', 1, 1, CAST(0x0000A2F701013B2C AS DateTime), 1, CAST(0x0000A2F701013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (905, 9, N'Dysselsdorp', N'Dysselsdorp', N'DYSSELSDORP', 1, 1, CAST(0x0000A2F70111B5EC AS DateTime), 1, CAST(0x0000A2F70111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (906, 3, N'Edenburg', N'Edenburg', N'EDENBURG', 1, 1, CAST(0x0000A2F7012230AC AS DateTime), 1, CAST(0x0000A2F7012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (907, 1, N'Edenvale', N'Edenvale', N'EDENVALE', 1, 1, CAST(0x0000A2F70132AB6C AS DateTime), 1, CAST(0x0000A2F70132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (908, 3, N'Edenville', N'Edenville', N'EDENVILLE', 1, 1, CAST(0x0000A2F70143262C AS DateTime), 1, CAST(0x0000A2F70143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (909, 9, N'Eendekuil', N'Eendekuil', N'EENDEKUIL', 1, 1, CAST(0x0000A2F70153A0EC AS DateTime), 1, CAST(0x0000A2F70153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (910, 5, N'East London', N'East London', N'EAST_LONDON', 1, 1, CAST(0x0000A2F701641BAC AS DateTime), 1, CAST(0x0000A2F701641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (911, 4, N'ekuPhakameni', N'ekuPhakameni', N'EKUPHAKAMENI', 1, 1, CAST(0x0000A2F70174966C AS DateTime), 1, CAST(0x0000A2F70174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (912, 9, N'Elandsbaai', N'Elandsbaai', N'ELANDSBAAI', 1, 1, CAST(0x0000A2F70185112C AS DateTime), 1, CAST(0x0000A2F70185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (913, 4, N'Elandslaagte', N'Elandslaagte', N'ELANDSLAAGTE', 1, 1, CAST(0x0000A2F8000A09EC AS DateTime), 1, CAST(0x0000A2F8000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (914, 9, N'Elim', N'Elim', N'ELIM', 1, 1, CAST(0x0000A2F8001A84AC AS DateTime), 1, CAST(0x0000A2F8001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (915, 9, N'Elgin', N'Elgin', N'ELGIN', 1, 1, CAST(0x0000A2F8002AFF6C AS DateTime), 1, CAST(0x0000A2F8002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (916, 5, N'Elliot', N'Elliot', N'ELLIOT', 1, 1, CAST(0x0000A2F8003B7A2C AS DateTime), 1, CAST(0x0000A2F8003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (917, 4, N'Empangeni', N'Empangeni', N'EMPANGENI', 1, 1, CAST(0x0000A2F8004BF4EC AS DateTime), 1, CAST(0x0000A2F8004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (918, 6, N'Ermelo', N'Ermelo', N'ERMELO', 1, 1, CAST(0x0000A2F8005C6FAC AS DateTime), 1, CAST(0x0000A2F8005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (919, 4, N'Eshowe', N'Eshowe', N'ESHOWE', 1, 1, CAST(0x0000A2F8006CEA6C AS DateTime), 1, CAST(0x0000A2F8006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (920, 4, N'Estcourt', N'Estcourt', N'ESTCOURT', 1, 1, CAST(0x0000A2F8007D652C AS DateTime), 1, CAST(0x0000A2F8007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (921, 1, N'Evaton', N'Evaton', N'EVATON', 1, 1, CAST(0x0000A2F8008DDFEC AS DateTime), 1, CAST(0x0000A2F8008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (922, 3, N'Excelsior', N'Excelsior', N'EXCELSIOR', 1, 1, CAST(0x0000A2F8009E5AAC AS DateTime), 1, CAST(0x0000A2F8009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (923, 3, N'Fauresmith', N'Fauresmith', N'FAURESMITH', 1, 1, CAST(0x0000A2F800AED56C AS DateTime), 1, CAST(0x0000A2F800AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (924, 3, N'Ficksburg', N'Ficksburg', N'FICKSBURG', 1, 1, CAST(0x0000A2F800BF502C AS DateTime), 1, CAST(0x0000A2F800BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (925, 9, N'Fisherhaven', N'Fisherhaven', N'FISHERHAVEN', 1, 1, CAST(0x0000A2F800CFCAEC AS DateTime), 1, CAST(0x0000A2F800CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (926, 5, N'Flagstaff', N'Flagstaff', N'FLAGSTAFF', 1, 1, CAST(0x0000A2F800E045AC AS DateTime), 1, CAST(0x0000A2F800E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (927, 5, N'Fort Beaufort', N'Fort Beaufort', N'FORT_BEAUFORT', 1, 1, CAST(0x0000A2F800F0C06C AS DateTime), 1, CAST(0x0000A2F800F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (928, 3, N'Fouriesburg', N'Fouriesburg', N'FOURIESBURG', 1, 1, CAST(0x0000A2F801013B2C AS DateTime), 1, CAST(0x0000A2F801013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (929, 3, N'Frankfort', N'Frankfort', N'FRANKFORT', 1, 1, CAST(0x0000A2F80111B5EC AS DateTime), 1, CAST(0x0000A2F80111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (930, 4, N'Franklin', N'Franklin', N'FRANKLIN', 1, 1, CAST(0x0000A2F8012230AC AS DateTime), 1, CAST(0x0000A2F8012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (931, 9, N'Franskraal', N'Franskraal', N'FRANSKRAAL', 1, 1, CAST(0x0000A2F80132AB6C AS DateTime), 1, CAST(0x0000A2F80132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (932, 9, N'Franschhoek', N'Franschhoek', N'FRANSCHHOEK', 1, 1, CAST(0x0000A2F80143262C AS DateTime), 1, CAST(0x0000A2F80143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (933, 8, N'Fraserburg', N'Fraserburg', N'FRASERBURG', 1, 1, CAST(0x0000A2F80153A0EC AS DateTime), 1, CAST(0x0000A2F80153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (934, 9, N'Gansbaai', N'Gansbaai', N'GANSBAAI', 1, 1, CAST(0x0000A2F801641BAC AS DateTime), 1, CAST(0x0000A2F801641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (935, 7, N'Ganyesa', N'Ganyesa', N'GANYESA', 1, 1, CAST(0x0000A2F80174966C AS DateTime), 1, CAST(0x0000A2F80174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (936, 7, N'Ga-Rankuwa', N'Ga-Rankuwa', N'GA-RANKUWA', 1, 1, CAST(0x0000A2F80185112C AS DateTime), 1, CAST(0x0000A2F80185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (937, 5, N'Gcuwa', N'Gcuwa', N'GCUWA', 1, 1, CAST(0x0000A2F9000A09EC AS DateTime), 1, CAST(0x0000A2F9000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (938, 9, N'Genadendal', N'Genadendal', N'GENADENDAL', 1, 1, CAST(0x0000A2F9001A84AC AS DateTime), 1, CAST(0x0000A2F9001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (939, 9, N'George', N'George', N'GEORGE', 1, 1, CAST(0x0000A2F9002AFF6C AS DateTime), 1, CAST(0x0000A2F9002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (940, 1, N'Germiston', N'Germiston', N'GERMISTON', 1, 1, CAST(0x0000A2F9003B7A2C AS DateTime), 1, CAST(0x0000A2F9003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (941, 2, N'Giyani', N'Giyani', N'GIYANI', 1, 1, CAST(0x0000A2F9004BF4EC AS DateTime), 1, CAST(0x0000A2F9004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (942, 4, N'Glencoe', N'Glencoe', N'GLENCOE', 1, 1, CAST(0x0000A2F9005C6FAC AS DateTime), 1, CAST(0x0000A2F9005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (943, 5, N'Gonubie', N'Gonubie', N'GONUBIE', 1, 1, CAST(0x0000A2F9006CEA6C AS DateTime), 1, CAST(0x0000A2F9006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (944, 9, N'Gouda', N'Gouda', N'GOUDA', 1, 1, CAST(0x0000A2F9007D652C AS DateTime), 1, CAST(0x0000A2F9007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (945, 5, N'Graaff-Reinet', N'Graaff-Reinet', N'GRAAFF-REINET', 1, 1, CAST(0x0000A2F9008DDFEC AS DateTime), 1, CAST(0x0000A2F9008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (946, 9, N'Graafwater', N'Graafwater', N'GRAAFWATER', 1, 1, CAST(0x0000A2F9009E5AAC AS DateTime), 1, CAST(0x0000A2F9009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (947, 9, N'Grabouw', N'Grabouw', N'GRABOUW', 1, 1, CAST(0x0000A2F900AED56C AS DateTime), 1, CAST(0x0000A2F900AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (948, 5, N'Grahamstown', N'Grahamstown', N'GRAHAMSTOWN', 1, 1, CAST(0x0000A2F900BF502C AS DateTime), 1, CAST(0x0000A2F900BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (949, 6, N'Graskop', N'Graskop', N'GRASKOP', 1, 1, CAST(0x0000A2F900CFCAEC AS DateTime), 1, CAST(0x0000A2F900CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (950, 2, N'Gravelotte, 2', N'Gravelotte, 2', N'GRAVELOTTE,_2', 1, 1, CAST(0x0000A2F900E045AC AS DateTime), 1, CAST(0x0000A2F900E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (951, 6, N'Greylingstad', N'Greylingstad', N'GREYLINGSTAD', 1, 1, CAST(0x0000A2F900F0C06C AS DateTime), 1, CAST(0x0000A2F900F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (952, 9, N'Greyton', N'Greyton', N'GREYTON', 1, 1, CAST(0x0000A2F901013B2C AS DateTime), 1, CAST(0x0000A2F901013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (953, 4, N'Greytown', N'Greytown', N'GREYTOWN', 1, 1, CAST(0x0000A2F90111B5EC AS DateTime), 1, CAST(0x0000A2F90111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (954, 8, N'Griekwastad', N'Griekwastad', N'GRIEKWASTAD', 1, 1, CAST(0x0000A2F9012230AC AS DateTime), 1, CAST(0x0000A2F9012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (955, 2, N'Groblersdal', N'Groblersdal', N'GROBLERSDAL', 1, 1, CAST(0x0000A2F90132AB6C AS DateTime), 1, CAST(0x0000A2F90132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (956, 8, N'Groblershoop', N'Groblershoop', N'GROBLERSHOOP', 1, 1, CAST(0x0000A2F90143262C AS DateTime), 1, CAST(0x0000A2F90143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (957, 7, N'Groot Marico', N'Groot Marico', N'GROOT_MARICO', 1, 1, CAST(0x0000A2F90153A0EC AS DateTime), 1, CAST(0x0000A2F90153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (958, 2, N'Haenertsburg', N'Haenertsburg', N'HAENERTSBURG', 1, 1, CAST(0x0000A2F901641BAC AS DateTime), 1, CAST(0x0000A2F901641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (959, 1, N'Hammanskraal', N'Hammanskraal', N'HAMMANSKRAAL', 1, 1, CAST(0x0000A2F90174966C AS DateTime), 1, CAST(0x0000A2F90174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (960, 5, N'Hankey', N'Hankey', N'HANKEY', 1, 1, CAST(0x0000A2F90185112C AS DateTime), 1, CAST(0x0000A2F90185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (961, 3, N'Harrismith', N'Harrismith', N'HARRISMITH', 1, 1, CAST(0x0000A2FA000A09EC AS DateTime), 1, CAST(0x0000A2FA000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (962, 7, N'Hartbeespoort', N'Hartbeespoort', N'HARTBEESPOORT', 1, 1, CAST(0x0000A2FA001A84AC AS DateTime), 1, CAST(0x0000A2FA001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (963, 4, N'Hattingspruit', N'Hattingspruit', N'HATTINGSPRUIT', 1, 1, CAST(0x0000A2FA002AFF6C AS DateTime), 1, CAST(0x0000A2FA002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (964, 6, N'Hazyview', N'Hazyview', N'HAZYVIEW', 1, 1, CAST(0x0000A2FA003B7A2C AS DateTime), 1, CAST(0x0000A2FA003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (965, 6, N'Hectorspruit', N'Hectorspruit', N'HECTORSPRUIT', 1, 1, CAST(0x0000A2FA004BF4EC AS DateTime), 1, CAST(0x0000A2FA004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (966, 1, N'Heidelberg', N'Heidelberg', N'HEIDELBERG', 1, 1, CAST(0x0000A2FA005C6FAC AS DateTime), 1, CAST(0x0000A2FA005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (967, 9, N'Heidelberg', N'Heidelberg', N'HEIDELBERG', 1, 1, CAST(0x0000A2FA006CEA6C AS DateTime), 1, CAST(0x0000A2FA006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (968, 3, N'Heilbron', N'Heilbron', N'HEILBRON', 1, 1, CAST(0x0000A2FA007D652C AS DateTime), 1, CAST(0x0000A2FA007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (969, 1, N'Henley on Klip', N'Henley on Klip', N'HENLEY_ON_KLIP', 1, 1, CAST(0x0000A2FA008DDFEC AS DateTime), 1, CAST(0x0000A2FA008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (970, 3, N'Hennenman', N'Hennenman', N'HENNENMAN', 1, 1, CAST(0x0000A2FA009E5AAC AS DateTime), 1, CAST(0x0000A2FA009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (971, 9, N'Hermanus', N'Hermanus', N'HERMANUS', 1, 1, CAST(0x0000A2FA00AED56C AS DateTime), 1, CAST(0x0000A2FA00AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (972, 3, N'Hertzogville', N'Hertzogville', N'HERTZOGVILLE', 1, 1, CAST(0x0000A2FA00BF502C AS DateTime), 1, CAST(0x0000A2FA00BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (973, 4, N'Hibberdene', N'Hibberdene', N'HIBBERDENE', 1, 1, CAST(0x0000A2FA00CFCAEC AS DateTime), 1, CAST(0x0000A2FA00CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (974, 4, N'Hillcrest', N'Hillcrest', N'HILLCREST', 1, 1, CAST(0x0000A2FA00E045AC AS DateTime), 1, CAST(0x0000A2FA00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (975, 4, N'Hilton', N'Hilton', N'HILTON', 1, 1, CAST(0x0000A2FA00F0C06C AS DateTime), 1, CAST(0x0000A2FA00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (976, 4, N'Himeville', N'Himeville', N'HIMEVILLE', 1, 1, CAST(0x0000A2FA01013B2C AS DateTime), 1, CAST(0x0000A2FA01013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (977, 4, N'Hluhluwe', N'Hluhluwe', N'HLUHLUWE', 1, 1, CAST(0x0000A2FA0111B5EC AS DateTime), 1, CAST(0x0000A2FA0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (978, 3, N'Hobhouse', N'Hobhouse', N'HOBHOUSE', 1, 1, CAST(0x0000A2FA012230AC AS DateTime), 1, CAST(0x0000A2FA012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (979, 2, N'Hoedspruit', N'Hoedspruit', N'HOEDSPRUIT', 1, 1, CAST(0x0000A2FA0132AB6C AS DateTime), 1, CAST(0x0000A2FA0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (980, 5, N'Hofmeyr', N'Hofmeyr', N'HOFMEYR', 1, 1, CAST(0x0000A2FA0143262C AS DateTime), 1, CAST(0x0000A2FA0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (981, 3, N'Hoopstad', N'Hoopstad', N'HOOPSTAD', 1, 1, CAST(0x0000A2FA0153A0EC AS DateTime), 1, CAST(0x0000A2FA0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (982, 9, N'Hopefield', N'Hopefield', N'HOPEFIELD', 1, 1, CAST(0x0000A2FA01641BAC AS DateTime), 1, CAST(0x0000A2FA01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (983, 8, N'Hopetown', N'Hopetown', N'HOPETOWN', 1, 1, CAST(0x0000A2FA0174966C AS DateTime), 1, CAST(0x0000A2FA0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (984, 4, N'Howick', N'Howick', N'HOWICK', 1, 1, CAST(0x0000A2FA0185112C AS DateTime), 1, CAST(0x0000A2FA0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (985, 5, N'Humansdorp', N'Humansdorp', N'HUMANSDORP', 1, 1, CAST(0x0000A2FB000A09EC AS DateTime), 1, CAST(0x0000A2FB000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (986, 5, N'Idutywa', N'Idutywa', N'IDUTYWA', 1, 1, CAST(0x0000A2FB001A84AC AS DateTime), 1, CAST(0x0000A2FB001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (987, 4, N'Ifafa Beach', N'Ifafa Beach', N'IFAFA_BEACH', 1, 1, CAST(0x0000A2FB002AFF6C AS DateTime), 1, CAST(0x0000A2FB002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (988, 4, N'Illovo Beach', N'Illovo Beach', N'ILLOVO_BEACH', 1, 1, CAST(0x0000A2FB003B7A2C AS DateTime), 1, CAST(0x0000A2FB003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (989, 4, N'Impendle', N'Impendle', N'IMPENDLE', 1, 1, CAST(0x0000A2FB004BF4EC AS DateTime), 1, CAST(0x0000A2FB004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (990, 4, N'Inanda', N'Inanda', N'INANDA', 1, 1, CAST(0x0000A2FB005C6FAC AS DateTime), 1, CAST(0x0000A2FB005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (991, 4, N'Ingwavuma', N'Ingwavuma', N'INGWAVUMA', 1, 1, CAST(0x0000A2FB006CEA6C AS DateTime), 1, CAST(0x0000A2FB006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (992, 1, N'Irene', N'Irene', N'IRENE', 1, 1, CAST(0x0000A2FB007D652C AS DateTime), 1, CAST(0x0000A2FB007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (993, 1, N'Isando', N'Isando', N'ISANDO', 1, 1, CAST(0x0000A2FB008DDFEC AS DateTime), 1, CAST(0x0000A2FB008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (994, 4, N'Isipingo Beach', N'Isipingo Beach', N'ISIPINGO_BEACH', 1, 1, CAST(0x0000A2FB009E5AAC AS DateTime), 1, CAST(0x0000A2FB009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (995, 4, N'Ixopo', N'Ixopo', N'IXOPO', 1, 1, CAST(0x0000A2FB00AED56C AS DateTime), 1, CAST(0x0000A2FB00AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (996, 5, N'Jansenville', N'Jansenville', N'JANSENVILLE', 1, 1, CAST(0x0000A2FB00BF502C AS DateTime), 1, CAST(0x0000A2FB00BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (997, 3, N'Jacobsdal', N'Jacobsdal', N'JACOBSDAL', 1, 1, CAST(0x0000A2FB00CFCAEC AS DateTime), 1, CAST(0x0000A2FB00CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (998, 3, N'Jagersfontein', N'Jagersfontein', N'JAGERSFONTEIN', 1, 1, CAST(0x0000A2FB00E045AC AS DateTime), 1, CAST(0x0000A2FB00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (999, 5, N'Jeffreys Bay', N'Jeffreys Bay', N'JEFFREYS_BAY', 1, 1, CAST(0x0000A2FB00F0C06C AS DateTime), 1, CAST(0x0000A2FB00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1001, 6, N'Kaapmuiden', N'Kaapmuiden', N'KAAPMUIDEN', 1, 1, CAST(0x0000A2FB0111B5EC AS DateTime), 1, CAST(0x0000A2FB0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1002, 4, N'Karridene', N'Karridene', N'KARRIDENE', 1, 1, CAST(0x0000A2FB012230AC AS DateTime), 1, CAST(0x0000A2FB012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1003, 1, N'Katlehong', N'Katlehong', N'KATLEHONG', 1, 1, CAST(0x0000A2FB0132AB6C AS DateTime), 1, CAST(0x0000A2FB0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1004, 1, N'Kempton Park', N'Kempton Park', N'KEMPTON_PARK', 1, 1, CAST(0x0000A2FB0143262C AS DateTime), 1, CAST(0x0000A2FB0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1005, 5, N'Kenton-on-Sea', N'Kenton-on-Sea', N'KENTON-ON-SEA', 1, 1, CAST(0x0000A2FB0153A0EC AS DateTime), 1, CAST(0x0000A2FB0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1006, 3, N'Kestell', N'Kestell', N'KESTELL', 1, 1, CAST(0x0000A2FB01641BAC AS DateTime), 1, CAST(0x0000A2FB01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1007, 9, N'Keurboomstrand', N'Keurboomstrand', N'KEURBOOMSTRAND', 1, 1, CAST(0x0000A2FB0174966C AS DateTime), 1, CAST(0x0000A2FB0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1008, 3, N'Kgotsong', N'Kgotsong', N'KGOTSONG', 1, 1, CAST(0x0000A2FB0185112C AS DateTime), 1, CAST(0x0000A2FB0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1009, 9, N'Khayelitsha', N'Khayelitsha', N'KHAYELITSHA', 1, 1, CAST(0x0000A2FC000A09EC AS DateTime), 1, CAST(0x0000A2FC000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1010, 8, N'Kimberley', N'Kimberley', N'KIMBERLEY', 1, 1, CAST(0x0000A2FC001A84AC AS DateTime), 1, CAST(0x0000A2FC001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1011, 4, N'Kingsburgh', N'Kingsburgh', N'KINGSBURGH', 1, 1, CAST(0x0000A2FC002AFF6C AS DateTime), 1, CAST(0x0000A2FC002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1012, 5, N'King William''s Town', N'King William''s Town', N'KING_WILLIAM''S_TOWN', 1, 1, CAST(0x0000A2FC003B7A2C AS DateTime), 1, CAST(0x0000A2FC003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1013, 6, N'Kinross', N'Kinross', N'KINROSS', 1, 1, CAST(0x0000A2FC004BF4EC AS DateTime), 1, CAST(0x0000A2FC004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1014, 5, N'Kirkwood', N'Kirkwood', N'KIRKWOOD', 1, 1, CAST(0x0000A2FC005C6FAC AS DateTime), 1, CAST(0x0000A2FC005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1015, 7, N'Klerksdorp', N'Klerksdorp', N'KLERKSDORP', 1, 1, CAST(0x0000A2FC006CEA6C AS DateTime), 1, CAST(0x0000A2FC006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1016, 4, N'Kloof', N'Kloof', N'KLOOF', 1, 1, CAST(0x0000A2FC007D652C AS DateTime), 1, CAST(0x0000A2FC007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1017, 9, N'Knysna', N'Knysna', N'KNYSNA', 1, 1, CAST(0x0000A2FC008DDFEC AS DateTime), 1, CAST(0x0000A2FC008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1018, 3, N'Koffiefontein', N'Koffiefontein', N'KOFFIEFONTEIN', 1, 1, CAST(0x0000A2FC009E5AAC AS DateTime), 1, CAST(0x0000A2FC009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1019, 4, N'Kokstad', N'Kokstad', N'KOKSTAD', 1, 1, CAST(0x0000A2FC00AED56C AS DateTime), 1, CAST(0x0000A2FC00AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1020, 6, N'Komatipoort', N'Komatipoort', N'KOMATIPOORT', 1, 1, CAST(0x0000A2FC00BF502C AS DateTime), 1, CAST(0x0000A2FC00BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1021, 3, N'Koppies', N'Koppies', N'KOPPIES', 1, 1, CAST(0x0000A2FC00CFCAEC AS DateTime), 1, CAST(0x0000A2FC00CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1022, 1, N'Kromdraai', N'Kromdraai', N'KROMDRAAI', 1, 1, CAST(0x0000A2FC00E045AC AS DateTime), 1, CAST(0x0000A2FC00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1023, 3, N'Kroonstad', N'Kroonstad', N'KROONSTAD', 1, 1, CAST(0x0000A2FC00F0C06C AS DateTime), 1, CAST(0x0000A2FC00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1024, 6, N'Kriel', N'Kriel', N'KRIEL', 1, 1, CAST(0x0000A2FC01013B2C AS DateTime), 1, CAST(0x0000A2FC01013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1025, 1, N'Krugersdorp', N'Krugersdorp', N'KRUGERSDORP', 1, 1, CAST(0x0000A2FC0111B5EC AS DateTime), 1, CAST(0x0000A2FC0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1026, 4, N'KwaDukuza', N'KwaDukuza', N'KWADUKUZA', 1, 1, CAST(0x0000A2FC012230AC AS DateTime), 1, CAST(0x0000A2FC012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1027, 4, N'KwaMashu', N'KwaMashu', N'KWAMASHU', 1, 1, CAST(0x0000A2FC0132AB6C AS DateTime), 1, CAST(0x0000A2FC0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1028, 6, N'KwaMhlanga', N'KwaMhlanga', N'KWAMHLANGA', 1, 1, CAST(0x0000A2FC0143262C AS DateTime), 1, CAST(0x0000A2FC0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1029, 1, N'KwaThema', N'KwaThema', N'KWATHEMA', 1, 1, CAST(0x0000A2FC0153A0EC AS DateTime), 1, CAST(0x0000A2FC0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1030, 3, N'Ladybrand', N'Ladybrand', N'LADYBRAND', 1, 1, CAST(0x0000A2FC01641BAC AS DateTime), 1, CAST(0x0000A2FC01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1031, 5, N'Lady Frere', N'Lady Frere', N'LADY_FRERE', 1, 1, CAST(0x0000A2FC0174966C AS DateTime), 1, CAST(0x0000A2FC0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1032, 5, N'Lady Grey', N'Lady Grey', N'LADY_GREY', 1, 1, CAST(0x0000A2FC0185112C AS DateTime), 1, CAST(0x0000A2FC0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1033, 4, N'Ladysmith', N'Ladysmith', N'LADYSMITH', 1, 1, CAST(0x0000A2FD000A09EC AS DateTime), 1, CAST(0x0000A2FD000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1034, 9, N'Laingsburg', N'Laingsburg', N'LAINGSBURG', 1, 1, CAST(0x0000A2FD001A84AC AS DateTime), 1, CAST(0x0000A2FD001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1035, 4, N'La Lucia', N'La Lucia', N'LA_LUCIA', 1, 1, CAST(0x0000A2FD002AFF6C AS DateTime), 1, CAST(0x0000A2FD002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1036, 4, N'La Mercy', N'La Mercy', N'LA_MERCY', 1, 1, CAST(0x0000A2FD003B7A2C AS DateTime), 1, CAST(0x0000A2FD003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1037, 1, N'Lenasia', N'Lenasia', N'LENASIA', 1, 1, CAST(0x0000A2FD004BF4EC AS DateTime), 1, CAST(0x0000A2FD004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1038, 2, N'Lephalale', N'Lephalale', N'LEPHALALE', 1, 1, CAST(0x0000A2FD005C6FAC AS DateTime), 1, CAST(0x0000A2FD005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1039, 7, N'Lichtenburg', N'Lichtenburg', N'LICHTENBURG', 1, 1, CAST(0x0000A2FD006CEA6C AS DateTime), 1, CAST(0x0000A2FD006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1040, 3, N'Lindley', N'Lindley', N'LINDLEY', 1, 1, CAST(0x0000A2FD007D652C AS DateTime), 1, CAST(0x0000A2FD007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1041, 1, N'Lyttelton', N'Lyttelton', N'LYTTELTON', 1, 1, CAST(0x0000A2FD008DDFEC AS DateTime), 1, CAST(0x0000A2FD008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1042, 6, N'Loopspruit', N'Loopspruit', N'LOOPSPRUIT', 1, 1, CAST(0x0000A2FD009E5AAC AS DateTime), 1, CAST(0x0000A2FD009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1043, 2, N'Louis Trichardt', N'Louis Trichardt', N'LOUIS_TRICHARDT', 1, 1, CAST(0x0000A2FD00AED56C AS DateTime), 1, CAST(0x0000A2FD00AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1044, 4, N'Louwsburg', N'Louwsburg', N'LOUWSBURG', 1, 1, CAST(0x0000A2FD00BF502C AS DateTime), 1, CAST(0x0000A2FD00BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1045, 3, N'Luckhoff', N'Luckhoff', N'LUCKHOFF', 1, 1, CAST(0x0000A2FD00CFCAEC AS DateTime), 1, CAST(0x0000A2FD00CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1046, 6, N'Lydenburg', N'Lydenburg', N'LYDENBURG', 1, 1, CAST(0x0000A2FD00E045AC AS DateTime), 1, CAST(0x0000A2FD00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1047, 6, N'Machadodorp', N'Machadodorp', N'MACHADODORP', 1, 1, CAST(0x0000A2FD00F0C06C AS DateTime), 1, CAST(0x0000A2FD00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1048, 5, N'Maclear', N'Maclear', N'MACLEAR', 1, 1, CAST(0x0000A2FD01013B2C AS DateTime), 1, CAST(0x0000A2FD01013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1049, 5, N'Madadeni', N'Madadeni', N'MADADENI', 1, 1, CAST(0x0000A2FD0111B5EC AS DateTime), 1, CAST(0x0000A2FD0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1050, 7, N'Mafikeng', N'Mafikeng', N'MAFIKENG', 1, 1, CAST(0x0000A2FD012230AC AS DateTime), 1, CAST(0x0000A2FD012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1051, 1, N'Magaliesburg', N'Magaliesburg', N'MAGALIESBURG', 1, 1, CAST(0x0000A2FD0132AB6C AS DateTime), 1, CAST(0x0000A2FD0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1052, 4, N'Mahlabatini', N'Mahlabatini', N'MAHLABATINI', 1, 1, CAST(0x0000A2FD0143262C AS DateTime), 1, CAST(0x0000A2FD0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1053, 3, N'Makeleketla', N'Makeleketla', N'MAKELEKETLA', 1, 1, CAST(0x0000A2FD0153A0EC AS DateTime), 1, CAST(0x0000A2FD0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1054, 6, N'Malelane', N'Malelane', N'MALELANE', 1, 1, CAST(0x0000A2FD01641BAC AS DateTime), 1, CAST(0x0000A2FD01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1055, 1, N'Mamelodi', N'Mamelodi', N'MAMELODI', 1, 1, CAST(0x0000A2FD0174966C AS DateTime), 1, CAST(0x0000A2FD0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1056, 5, N'Mandini', N'Mandini', N'MANDINI', 1, 1, CAST(0x0000A2FD0185112C AS DateTime), 1, CAST(0x0000A2FD0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1057, 2, N'Marble Hall', N'Marble Hall', N'MARBLE_HALL', 1, 1, CAST(0x0000A2FE000A09EC AS DateTime), 1, CAST(0x0000A2FE000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1058, 4, N'Margate', N'Margate', N'MARGATE', 1, 1, CAST(0x0000A2FE001A84AC AS DateTime), 1, CAST(0x0000A2FE001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1059, 3, N'Marquard', N'Marquard', N'MARQUARD', 1, 1, CAST(0x0000A2FE002AFF6C AS DateTime), 1, CAST(0x0000A2FE002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1060, 5, N'Matatiele', N'Matatiele', N'MATATIELE', 1, 1, CAST(0x0000A2FE003B7A2C AS DateTime), 1, CAST(0x0000A2FE003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1061, 9, N'Matjiesfontein', N'Matjiesfontein', N'MATJIESFONTEIN', 1, 1, CAST(0x0000A2FE004BF4EC AS DateTime), 1, CAST(0x0000A2FE004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1062, 4, N'Melmoth', N'Melmoth', N'MELMOTH', 1, 1, CAST(0x0000A2FE005C6FAC AS DateTime), 1, CAST(0x0000A2FE005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1063, 3, N'Memel', N'Memel', N'MEMEL', 1, 1, CAST(0x0000A2FE006CEA6C AS DateTime), 1, CAST(0x0000A2FE006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1064, 4, N'Merrivale', N'Merrivale', N'MERRIVALE', 1, 1, CAST(0x0000A2FE007D652C AS DateTime), 1, CAST(0x0000A2FE007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1065, 1, N'Meyerton', N'Meyerton', N'MEYERTON', 1, 1, CAST(0x0000A2FE008DDFEC AS DateTime), 1, CAST(0x0000A2FE008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1066, 5, N'Middelburg', N'Middelburg', N'MIDDELBURG', 1, 1, CAST(0x0000A2FE009E5AAC AS DateTime), 1, CAST(0x0000A2FE009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1067, 6, N'Middelburg', N'Middelburg', N'MIDDELBURG', 1, 1, CAST(0x0000A2FE00AED56C AS DateTime), 1, CAST(0x0000A2FE00AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1068, 1, N'Midrand', N'Midrand', N'MIDRAND', 1, 1, CAST(0x0000A2FE00BF502C AS DateTime), 1, CAST(0x0000A2FE00BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1069, 4, N'Mkuze', N'Mkuze', N'MKUZE', 1, 1, CAST(0x0000A2FE00CFCAEC AS DateTime), 1, CAST(0x0000A2FE00CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1070, 7, N'Mmabatho', N'Mmabatho', N'MMABATHO', 1, 1, CAST(0x0000A2FE00E045AC AS DateTime), 1, CAST(0x0000A2FE00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1071, 8, N'Modder River', N'Modder River', N'MODDER_RIVER', 1, 1, CAST(0x0000A2FE00F0C06C AS DateTime), 1, CAST(0x0000A2FE00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1072, 2, N'Modimolle', N'Modimolle', N'MODIMOLLE', 1, 1, CAST(0x0000A2FE01013B2C AS DateTime), 1, CAST(0x0000A2FE01013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1073, 2, N'Mokopane', N'Mokopane', N'MOKOPANE', 1, 1, CAST(0x0000A2FE0111B5EC AS DateTime), 1, CAST(0x0000A2FE0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1074, 5, N'Molteno', N'Molteno', N'MOLTENO', 1, 1, CAST(0x0000A2FE012230AC AS DateTime), 1, CAST(0x0000A2FE012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1075, 4, N'Mooirivier', N'Mooirivier', N'MOOIRIVIER', 1, 1, CAST(0x0000A2FE0132AB6C AS DateTime), 1, CAST(0x0000A2FE0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1076, 6, N'Morgenzon', N'Morgenzon', N'MORGENZON', 1, 1, CAST(0x0000A2FE0143262C AS DateTime), 1, CAST(0x0000A2FE0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1077, 4, N'Mount Edgecombe', N'Mount Edgecombe', N'MOUNT_EDGECOMBE', 1, 1, CAST(0x0000A2FE0153A0EC AS DateTime), 1, CAST(0x0000A2FE0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1078, 5, N'Mount Fletcher', N'Mount Fletcher', N'MOUNT_FLETCHER', 1, 1, CAST(0x0000A2FE01641BAC AS DateTime), 1, CAST(0x0000A2FE01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1079, 9, N'Mossel Bay', N'Mossel Bay', N'MOSSEL_BAY', 1, 1, CAST(0x0000A2FE0174966C AS DateTime), 1, CAST(0x0000A2FE0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1080, 5, N'Mthatha', N'Mthatha', N'MTHATHA', 1, 1, CAST(0x0000A2FE0185112C AS DateTime), 1, CAST(0x0000A2FE0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1081, 4, N'Mtubatuba', N'Mtubatuba', N'MTUBATUBA', 1, 1, CAST(0x0000A2FF000A09EC AS DateTime), 1, CAST(0x0000A2FF000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1082, 4, N'Mtunzini', N'Mtunzini', N'MTUNZINI', 1, 1, CAST(0x0000A2FF001A84AC AS DateTime), 1, CAST(0x0000A2FF001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1083, 6, N'Muden', N'Muden', N'MUDEN', 1, 1, CAST(0x0000A2FF002AFF6C AS DateTime), 1, CAST(0x0000A2FF002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1084, 1, N'Muldersdrift', N'Muldersdrift', N'MULDERSDRIFT', 1, 1, CAST(0x0000A2FF003B7A2C AS DateTime), 1, CAST(0x0000A2FF003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1085, 2, N'Musina', N'Musina', N'MUSINA', 1, 1, CAST(0x0000A2FF004BF4EC AS DateTime), 1, CAST(0x0000A2FF004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1086, 2, N'Naboomspruit', N'Naboomspruit', N'NABOOMSPRUIT', 1, 1, CAST(0x0000A2FF005C6FAC AS DateTime), 1, CAST(0x0000A2FF005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1087, 6, N'Nelspruit', N'Nelspruit', N'NELSPRUIT', 1, 1, CAST(0x0000A2FF006CEA6C AS DateTime), 1, CAST(0x0000A2FF006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1088, 4, N'Newcastle', N'Newcastle', N'NEWCASTLE', 1, 1, CAST(0x0000A2FF007D652C AS DateTime), 1, CAST(0x0000A2FF007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1089, 4, N'New Germany', N'New Germany', N'NEW_GERMANY', 1, 1, CAST(0x0000A2FF008DDFEC AS DateTime), 1, CAST(0x0000A2FF008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1090, 4, N'New Hanover', N'New Hanover', N'NEW_HANOVER', 1, 1, CAST(0x0000A2FF009E5AAC AS DateTime), 1, CAST(0x0000A2FF009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1091, 5, N'Engcobo', N'Engcobo', N'ENGCOBO', 1, 1, CAST(0x0000A2FF00AED56C AS DateTime), 1, CAST(0x0000A2FF00AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1092, 5, N'Nieu-Bethesda', N'Nieu-Bethesda', N'NIEU-BETHESDA', 1, 1, CAST(0x0000A2FF00BF502C AS DateTime), 1, CAST(0x0000A2FF00BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1093, 1, N'Nigel', N'Nigel', N'NIGEL', 1, 1, CAST(0x0000A2FF00CFCAEC AS DateTime), 1, CAST(0x0000A2FF00CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1094, 4, N'Nongoma', N'Nongoma', N'NONGOMA', 1, 1, CAST(0x0000A2FF00E045AC AS DateTime), 1, CAST(0x0000A2FF00E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1095, 4, N'Nottingham Road', N'Nottingham Road', N'NOTTINGHAM_ROAD', 1, 1, CAST(0x0000A2FF00F0C06C AS DateTime), 1, CAST(0x0000A2FF00F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1096, 3, N'Odendaalsrus', N'Odendaalsrus', N'ODENDAALSRUS', 1, 1, CAST(0x0000A2FF01013B2C AS DateTime), 1, CAST(0x0000A2FF01013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1097, 6, N'Ogies', N'Ogies', N'OGIES', 1, 1, CAST(0x0000A2FF0111B5EC AS DateTime), 1, CAST(0x0000A2FF0111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1098, 6, N'Ohrigstad', N'Ohrigstad', N'OHRIGSTAD', 1, 1, CAST(0x0000A2FF012230AC AS DateTime), 1, CAST(0x0000A2FF012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1099, 8, N'Orania, 8', N'Orania, 8', N'ORANIA,_8', 1, 1, CAST(0x0000A2FF0132AB6C AS DateTime), 1, CAST(0x0000A2FF0132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1100, 3, N'Oranjeville', N'Oranjeville', N'ORANJEVILLE', 1, 1, CAST(0x0000A2FF0143262C AS DateTime), 1, CAST(0x0000A2FF0143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1101, 1, N'Orchards', N'Orchards', N'ORCHARDS', 1, 1, CAST(0x0000A2FF0153A0EC AS DateTime), 1, CAST(0x0000A2FF0153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1102, 7, N'Orkney', N'Orkney', N'ORKNEY', 1, 1, CAST(0x0000A2FF01641BAC AS DateTime), 1, CAST(0x0000A2FF01641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1103, 9, N'Oudtshoorn', N'Oudtshoorn', N'OUDTSHOORN', 1, 1, CAST(0x0000A2FF0174966C AS DateTime), 1, CAST(0x0000A2FF0174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1104, 5, N'Oyster Bay', N'Oyster Bay', N'OYSTER_BAY', 1, 1, CAST(0x0000A2FF0185112C AS DateTime), 1, CAST(0x0000A2FF0185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1105, 9, N'Paarl', N'Paarl', N'PAARL', 1, 1, CAST(0x0000A300000A09EC AS DateTime), 1, CAST(0x0000A300000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1106, 4, N'Palm Beach', N'Palm Beach', N'PALM_BEACH', 1, 1, CAST(0x0000A300001A84AC AS DateTime), 1, CAST(0x0000A300001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1107, 4, N'Park Rynie', N'Park Rynie', N'PARK_RYNIE', 1, 1, CAST(0x0000A300002AFF6C AS DateTime), 1, CAST(0x0000A300002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1108, 3, N'Parys', N'Parys', N'PARYS', 1, 1, CAST(0x0000A300003B7A2C AS DateTime), 1, CAST(0x0000A300003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1109, 5, N'Patensie', N'Patensie', N'PATENSIE', 1, 1, CAST(0x0000A300004BF4EC AS DateTime), 1, CAST(0x0000A300004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1110, 5, N'Paterson', N'Paterson', N'PATERSON', 1, 1, CAST(0x0000A300005C6FAC AS DateTime), 1, CAST(0x0000A300005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1111, 4, N'Paulpietersburg', N'Paulpietersburg', N'PAULPIETERSBURG', 1, 1, CAST(0x0000A300006CEA6C AS DateTime), 1, CAST(0x0000A300006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1112, 3, N'Paul Roux', N'Paul Roux', N'PAUL_ROUX', 1, 1, CAST(0x0000A300007D652C AS DateTime), 1, CAST(0x0000A300007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1113, 4, N'Pennington', N'Pennington', N'PENNINGTON', 1, 1, CAST(0x0000A300008DDFEC AS DateTime), 1, CAST(0x0000A300008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1114, 6, N'Perdekop', N'Perdekop', N'PERDEKOP', 1, 1, CAST(0x0000A300009E5AAC AS DateTime), 1, CAST(0x0000A300009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1115, 3, N'Petrusburg', N'Petrusburg', N'PETRUSBURG', 1, 1, CAST(0x0000A30000AED56C AS DateTime), 1, CAST(0x0000A30000AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1116, 3, N'Petrus Steyn', N'Petrus Steyn', N'PETRUS_STEYN', 1, 1, CAST(0x0000A30000BF502C AS DateTime), 1, CAST(0x0000A30000BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1117, 2, N'Phalaborwa', N'Phalaborwa', N'PHALABORWA', 1, 1, CAST(0x0000A30000CFCAEC AS DateTime), 1, CAST(0x0000A30000CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1118, 3, N'Philippolis', N'Philippolis', N'PHILIPPOLIS', 1, 1, CAST(0x0000A30000E045AC AS DateTime), 1, CAST(0x0000A30000E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1119, 3, N'Phuthaditjhaba', N'Phuthaditjhaba', N'PHUTHADITJHABA', 1, 1, CAST(0x0000A30000F0C06C AS DateTime), 1, CAST(0x0000A30000F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1120, 6, N'Piet Retief', N'Piet Retief', N'PIET_RETIEF', 1, 1, CAST(0x0000A30001013B2C AS DateTime), 1, CAST(0x0000A30001013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1121, 4, N'Pietermaritzburg', N'Pietermaritzburg', N'PIETERMARITZBURG', 1, 1, CAST(0x0000A3000111B5EC AS DateTime), 1, CAST(0x0000A3000111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1122, 9, N'Piketberg', N'Piketberg', N'PIKETBERG', 1, 1, CAST(0x0000A300012230AC AS DateTime), 1, CAST(0x0000A300012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1123, 6, N'Pilgrim''s Rest', N'Pilgrim''s Rest', N'PILGRIM''S_REST', 1, 1, CAST(0x0000A3000132AB6C AS DateTime), 1, CAST(0x0000A3000132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1124, 4, N'Pinetown', N'Pinetown', N'PINETOWN', 1, 1, CAST(0x0000A3000143262C AS DateTime), 1, CAST(0x0000A3000143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1125, 9, N'Plettenberg Bay', N'Plettenberg Bay', N'PLETTENBERG_BAY', 1, 1, CAST(0x0000A3000153A0EC AS DateTime), 1, CAST(0x0000A3000153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1126, 9, N'Pniel', N'Pniel', N'PNIEL', 1, 1, CAST(0x0000A30001641BAC AS DateTime), 1, CAST(0x0000A30001641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1127, 2, N'Polokwane', N'Polokwane', N'POLOKWANE', 1, 1, CAST(0x0000A3000174966C AS DateTime), 1, CAST(0x0000A3000174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1128, 4, N'Pomeroy', N'Pomeroy', N'POMEROY', 1, 1, CAST(0x0000A3000185112C AS DateTime), 1, CAST(0x0000A3000185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1129, 4, N'Pongola', N'Pongola', N'PONGOLA', 1, 1, CAST(0x0000A301000A09EC AS DateTime), 1, CAST(0x0000A301000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1130, 5, N'Port Alfred', N'Port Alfred', N'PORT_ALFRED', 1, 1, CAST(0x0000A301001A84AC AS DateTime), 1, CAST(0x0000A301001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1131, 4, N'Port Edward', N'Port Edward', N'PORT_EDWARD', 1, 1, CAST(0x0000A301002AFF6C AS DateTime), 1, CAST(0x0000A301002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1132, 5, N'Port Elizabeth', N'Port Elizabeth', N'PORT_ELIZABETH', 1, 1, CAST(0x0000A301003B7A2C AS DateTime), 1, CAST(0x0000A301003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1133, 4, N'Port Shepstone', N'Port Shepstone', N'PORT_SHEPSTONE', 1, 1, CAST(0x0000A301004BF4EC AS DateTime), 1, CAST(0x0000A301004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1134, 5, N'Port St. Johns', N'Port St. Johns', N'PORT_ST._JOHNS', 1, 1, CAST(0x0000A301005C6FAC AS DateTime), 1, CAST(0x0000A301005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1135, 7, N'Potchefstroom', N'Potchefstroom', N'POTCHEFSTROOM', 1, 1, CAST(0x0000A301006CEA6C AS DateTime), 1, CAST(0x0000A301006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1136, 1, N'Pretoria', N'Pretoria', N'PRETORIA', 1, 1, CAST(0x0000A301007D652C AS DateTime), 1, CAST(0x0000A301007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1137, 8, N'Prieska', N'Prieska', N'PRIESKA', 1, 1, CAST(0x0000A301008DDFEC AS DateTime), 1, CAST(0x0000A301008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1138, 4, N'Queensburgh', N'Queensburgh', N'QUEENSBURGH', 1, 1, CAST(0x0000A301009E5AAC AS DateTime), 1, CAST(0x0000A301009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1139, 5, N'Queenstown', N'Queenstown', N'QUEENSTOWN', 1, 1, CAST(0x0000A30100AED56C AS DateTime), 1, CAST(0x0000A30100AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1140, 4, N'Ramsgate', N'Ramsgate', N'RAMSGATE', 1, 1, CAST(0x0000A30100BF502C AS DateTime), 1, CAST(0x0000A30100BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1141, 1, N'Randburg', N'Randburg', N'RANDBURG', 1, 1, CAST(0x0000A30100CFCAEC AS DateTime), 1, CAST(0x0000A30100CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1142, 1, N'Randfontein', N'Randfontein', N'RANDFONTEIN', 1, 1, CAST(0x0000A30100E045AC AS DateTime), 1, CAST(0x0000A30100E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1143, 1, N'Ratanda', N'Ratanda', N'RATANDA', 1, 1, CAST(0x0000A30100F0C06C AS DateTime), 1, CAST(0x0000A30100F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1144, 3, N'Reddersburg', N'Reddersburg', N'REDDERSBURG', 1, 1, CAST(0x0000A30101013B2C AS DateTime), 1, CAST(0x0000A30101013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1145, 3, N'Reitz', N'Reitz', N'REITZ', 1, 1, CAST(0x0000A3010111B5EC AS DateTime), 1, CAST(0x0000A3010111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1146, 4, N'Richards Bay', N'Richards Bay', N'RICHARDS_BAY', 1, 1, CAST(0x0000A301012230AC AS DateTime), 1, CAST(0x0000A301012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1147, 8, N'Richmond', N'Richmond', N'RICHMOND', 1, 1, CAST(0x0000A3010132AB6C AS DateTime), 1, CAST(0x0000A3010132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1148, 9, N'Riebeek Kasteel', N'Riebeek Kasteel', N'RIEBEEK_KASTEEL', 1, 1, CAST(0x0000A3010143262C AS DateTime), 1, CAST(0x0000A3010143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1149, 1, N'Roodepoort', N'Roodepoort', N'ROODEPOORT', 1, 1, CAST(0x0000A3010153A0EC AS DateTime), 1, CAST(0x0000A3010153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1150, 1, N'Rooihuiskraal', N'Rooihuiskraal', N'ROOIHUISKRAAL', 1, 1, CAST(0x0000A30101641BAC AS DateTime), 1, CAST(0x0000A30101641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1151, 3, N'Rosendal', N'Rosendal', N'ROSENDAL', 1, 1, CAST(0x0000A3010174966C AS DateTime), 1, CAST(0x0000A3010174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1152, 3, N'Rouxville', N'Rouxville', N'ROUXVILLE', 1, 1, CAST(0x0000A3010185112C AS DateTime), 1, CAST(0x0000A3010185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1153, 7, N'Rustenburg', N'Rustenburg', N'RUSTENBURG', 1, 1, CAST(0x0000A302000A09EC AS DateTime), 1, CAST(0x0000A302000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1154, 6, N'Sabie', N'Sabie', N'SABIE', 1, 1, CAST(0x0000A302001A84AC AS DateTime), 1, CAST(0x0000A302001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1155, 4, N'Salt Rock', N'Salt Rock', N'SALT_ROCK', 1, 1, CAST(0x0000A302002AFF6C AS DateTime), 1, CAST(0x0000A302002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1156, 1, N'Sandton', N'Sandton', N'SANDTON', 1, 1, CAST(0x0000A302003B7A2C AS DateTime), 1, CAST(0x0000A302003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1157, 3, N'Sasolburg', N'Sasolburg', N'SASOLBURG', 1, 1, CAST(0x0000A302004BF4EC AS DateTime), 1, CAST(0x0000A302004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1158, 7, N'Schweizer-Reneke', N'Schweizer-Reneke', N'SCHWEIZER-RENEKE', 1, 1, CAST(0x0000A302005C6FAC AS DateTime), 1, CAST(0x0000A302005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1159, 4, N'Scottburgh', N'Scottburgh', N'SCOTTBURGH', 1, 1, CAST(0x0000A302006CEA6C AS DateTime), 1, CAST(0x0000A302006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1160, 1, N'Sebokeng', N'Sebokeng', N'SEBOKENG', 1, 1, CAST(0x0000A302007D652C AS DateTime), 1, CAST(0x0000A302007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1161, 6, N'Secunda', N'Secunda', N'SECUNDA', 1, 1, CAST(0x0000A302008DDFEC AS DateTime), 1, CAST(0x0000A302008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1162, 3, N'Senekal', N'Senekal', N'SENEKAL', 1, 1, CAST(0x0000A302009E5AAC AS DateTime), 1, CAST(0x0000A302009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1163, 4, N'Sezela', N'Sezela', N'SEZELA', 1, 1, CAST(0x0000A30200AED56C AS DateTime), 1, CAST(0x0000A30200AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1164, 1, N'Sharpeville', N'Sharpeville', N'SHARPEVILLE', 1, 1, CAST(0x0000A30200BF502C AS DateTime), 1, CAST(0x0000A30200BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1165, 4, N'Shelly Beach', N'Shelly Beach', N'SHELLY_BEACH', 1, 1, CAST(0x0000A30200CFCAEC AS DateTime), 1, CAST(0x0000A30200CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1166, 3, N'Smithfield', N'Smithfield', N'SMITHFIELD', 1, 1, CAST(0x0000A30200E045AC AS DateTime), 1, CAST(0x0000A30200E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1167, 5, N'Somerset East', N'Somerset East', N'SOMERSET_EAST', 1, 1, CAST(0x0000A30200F0C06C AS DateTime), 1, CAST(0x0000A30200F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1168, 9, N'Somerset West', N'Somerset West', N'SOMERSET_WEST', 1, 1, CAST(0x0000A30201013B2C AS DateTime), 1, CAST(0x0000A30201013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1169, 1, N'Soshanguve', N'Soshanguve', N'SOSHANGUVE', 1, 1, CAST(0x0000A3020111B5EC AS DateTime), 1, CAST(0x0000A3020111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1170, 4, N'Southbroom', N'Southbroom', N'SOUTHBROOM', 1, 1, CAST(0x0000A302012230AC AS DateTime), 1, CAST(0x0000A302012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1171, 1, N'Soweto', N'Soweto', N'SOWETO', 1, 1, CAST(0x0000A3020132AB6C AS DateTime), 1, CAST(0x0000A3020132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1172, 8, N'Springbok', N'Springbok', N'SPRINGBOK', 1, 1, CAST(0x0000A3020143262C AS DateTime), 1, CAST(0x0000A3020143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1173, 3, N'Springfontein', N'Springfontein', N'SPRINGFONTEIN', 1, 1, CAST(0x0000A3020153A0EC AS DateTime), 1, CAST(0x0000A3020153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1174, 1, N'Springs', N'Springs', N'SPRINGS', 1, 1, CAST(0x0000A30201641BAC AS DateTime), 1, CAST(0x0000A30201641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1175, 6, N'Standerton', N'Standerton', N'STANDERTON', 1, 1, CAST(0x0000A3020174966C AS DateTime), 1, CAST(0x0000A3020174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1176, 7, N'Stilfontein', N'Stilfontein', N'STILFONTEIN', 1, 1, CAST(0x0000A3020185112C AS DateTime), 1, CAST(0x0000A3020185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1177, 9, N'Stellenbosch', N'Stellenbosch', N'STELLENBOSCH', 1, 1, CAST(0x0000A303000A09EC AS DateTime), 1, CAST(0x0000A303000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1178, 5, N'Steynsburg', N'Steynsburg', N'STEYNSBURG', 1, 1, CAST(0x0000A303001A84AC AS DateTime), 1, CAST(0x0000A303001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1179, 3, N'Steynsrus', N'Steynsrus', N'STEYNSRUS', 1, 1, CAST(0x0000A303002AFF6C AS DateTime), 1, CAST(0x0000A303002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1180, 5, N'St Francis Bay', N'St Francis Bay', N'ST_FRANCIS_BAY', 1, 1, CAST(0x0000A303003B7A2C AS DateTime), 1, CAST(0x0000A303003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1181, 4, N'St Lucia', N'St Lucia', N'ST_LUCIA', 1, 1, CAST(0x0000A303004BF4EC AS DateTime), 1, CAST(0x0000A303004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1182, 4, N'St Michael''s-on-sea', N'St Michael''s-on-sea', N'ST_MICHAEL''S-ON-SEA', 1, 1, CAST(0x0000A303005C6FAC AS DateTime), 1, CAST(0x0000A303005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1183, 9, N'Strand', N'Strand', N'STRAND', 1, 1, CAST(0x0000A303006CEA6C AS DateTime), 1, CAST(0x0000A303006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1184, 8, N'Strydenburg', N'Strydenburg', N'STRYDENBURG', 1, 1, CAST(0x0000A303007D652C AS DateTime), 1, CAST(0x0000A303007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1185, 5, N'Stutterheim', N'Stutterheim', N'STUTTERHEIM', 1, 1, CAST(0x0000A303008DDFEC AS DateTime), 1, CAST(0x0000A303008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1186, 9, N'Swartberg', N'Swartberg', N'SWARTBERG', 1, 1, CAST(0x0000A303009E5AAC AS DateTime), 1, CAST(0x0000A303009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1187, 9, N'Swellendam', N'Swellendam', N'SWELLENDAM', 1, 1, CAST(0x0000A30300AED56C AS DateTime), 1, CAST(0x0000A30300AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1188, 3, N'Swinburne', N'Swinburne', N'SWINBURNE', 1, 1, CAST(0x0000A30300BF502C AS DateTime), 1, CAST(0x0000A30300BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1189, 5, N'Tarkastad', N'Tarkastad', N'TARKASTAD', 1, 1, CAST(0x0000A30300CFCAEC AS DateTime), 1, CAST(0x0000A30300CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1190, 1, N'Tembisa', N'Tembisa', N'TEMBISA', 1, 1, CAST(0x0000A30300E045AC AS DateTime), 1, CAST(0x0000A30300E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1191, 3, N'Thaba Nchu', N'Thaba Nchu', N'THABA_NCHU', 1, 1, CAST(0x0000A30300F0C06C AS DateTime), 1, CAST(0x0000A30300F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1192, 2, N'Thabazimbi', N'Thabazimbi', N'THABAZIMBI', 1, 1, CAST(0x0000A30301013B2C AS DateTime), 1, CAST(0x0000A30301013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1193, 3, N'Theunissen', N'Theunissen', N'THEUNISSEN', 1, 1, CAST(0x0000A3030111B5EC AS DateTime), 1, CAST(0x0000A3030111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1194, 2, N'Thohoyandou', N'Thohoyandou', N'THOHOYANDOU', 1, 1, CAST(0x0000A303012230AC AS DateTime), 1, CAST(0x0000A303012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1195, 1, N'Thokoza', N'Thokoza', N'THOKOZA', 1, 1, CAST(0x0000A3030132AB6C AS DateTime), 1, CAST(0x0000A3030132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1196, 4, N'Tongaat', N'Tongaat', N'TONGAAT', 1, 1, CAST(0x0000A3030143262C AS DateTime), 1, CAST(0x0000A3030143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1197, 6, N'Trichardt', N'Trichardt', N'TRICHARDT', 1, 1, CAST(0x0000A3030153A0EC AS DateTime), 1, CAST(0x0000A3030153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1198, 3, N'Trompsburg', N'Trompsburg', N'TROMPSBURG', 1, 1, CAST(0x0000A30301641BAC AS DateTime), 1, CAST(0x0000A30301641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1199, 1, N'Tsakane', N'Tsakane', N'TSAKANE', 1, 1, CAST(0x0000A3030174966C AS DateTime), 1, CAST(0x0000A3030174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1200, 4, N'Tugela Ferry', N'Tugela Ferry', N'TUGELA_FERRY', 1, 1, CAST(0x0000A3030185112C AS DateTime), 1, CAST(0x0000A3030185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1201, 9, N'Tulbagh', N'Tulbagh', N'TULBAGH', 1, 1, CAST(0x0000A304000A09EC AS DateTime), 1, CAST(0x0000A304000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1202, 3, N'Tweeling', N'Tweeling', N'TWEELING', 1, 1, CAST(0x0000A304001A84AC AS DateTime), 1, CAST(0x0000A304001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1203, 10, N'Tweespruit', N'Tweespruit', N'TWEESPRUIT', 1, 1, CAST(0x0000A304002AFF6C AS DateTime), 1, CAST(0x0000A304002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1204, 4, N'Ubombo', N'Ubombo', N'UBOMBO', 1, 1, CAST(0x0000A304003B7A2C AS DateTime), 1, CAST(0x0000A304003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1205, 5, N'Uitenhage', N'Uitenhage', N'UITENHAGE', 1, 1, CAST(0x0000A304004BF4EC AS DateTime), 1, CAST(0x0000A304004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1206, 4, N'Ulundi', N'Ulundi', N'ULUNDI', 1, 1, CAST(0x0000A304005C6FAC AS DateTime), 1, CAST(0x0000A304005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1207, 4, N'Umbogintwini', N'Umbogintwini', N'UMBOGINTWINI', 1, 1, CAST(0x0000A304006CEA6C AS DateTime), 1, CAST(0x0000A304006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1208, 4, N'Umdloti', N'Umdloti', N'UMDLOTI', 1, 1, CAST(0x0000A304007D652C AS DateTime), 1, CAST(0x0000A304007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1209, 4, N'Umgababa', N'Umgababa', N'UMGABABA', 1, 1, CAST(0x0000A304008DDFEC AS DateTime), 1, CAST(0x0000A304008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1210, 4, N'Umhlanga Rocks', N'Umhlanga Rocks', N'UMHLANGA_ROCKS', 1, 1, CAST(0x0000A304009E5AAC AS DateTime), 1, CAST(0x0000A304009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1211, 4, N'Umkomaas', N'Umkomaas', N'UMKOMAAS', 1, 1, CAST(0x0000A30400AED56C AS DateTime), 1, CAST(0x0000A30400AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1212, 4, N'Umlazi', N'Umlazi', N'UMLAZI', 1, 1, CAST(0x0000A30400BF502C AS DateTime), 1, CAST(0x0000A30400BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1213, 4, N'Umtentweni', N'Umtentweni', N'UMTENTWENI', 1, 1, CAST(0x0000A30400CFCAEC AS DateTime), 1, CAST(0x0000A30400CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1214, 5, N'uMthatha', N'uMthatha', N'UMTHATHA', 1, 1, CAST(0x0000A30400E045AC AS DateTime), 1, CAST(0x0000A30400E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1215, 4, N'Umzinto', N'Umzinto', N'UMZINTO', 1, 1, CAST(0x0000A30400F0C06C AS DateTime), 1, CAST(0x0000A30400F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1216, 4, N'Umzumbe', N'Umzumbe', N'UMZUMBE', 1, 1, CAST(0x0000A30401013B2C AS DateTime), 1, CAST(0x0000A30401013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1217, 4, N'Underberg', N'Underberg', N'UNDERBERG', 1, 1, CAST(0x0000A3040111B5EC AS DateTime), 1, CAST(0x0000A3040111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1218, 8, N'Upington', N'Upington', N'UPINGTON', 1, 1, CAST(0x0000A304012230AC AS DateTime), 1, CAST(0x0000A304012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1219, 9, N'Uniondale, 9', N'Uniondale, 9', N'UNIONDALE,_9', 1, 1, CAST(0x0000A3040132AB6C AS DateTime), 1, CAST(0x0000A3040132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1220, 4, N'Utrecht', N'Utrecht', N'UTRECHT', 1, 1, CAST(0x0000A3040143262C AS DateTime), 1, CAST(0x0000A3040143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1221, 4, N'Uvongo', N'Uvongo', N'UVONGO', 1, 1, CAST(0x0000A3040153A0EC AS DateTime), 1, CAST(0x0000A3040153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1222, 6, N'Vaalbank', N'Vaalbank', N'VAALBANK', 1, 1, CAST(0x0000A30401641BAC AS DateTime), 1, CAST(0x0000A30401641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1223, 2, N'Vaalwater', N'Vaalwater', N'VAALWATER', 1, 1, CAST(0x0000A3040174966C AS DateTime), 1, CAST(0x0000A3040174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1224, 1, N'Vanderbijlpark', N'Vanderbijlpark', N'VANDERBIJLPARK', 1, 1, CAST(0x0000A3040185112C AS DateTime), 1, CAST(0x0000A3040185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1225, 4, N'Van Reenen', N'Van Reenen', N'VAN_REENEN', 1, 1, CAST(0x0000A305000A09EC AS DateTime), 1, CAST(0x0000A305000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1226, 3, N'Van Stadensrus', N'Van Stadensrus', N'VAN_STADENSRUS', 1, 1, CAST(0x0000A305001A84AC AS DateTime), 1, CAST(0x0000A305001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1227, 3, N'Ventersburg', N'Ventersburg', N'VENTERSBURG', 1, 1, CAST(0x0000A305002AFF6C AS DateTime), 1, CAST(0x0000A305002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1228, 1, N'Vereeniging', N'Vereeniging', N'VEREENIGING', 1, 1, CAST(0x0000A305003B7A2C AS DateTime), 1, CAST(0x0000A305003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1229, 3, N'Verkeerdevlei', N'Verkeerdevlei', N'VERKEERDEVLEI', 1, 1, CAST(0x0000A305004BF4EC AS DateTime), 1, CAST(0x0000A305004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1230, 4, N'Verulam', N'Verulam', N'VERULAM', 1, 1, CAST(0x0000A305005C6FAC AS DateTime), 1, CAST(0x0000A305005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1231, 8, N'Victoria West', N'Victoria West', N'VICTORIA_WEST', 1, 1, CAST(0x0000A305006CEA6C AS DateTime), 1, CAST(0x0000A305006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1232, 3, N'Viljoenskroon', N'Viljoenskroon', N'VILJOENSKROON', 1, 1, CAST(0x0000A305007D652C AS DateTime), 1, CAST(0x0000A305007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1233, 3, N'Villiers', N'Villiers', N'VILLIERS', 1, 1, CAST(0x0000A305008DDFEC AS DateTime), 1, CAST(0x0000A305008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1234, 4, N'Virginia', N'Virginia', N'VIRGINIA', 1, 1, CAST(0x0000A305009E5AAC AS DateTime), 1, CAST(0x0000A305009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1235, 2, N'Vivo', N'Vivo', N'VIVO', 1, 1, CAST(0x0000A30500AED56C AS DateTime), 1, CAST(0x0000A30500AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1236, 6, N'Volksrust', N'Volksrust', N'VOLKSRUST', 1, 1, CAST(0x0000A30500BF502C AS DateTime), 1, CAST(0x0000A30500BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1237, 1, N'Vosloorus', N'Vosloorus', N'VOSLOORUS', 1, 1, CAST(0x0000A30500CFCAEC AS DateTime), 1, CAST(0x0000A30500CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1238, 3, N'Vrede', N'Vrede', N'VREDE', 1, 1, CAST(0x0000A30500E045AC AS DateTime), 1, CAST(0x0000A30500E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1239, 3, N'Vredefort', N'Vredefort', N'VREDEFORT', 1, 1, CAST(0x0000A30500F0C06C AS DateTime), 1, CAST(0x0000A30500F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1240, 7, N'Vryburg', N'Vryburg', N'VRYBURG', 1, 1, CAST(0x0000A30501013B2C AS DateTime), 1, CAST(0x0000A30501013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1241, 4, N'Vryheid', N'Vryheid', N'VRYHEID', 1, 1, CAST(0x0000A3050111B5EC AS DateTime), 1, CAST(0x0000A3050111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1242, 6, N'Wakkerstroom', N'Wakkerstroom', N'WAKKERSTROOM', 1, 1, CAST(0x0000A305012230AC AS DateTime), 1, CAST(0x0000A305012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1243, 3, N'Warden', N'Warden', N'WARDEN', 1, 1, CAST(0x0000A3050132AB6C AS DateTime), 1, CAST(0x0000A3050132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1244, 4, N'Warner Beach', N'Warner Beach', N'WARNER_BEACH', 1, 1, CAST(0x0000A3050143262C AS DateTime), 1, CAST(0x0000A3050143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1245, 8, N'Warrenton', N'Warrenton', N'WARRENTON', 1, 1, CAST(0x0000A3050153A0EC AS DateTime), 1, CAST(0x0000A3050153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1246, 4, N'Wartburg', N'Wartburg', N'WARTBURG', 1, 1, CAST(0x0000A30501641BAC AS DateTime), 1, CAST(0x0000A30501641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1247, 4, N'Wasbank', N'Wasbank', N'WASBANK', 1, 1, CAST(0x0000A3050174966C AS DateTime), 1, CAST(0x0000A3050174966C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1248, 6, N'Waterval Boven', N'Waterval Boven', N'WATERVAL_BOVEN', 1, 1, CAST(0x0000A3050185112C AS DateTime), 1, CAST(0x0000A3050185112C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1249, 6, N'Waterval Onder', N'Waterval Onder', N'WATERVAL_ONDER', 1, 1, CAST(0x0000A306000A09EC AS DateTime), 1, CAST(0x0000A306000A09EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1250, 4, N'Weenen', N'Weenen', N'WEENEN', 1, 1, CAST(0x0000A306001A84AC AS DateTime), 1, CAST(0x0000A306001A84AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1251, 3, N'Welkom', N'Welkom', N'WELKOM', 1, 1, CAST(0x0000A306002AFF6C AS DateTime), 1, CAST(0x0000A306002AFF6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1252, 9, N'Wellington', N'Wellington', N'WELLINGTON', 1, 1, CAST(0x0000A306003B7A2C AS DateTime), 1, CAST(0x0000A306003B7A2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1253, 3, N'Wepener', N'Wepener', N'WEPENER', 1, 1, CAST(0x0000A306004BF4EC AS DateTime), 1, CAST(0x0000A306004BF4EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1254, 3, N'Wesselsbron', N'Wesselsbron', N'WESSELSBRON', 1, 1, CAST(0x0000A306005C6FAC AS DateTime), 1, CAST(0x0000A306005C6FAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1255, 1, N'Westonaria', N'Westonaria', N'WESTONARIA', 1, 1, CAST(0x0000A306006CEA6C AS DateTime), 1, CAST(0x0000A306006CEA6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1256, 4, N'Westville', N'Westville', N'WESTVILLE', 1, 1, CAST(0x0000A306007D652C AS DateTime), 1, CAST(0x0000A306007D652C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1257, 6, N'White River', N'White River', N'WHITE_RIVER', 1, 1, CAST(0x0000A306008DDFEC AS DateTime), 1, CAST(0x0000A306008DDFEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1258, 5, N'Whittlesea', N'Whittlesea', N'WHITTLESEA', 1, 1, CAST(0x0000A306009E5AAC AS DateTime), 1, CAST(0x0000A306009E5AAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1259, 9, N'Wilderness', N'Wilderness', N'WILDERNESS', 1, 1, CAST(0x0000A30600AED56C AS DateTime), 1, CAST(0x0000A30600AED56C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1260, 8, N'Williston', N'Williston', N'WILLISTON', 1, 1, CAST(0x0000A30600BF502C AS DateTime), 1, CAST(0x0000A30600BF502C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1261, 3, N'Winburg', N'Winburg', N'WINBURG', 1, 1, CAST(0x0000A30600CFCAEC AS DateTime), 1, CAST(0x0000A30600CFCAEC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1262, 4, N'Winkelspruit', N'Winkelspruit', N'WINKELSPRUIT', 1, 1, CAST(0x0000A30600E045AC AS DateTime), 1, CAST(0x0000A30600E045AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1263, 4, N'Winterton', N'Winterton', N'WINTERTON', 1, 1, CAST(0x0000A30600F0C06C AS DateTime), 1, CAST(0x0000A30600F0C06C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1264, 6, N'Witbank', N'Witbank', N'WITBANK', 1, 1, CAST(0x0000A30601013B2C AS DateTime), 1, CAST(0x0000A30601013B2C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1265, 7, N'Wolmaransstad', N'Wolmaransstad', N'WOLMARANSSTAD', 1, 1, CAST(0x0000A3060111B5EC AS DateTime), 1, CAST(0x0000A3060111B5EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1267, 4, N'York', N'York', N'YORK', 1, 1, CAST(0x0000A3060132AB6C AS DateTime), 1, CAST(0x0000A3060132AB6C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1268, 3, N'Zastron', N'Zastron', N'ZASTRON', 1, 1, CAST(0x0000A3060143262C AS DateTime), 1, CAST(0x0000A3060143262C AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1269, 7, N'Zeerust', N'Zeerust', N'ZEERUST', 1, 1, CAST(0x0000A3060153A0EC AS DateTime), 1, CAST(0x0000A3060153A0EC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1270, 5, N'Zwelitsha', N'Zwelitsha', N'ZWELITSHA', 1, 1, CAST(0x0000A30601641BAC AS DateTime), 1, CAST(0x0000A30601641BAC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1277, 9, N'Belville', N'Belville', N'BELVILLE', 1, 1, CAST(0x0000A2F4012230AC AS DateTime), 1, CAST(0x0000A2F4012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1278, 9, N'Brakenfell', N'Brakenfell', N'BRAKENFELL', 1, 1, CAST(0x0000A2F4012230AC AS DateTime), 1, CAST(0x0000A2F4012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1279, 9, N'Eersterivier', N'Eersterivier', N'EERSTERIVIER', 1, 1, CAST(0x0000A2F4012230AC AS DateTime), 1, CAST(0x0000A2F4012230AC AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1280, 1, N'Johannesburg', N'Johannesburg', N'JOHANNESBURG', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1281, 9, N'Kayamandi', N'Kayamandi', N'KAYAMANDI', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1282, 9, N'Kaymor Stikland', N'Kaymor Stikland', N'KAYMOR_STIKLAN', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1283, 9, N'Kraaifontein', N'Kraaifontein', N'KRAAIFONTEIN', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1284, 9, N'Kuilsrivier', N'Kuilsrivier', N'KUILSRIVIER', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1285, 9, N'Malmsbury', N'Malmsbury', N'MALMSBURY', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1286, 9, N'Mbekweni', N'Mbekweni', N'MBEKWENI', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1287, 9, N'Paarl', N'Paarl', N'PAARL', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1288, 9, N'Philippi', N'Philippi', N'PHILIPPI', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1289, 9, N'Robertson', N'Robertson', N'ROBERTSON', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1290, 9, N'Vredenburg', N'Vredenburg', N'VREDENBURG', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
INSERT [dbo].[City] ([CityId], [ProvinceId], [Name], [Description], [LookUpKey], [IsNotDeleted], [LastChangedById], [LastChangedDate], [CreatedById], [CreateDate]) VALUES (1291, 9, N'Wynburg', N'Wynburg', N'WYNBURG', 1, 1, CAST(0x0000A2F800A89F1F AS DateTime), 1, CAST(0x0000A2F800A89F1F AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[City] OFF
GO

