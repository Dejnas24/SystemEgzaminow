/*
 SystemEgzaminow - English public demo database
 Generated from the project's SQL Server schema and sanitized for publication.

 Demo accounts:
   Administrator: login admin        password Admin123!
   Teacher:       login teacher   password Teacher123!
   Student:       login student        password Student123!

 This script contains DEMO DATA ONLY.
 It does not contain the author's private SQL Server connection credentials
 or private/test content from the development database.
*/

USE [master]
GO

IF DB_ID(N'EgzaminyTestyDb_EN') IS NULL
BEGIN
    CREATE DATABASE [EgzaminyTestyDb_EN];
END
GO

USE [EgzaminyTestyDb_EN]
GO

/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Klasy]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Klasy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NazwaKlasy] [nvarchar](450) NOT NULL,
	[RokSzkolny] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Klasy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogiLogowan]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogiLogowan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UzytkownikId] [int] NULL,
	[DataLogowania] [datetime2](7) NOT NULL,
	[DataWylogowania] [datetime2](7) NULL,
	[CzySukces] [bit] NOT NULL,
	[Sesja] [int] NOT NULL,
	[AdresIp] [nvarchar](max) NULL,
 CONSTRAINT [PK_LogiLogowan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogiRozwiazywaniaTestu]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogiRozwiazywaniaTestu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrzypisanyTestId] [int] NOT NULL,
	[UzytkownikId] [int] NOT NULL,
	[DataZdarzenia] [datetime2](7) NOT NULL,
	[TypZdarzenia] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_LogiRozwiazywaniaTestu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Odpowiedzi]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Odpowiedzi](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrescOdpowiedzi] [nvarchar](max) NOT NULL,
	[CzyPoprawna] [bit] NOT NULL,
	[LiczbaPunktow] [int] NULL,
	[PytanieId] [int] NOT NULL,
 CONSTRAINT [PK_Odpowiedzi] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgiOcen]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgiOcen](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdSkaliOcen] [int] NOT NULL,
	[Ocena] [decimal](3, 1) NOT NULL,
	[ProgOd] [decimal](5, 2) NOT NULL,
	[ProgDo] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_ProgiOcen] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrzypisaneTesty]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrzypisaneTesty](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TestId] [int] NOT NULL,
	[UczenId] [int] NOT NULL,
	[NauczycielId] [int] NOT NULL,
	[DataPrzypisania] [datetime2](7) NOT NULL,
	[DataDostepnosci] [datetime2](7) NOT NULL,
	[DataWygasniecia] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
	[CzyPokazacWynikPoZakonczeniu] [bit] NOT NULL,
	[SposobWyswietlaniaWyniku] [int] NOT NULL,
	[InformacjaKoncowa] [nvarchar](max) NULL,
	[InformacjaStartowa] [nvarchar](max) NULL,
	[LiczbaProb] [int] NOT NULL,
	[SposobOceniania] [int] NOT NULL,
	[KlasaId] [int] NULL,
 CONSTRAINT [PK_PrzypisaneTesty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pytania]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pytania](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrescPytania] [nvarchar](max) NOT NULL,
	[TypPytania] [int] NOT NULL,
	[LiczbaPunktow] [int] NOT NULL,
	[IdAutora] [int] NOT NULL,
	[SciezkaZdjecia] [nvarchar](max) NULL,
	[CzyZarchiwizowane] [bit] NOT NULL,
 CONSTRAINT [PK_Pytania] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nazwa] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RozwiazaneOdpowiedzi]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RozwiazaneOdpowiedzi](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdRozwiazanegoPytania] [int] NOT NULL,
	[IdOdpowiedziZrodlowej] [int] NULL,
	[TrescOdpowiedzi] [nvarchar](2000) NOT NULL,
	[CzyPoprawna] [bit] NOT NULL,
	[CzyWybranaPrzezUcznia] [bit] NOT NULL,
	[TrescOdpowiedziUcznia] [nvarchar](4000) NULL,
	[Kolejnosc] [int] NOT NULL,
	[LiczbaPunktow] [decimal](18, 2) NOT NULL,
	[KomentarzNauczyciela] [nvarchar](max) NULL,
 CONSTRAINT [PK_RozwiazaneOdpowiedzi] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RozwiazanePytania]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RozwiazanePytania](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPytaniaZrodlowego] [int] NULL,
	[TrescPytania] [nvarchar](4000) NOT NULL,
	[TypPytania] [int] NOT NULL,
	[LiczbaPunktowMax] [int] NOT NULL,
	[LiczbaPunktowZdobytych] [int] NOT NULL,
	[IdAutoraPytania] [int] NOT NULL,
	[Kolejnosc] [int] NOT NULL,
	[IdWynikuTestu] [int] NOT NULL,
 CONSTRAINT [PK_RozwiazanePytania] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SkaleOcen]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkaleOcen](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTypuTestu] [int] NOT NULL,
	[IdUzytkownika] [int] NULL,
	[Nazwa] [nvarchar](100) NOT NULL,
	[CzyAktywna] [bit] NOT NULL,
	[DataUtworzenia] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_SkaleOcen] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestPytania]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestPytania](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TestId] [int] NOT NULL,
	[PytanieId] [int] NOT NULL,
	[NumerKolejnosci] [int] NOT NULL,
 CONSTRAINT [PK_TestPytania] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Testy]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Testy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Tytul] [nvarchar](max) NOT NULL,
	[Opis] [nvarchar](max) NULL,
	[ProgZdania] [int] NOT NULL,
	[DataUtworzenia] [datetime2](7) NOT NULL,
	[CzasTrwaniaMinuty] [int] NOT NULL,
	[IdAutora] [int] NOT NULL,
	[IdTypuTestu] [int] NOT NULL,
	[LosujKolejnoscOdpowiedzi] [bit] NOT NULL,
	[LosujKolejnoscPytan] [bit] NOT NULL,
	[CzyZarchiwizowany] [bit] NOT NULL,
 CONSTRAINT [PK_Testy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypyTestow]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypyTestow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NazwaTypu] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_TypyTestow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Uzytkownicy]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Uzytkownicy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](max) NOT NULL,
	[Haslo] [nvarchar](max) NOT NULL,
	[Imie] [nvarchar](max) NOT NULL,
	[Nazwisko] [nvarchar](max) NOT NULL,
	[RolaId] [int] NOT NULL,
	[KlasaId] [int] NULL,
 CONSTRAINT [PK_Uzytkownicy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WynikiTestow]    Script Date: 05/09/2026 19:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WynikiTestow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UzytkownikId] [int] NULL,
	[PrzypisanyTestId] [int] NULL,
	[LiczbaPunktow] [int] NOT NULL,
	[Procent] [decimal](5, 2) NOT NULL,
	[Ocena] [decimal](3, 1) NOT NULL,
	[CzyZdane] [bit] NOT NULL,
	[DataRozpoczecia] [datetime2](7) NULL,
	[DataZakonczenia] [datetime2](7) NULL,
	[CzasRozwiazywaniaMinuty] [int] NULL,
	[MaksymalnaLiczbaPunktow] [int] NOT NULL,
	[CzyProbny] [bit] NULL,
 CONSTRAINT [PK_WynikiTestow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260408183857_InitialCreate', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260408185225_SeedRole', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260409142806_AddFullExamSystem', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260708175329_FixPytanieAutorRelation', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260709161045_RemoveUnusedUzytkownikIdFromPytania', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260712155105_AddQuestionImagePath', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260715131819_AddQuestionArchiveStatus', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260716162531_AddTestResultsModule', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260720124723_AddTestSettingsAndQuestionOrder', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260720130555_RenameProgZadaniaToProgZdania', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260720134816_FixTestAuthorRelation', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260723170523_AddCzyZarchiwizowanyDoTest', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260804141410_MakeTestResultFieldsNullable', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260804161643_AddCzyProbnyToWynikTestu', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260804163548_RemoveOldCzyPrubnyColumn', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260806142208_ChangeRozwiazanaOdpowiedzPointsToDecimal', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260806170000_AddDisplayResultModeToAssignedTest', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260807151026_AddAssignedTestSettings', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260810110242_AddKlasa', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260810175448_AddSposobOcenianiaToPrzypisanyTest', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260813151756_AddKlasaToPrzypisanyTest', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260820181142_UpdateLogLogowaniaSession', N'8.0.25')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260824161625_AddSkalaOcenAndProgOceny', N'8.0.25')
GO

/* ===== Public reference/demo data ===== */


SET IDENTITY_INSERT [dbo].[Klasy] ON
GO
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (1, N'1A', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (10, N'1A', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (19, N'1A', N'2026/2027')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (2, N'1B', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (11, N'1B', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (20, N'1B', N'2026/2027')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (3, N'1C', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (12, N'1C', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (21, N'1C', N'2026/2027')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (4, N'2A', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (13, N'2A', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (22, N'2A', N'2026/2027')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (5, N'2B', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (14, N'2B', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (23, N'2B', N'2026/2027')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (6, N'2C', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (15, N'2C', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (24, N'2C', N'2026/2027')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (7, N'3A', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (16, N'3A', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (25, N'3A', N'2026/2027')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (8, N'3B', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (17, N'3B', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (26, N'3B', N'2026/2027')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (9, N'3C', N'2024/2025')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (18, N'3C', N'2025/2026')
INSERT [dbo].[Klasy] ([Id], [NazwaKlasy], [RokSzkolny]) VALUES (27, N'3C', N'2026/2027')
SET IDENTITY_INSERT [dbo].[Klasy] OFF
GO

SET IDENTITY_INSERT [dbo].[Role] ON
GO
INSERT [dbo].[Role] ([Id], [Nazwa]) VALUES (1, N'Administrator')
INSERT [dbo].[Role] ([Id], [Nazwa]) VALUES (2, N'Teacher')
INSERT [dbo].[Role] ([Id], [Nazwa]) VALUES (3, N'Student')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO

SET IDENTITY_INSERT [dbo].[TypyTestow] ON
GO
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (7, N'Exam')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (9, N'Certification Exam')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (10, N'Other')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (1, N'Quiz')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (6, N'Midterm')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (4, N'Olympiad')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (2, N'Test')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (8, N'Certification Test')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (3, N'Practice Test')
INSERT [dbo].[TypyTestow] ([Id], [NazwaTypu]) VALUES (5, N'Entry Test')
SET IDENTITY_INSERT [dbo].[TypyTestow] OFF
GO

SET IDENTITY_INSERT [dbo].[Uzytkownicy] ON
GO
INSERT [dbo].[Uzytkownicy] ([Id], [Login], [Haslo], [Imie], [Nazwisko], [RolaId], [KlasaId]) VALUES (1, N'admin', N'$2a$11$OBMFBVR69BHicXCSfvM76.DOA56UDvfCQvQT9YpCxiqtFTRX420Xi', N'Admin', N'System', 1, NULL)
INSERT [dbo].[Uzytkownicy] ([Id], [Login], [Haslo], [Imie], [Nazwisko], [RolaId], [KlasaId]) VALUES (2, N'teacher', N'$2a$11$GHgwaN9fnSfe5a/N0NN9WO0UfTv4QRM4YEydXYaw/CDVG4I.9paTG', N'John', N'Smith', 2, NULL)
INSERT [dbo].[Uzytkownicy] ([Id], [Login], [Haslo], [Imie], [Nazwisko], [RolaId], [KlasaId]) VALUES (3, N'student', N'$2a$11$vLwHLtnJMgFIcMRa8apt0OTG7OPFNloOSpf2rruTakDTcr0ghA71C', N'Alex', N'Johnson', 3, 19)
SET IDENTITY_INSERT [dbo].[Uzytkownicy] OFF
GO

SET IDENTITY_INSERT [dbo].[SkaleOcen] ON
GO
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (1, 1, NULL, N'Default - Quiz', 1, CAST(N'2026-08-24T18:29:54.0800000' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (2, 2, NULL, N'Default - Test', 1, CAST(N'2026-08-24T18:29:54.0833333' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (3, 3, NULL, N'Default - Practice Test', 1, CAST(N'2026-08-24T18:29:54.0833333' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (4, 4, NULL, N'Default - Olympiad', 1, CAST(N'2026-08-24T18:29:54.0833333' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (5, 5, NULL, N'Default - Entry Test', 1, CAST(N'2026-08-24T18:29:54.0833333' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (6, 6, NULL, N'Default - Midterm', 1, CAST(N'2026-08-24T18:29:54.0833333' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (7, 7, NULL, N'Default - Exam', 1, CAST(N'2026-08-24T18:29:54.0833333' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (8, 8, NULL, N'Default - Certification Test', 1, CAST(N'2026-08-24T18:29:54.0833333' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (9, 9, NULL, N'Default - Certification Exam', 1, CAST(N'2026-08-24T18:29:54.0833333' AS DateTime2))
INSERT [dbo].[SkaleOcen] ([Id], [IdTypuTestu], [IdUzytkownika], [Nazwa], [CzyAktywna], [DataUtworzenia]) VALUES (10, 10, NULL, N'Default - Other', 1, CAST(N'2026-08-24T18:29:54.0866667' AS DateTime2))
SET IDENTITY_INSERT [dbo].[SkaleOcen] OFF
GO

SET IDENTITY_INSERT [dbo].[ProgiOcen] ON
GO
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (1, 1, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (2, 1, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (3, 1, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (4, 1, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (5, 1, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (6, 1, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (7, 2, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (8, 2, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (9, 2, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (10, 2, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (11, 2, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (12, 2, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (13, 3, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (14, 3, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (15, 3, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (16, 3, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (17, 3, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (18, 3, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (19, 4, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (20, 4, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (21, 4, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (22, 4, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (23, 4, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (24, 4, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (25, 5, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (26, 5, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (27, 5, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (28, 5, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (29, 5, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (30, 5, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (31, 6, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (32, 6, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (33, 6, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (34, 6, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (35, 6, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (36, 6, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (37, 7, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (38, 7, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (39, 7, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (40, 7, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (41, 7, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (42, 7, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (43, 8, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (44, 8, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (45, 8, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (46, 8, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (47, 8, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (48, 8, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (49, 9, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (50, 9, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (51, 9, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (52, 9, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (53, 9, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (54, 9, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (55, 10, CAST(1.0 AS Decimal(3, 1)), CAST(0.00 AS Decimal(5, 2)), CAST(29.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (56, 10, CAST(2.0 AS Decimal(3, 1)), CAST(30.00 AS Decimal(5, 2)), CAST(49.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (57, 10, CAST(3.0 AS Decimal(3, 1)), CAST(50.00 AS Decimal(5, 2)), CAST(69.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (58, 10, CAST(4.0 AS Decimal(3, 1)), CAST(70.00 AS Decimal(5, 2)), CAST(84.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (59, 10, CAST(5.0 AS Decimal(3, 1)), CAST(85.00 AS Decimal(5, 2)), CAST(94.99 AS Decimal(5, 2)))
INSERT [dbo].[ProgiOcen] ([Id], [IdSkaliOcen], [Ocena], [ProgOd], [ProgDo]) VALUES (60, 10, CAST(6.0 AS Decimal(3, 1)), CAST(95.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(5, 2)))
SET IDENTITY_INSERT [dbo].[ProgiOcen] OFF
GO


/* ===== Neutral demo questions and answers ===== */


SET IDENTITY_INSERT [dbo].[Pytania] ON
GO
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (1, N'Which C# type is used to store integer numbers?', 1, 2, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (2, N'Which of the following are programming languages?', 2, 3, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (3, N'What is the OOP mechanism that allows a class to inherit features from another class called?', 3, 3, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (4, N'What does SQL stand for?', 1, 2, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (5, N'Which C# statement is used to execute code conditionally?', 1, 2, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (6, N'What is 7 × 8?', 1, 2, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (7, N'What is the square root of 144?', 3, 3, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (8, N'Which of the following numbers are prime numbers?', 2, 3, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (9, N'What is the sum of the interior angles of a triangle in degrees?', 1, 2, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (10, N'What is 25% of 200?', 3, 2, 2, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (11, N'What is the capital of Spain?', 1, 2, 1, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (12, N'Which of the following objects are planets in the Solar System?', 2, 4, 1, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (13, N'Enter the chemical formula for water.', 3, 3, 1, NULL, 0)
INSERT [dbo].[Pytania] ([Id], [TrescPytania], [TypPytania], [LiczbaPunktow], [IdAutora], [SciezkaZdjecia], [CzyZarchiwizowane]) VALUES (14, N'Which ocean lies between Europe and North America?', 1, 3, 1, NULL, 0)
SET IDENTITY_INSERT [dbo].[Pytania] OFF
GO

SET IDENTITY_INSERT [dbo].[Odpowiedzi] ON
GO
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (1, N'int', 1, 2, 1)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (2, N'double', 0, 0, 1)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (3, N'string', 0, 0, 1)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (4, N'bool', 0, 0, 1)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (5, N'C#', 1, 1, 2)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (6, N'Java', 1, 1, 2)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (7, N'Python', 1, 1, 2)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (8, N'CSS', 0, 0, 2)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (9, N'inheritance', 1, 3, 3)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (10, N'Structured Query Language', 1, 2, 4)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (11, N'Simple Query Logic', 0, 0, 4)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (12, N'System Question Language', 0, 0, 4)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (13, N'Standard Queue List', 0, 0, 4)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (14, N'if', 1, 2, 5)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (15, N'for', 0, 0, 5)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (16, N'using', 0, 0, 5)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (17, N'namespace', 0, 0, 5)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (18, N'56', 1, 2, 6)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (19, N'48', 0, 0, 6)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (20, N'54', 0, 0, 6)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (21, N'64', 0, 0, 6)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (22, N'12', 1, 3, 7)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (23, N'2', 1, 1, 8)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (24, N'3', 1, 1, 8)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (25, N'5', 1, 1, 8)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (26, N'9', 0, 0, 8)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (27, N'180', 1, 2, 9)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (28, N'90', 0, 0, 9)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (29, N'270', 0, 0, 9)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (30, N'360', 0, 0, 9)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (31, N'50', 1, 2, 10)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (32, N'Madrid', 1, 2, 11)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (33, N'Barcelona', 0, 0, 11)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (34, N'Valencia', 0, 0, 11)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (35, N'Seville', 0, 0, 11)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (36, N'Mars', 1, 2, 12)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (37, N'Venus', 1, 2, 12)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (38, N'Moon', 0, 0, 12)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (39, N'Sun', 0, 0, 12)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (40, N'H2O', 1, 3, 13)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (41, N'Atlantic Ocean', 1, 3, 14)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (42, N'Indian Ocean', 0, 0, 14)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (43, N'Pacific Ocean', 0, 0, 14)
INSERT [dbo].[Odpowiedzi] ([Id], [TrescOdpowiedzi], [CzyPoprawna], [LiczbaPunktow], [PytanieId]) VALUES (44, N'Arctic Ocean', 0, 0, 14)
SET IDENTITY_INSERT [dbo].[Odpowiedzi] OFF
GO


/* ===== Neutral demo tests ===== */


SET IDENTITY_INSERT [dbo].[Testy] ON
GO
INSERT [dbo].[Testy] ([Id], [Tytul], [Opis], [ProgZdania], [DataUtworzenia], [CzasTrwaniaMinuty], [IdAutora], [IdTypuTestu], [LosujKolejnoscOdpowiedzi], [LosujKolejnoscPytan], [CzyZarchiwizowany]) VALUES (1, N'Computer Science Basics', N'Demo test covering programming and database fundamentals.', 50, SYSDATETIME(), 15, 2, 3, 1, 1, 0)
INSERT [dbo].[Testy] ([Id], [Tytul], [Opis], [ProgZdania], [DataUtworzenia], [CzasTrwaniaMinuty], [IdAutora], [IdTypuTestu], [LosujKolejnoscOdpowiedzi], [LosujKolejnoscPytan], [CzyZarchiwizowany]) VALUES (2, N'Mathematics - Basics', N'Demo test covering basic mathematical operations and concepts.', 50, SYSDATETIME(), 15, 2, 1, 1, 1, 0)
INSERT [dbo].[Testy] ([Id], [Tytul], [Opis], [ProgZdania], [DataUtworzenia], [CzasTrwaniaMinuty], [IdAutora], [IdTypuTestu], [LosujKolejnoscOdpowiedzi], [LosujKolejnoscPytan], [CzyZarchiwizowany]) VALUES (3, N'General Knowledge - Demo', N'Short demo test showing different question types.', 50, SYSDATETIME(), 10, 1, 10, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[Testy] OFF
GO

SET IDENTITY_INSERT [dbo].[TestPytania] ON
GO
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (1, 1, 1, 1)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (2, 1, 2, 2)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (3, 1, 3, 3)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (4, 1, 4, 4)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (5, 1, 5, 5)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (6, 2, 6, 1)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (7, 2, 7, 2)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (8, 2, 8, 3)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (9, 2, 9, 4)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (10, 2, 10, 5)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (11, 3, 11, 1)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (12, 3, 12, 2)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (13, 3, 13, 3)
INSERT [dbo].[TestPytania] ([Id], [TestId], [PytanieId], [NumerKolejnosci]) VALUES (14, 3, 14, 4)
SET IDENTITY_INSERT [dbo].[TestPytania] OFF
GO


/* ===== Demo assignments and one sample completed result ===== */


SET IDENTITY_INSERT [dbo].[PrzypisaneTesty] ON
GO
INSERT [dbo].[PrzypisaneTesty] ([Id], [TestId], [UczenId], [NauczycielId], [DataPrzypisania], [DataDostepnosci], [DataWygasniecia], [Status], [CzyPokazacWynikPoZakonczeniu], [SposobWyswietlaniaWyniku], [InformacjaKoncowa], [InformacjaStartowa], [LiczbaProb], [SposobOceniania], [KlasaId]) VALUES (1, 1, 3, 2, DATEADD(day,-1,SYSDATETIME()), DATEADD(day,-1,SYSDATETIME()), DATEADD(day,30,SYSDATETIME()), 1, 1, 2, N'Thank you for completing the test.', N'Good luck! Read each question carefully.', 2, 3, 19)
INSERT [dbo].[PrzypisaneTesty] ([Id], [TestId], [UczenId], [NauczycielId], [DataPrzypisania], [DataDostepnosci], [DataWygasniecia], [Status], [CzyPokazacWynikPoZakonczeniu], [SposobWyswietlaniaWyniku], [InformacjaKoncowa], [InformacjaStartowa], [LiczbaProb], [SposobOceniania], [KlasaId]) VALUES (2, 2, 3, 2, SYSDATETIME(), DATEADD(day,7,SYSDATETIME()), DATEADD(day,37,SYSDATETIME()), 1, 1, 2, N'The result will be available after the test is completed.', N'The test will be available from the specified date.', 1, 3, 19)
INSERT [dbo].[PrzypisaneTesty] ([Id], [TestId], [UczenId], [NauczycielId], [DataPrzypisania], [DataDostepnosci], [DataWygasniecia], [Status], [CzyPokazacWynikPoZakonczeniu], [SposobWyswietlaniaWyniku], [InformacjaKoncowa], [InformacjaStartowa], [LiczbaProb], [SposobOceniania], [KlasaId]) VALUES (3, 3, 3, 1, DATEADD(day,-10,SYSDATETIME()), DATEADD(day,-9,SYSDATETIME()), DATEADD(day,20,SYSDATETIME()), 3, 1, 2, N'The demo test has been completed.', N'Sample general knowledge test.', 1, 4, 19)
SET IDENTITY_INSERT [dbo].[PrzypisaneTesty] OFF
GO

SET IDENTITY_INSERT [dbo].[WynikiTestow] ON
GO
INSERT [dbo].[WynikiTestow] ([Id], [UzytkownikId], [PrzypisanyTestId], [LiczbaPunktow], [Procent], [Ocena], [CzyZdane], [DataRozpoczecia], [DataZakonczenia], [CzasRozwiazywaniaMinuty], [MaksymalnaLiczbaPunktow], [CzyProbny]) VALUES (1, 3, 3, 10, CAST(83.33 AS Decimal(5,2)), CAST(4.0 AS Decimal(3,1)), 1, DATEADD(day,-8,SYSDATETIME()), DATEADD(minute,7,DATEADD(day,-8,SYSDATETIME())), 7, 12, 0)
SET IDENTITY_INSERT [dbo].[WynikiTestow] OFF
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Klasy_NazwaKlasy_RokSzkolny]    Script Date: 05/09/2026 19:39:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Klasy_NazwaKlasy_RokSzkolny] ON [dbo].[Klasy]
(
	[NazwaKlasy] ASC,
	[RokSzkolny] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LogiLogowan_UzytkownikId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_LogiLogowan_UzytkownikId] ON [dbo].[LogiLogowan]
(
	[UzytkownikId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LogiRozwiazywaniaTestu_PrzypisanyTestId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_LogiRozwiazywaniaTestu_PrzypisanyTestId] ON [dbo].[LogiRozwiazywaniaTestu]
(
	[PrzypisanyTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LogiRozwiazywaniaTestu_UzytkownikId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_LogiRozwiazywaniaTestu_UzytkownikId] ON [dbo].[LogiRozwiazywaniaTestu]
(
	[UzytkownikId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Odpowiedzi_PytanieId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_Odpowiedzi_PytanieId] ON [dbo].[Odpowiedzi]
(
	[PytanieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProgiOcen_IdSkaliOcen_ProgOd_ProgDo]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_ProgiOcen_IdSkaliOcen_ProgOd_ProgDo] ON [dbo].[ProgiOcen]
(
	[IdSkaliOcen] ASC,
	[ProgOd] ASC,
	[ProgDo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PrzypisaneTesty_KlasaId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_PrzypisaneTesty_KlasaId] ON [dbo].[PrzypisaneTesty]
(
	[KlasaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PrzypisaneTesty_NauczycielId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_PrzypisaneTesty_NauczycielId] ON [dbo].[PrzypisaneTesty]
(
	[NauczycielId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PrzypisaneTesty_TestId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_PrzypisaneTesty_TestId] ON [dbo].[PrzypisaneTesty]
(
	[TestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PrzypisaneTesty_UczenId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_PrzypisaneTesty_UczenId] ON [dbo].[PrzypisaneTesty]
(
	[UczenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pytania_IdAutora]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_Pytania_IdAutora] ON [dbo].[Pytania]
(
	[IdAutora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RozwiazaneOdpowiedzi_IdRozwiazanegoPytania]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_RozwiazaneOdpowiedzi_IdRozwiazanegoPytania] ON [dbo].[RozwiazaneOdpowiedzi]
(
	[IdRozwiazanegoPytania] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RozwiazanePytania_IdWynikuTestu_Kolejnosc]    Script Date: 05/09/2026 19:39:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_RozwiazanePytania_IdWynikuTestu_Kolejnosc] ON [dbo].[RozwiazanePytania]
(
	[IdWynikuTestu] ASC,
	[Kolejnosc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SkaleOcen_IdTypuTestu_IdUzytkownika_CzyAktywna]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_SkaleOcen_IdTypuTestu_IdUzytkownika_CzyAktywna] ON [dbo].[SkaleOcen]
(
	[IdTypuTestu] ASC,
	[IdUzytkownika] ASC,
	[CzyAktywna] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SkaleOcen_IdUzytkownika]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_SkaleOcen_IdUzytkownika] ON [dbo].[SkaleOcen]
(
	[IdUzytkownika] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TestPytania_PytanieId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_TestPytania_PytanieId] ON [dbo].[TestPytania]
(
	[PytanieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TestPytania_TestId_PytanieId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TestPytania_TestId_PytanieId] ON [dbo].[TestPytania]
(
	[TestId] ASC,
	[PytanieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Testy_IdAutora]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_Testy_IdAutora] ON [dbo].[Testy]
(
	[IdAutora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Testy_IdTypuTestu]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_Testy_IdTypuTestu] ON [dbo].[Testy]
(
	[IdTypuTestu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TypyTestow_NazwaTypu]    Script Date: 05/09/2026 19:39:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TypyTestow_NazwaTypu] ON [dbo].[TypyTestow]
(
	[NazwaTypu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Uzytkownicy_KlasaId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_Uzytkownicy_KlasaId] ON [dbo].[Uzytkownicy]
(
	[KlasaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Uzytkownicy_RolaId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_Uzytkownicy_RolaId] ON [dbo].[Uzytkownicy]
(
	[RolaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_WynikiTestow_PrzypisanyTestId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_WynikiTestow_PrzypisanyTestId] ON [dbo].[WynikiTestow]
(
	[PrzypisanyTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_WynikiTestow_UzytkownikId]    Script Date: 05/09/2026 19:39:50 ******/
CREATE NONCLUSTERED INDEX [IX_WynikiTestow_UzytkownikId] ON [dbo].[WynikiTestow]
(
	[UzytkownikId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LogiLogowan] ADD  DEFAULT ((0)) FOR [Sesja]
GO
ALTER TABLE [dbo].[PrzypisaneTesty] ADD  DEFAULT (CONVERT([bit],(0))) FOR [CzyPokazacWynikPoZakonczeniu]
GO
ALTER TABLE [dbo].[PrzypisaneTesty] ADD  DEFAULT ((0)) FOR [SposobWyswietlaniaWyniku]
GO
ALTER TABLE [dbo].[PrzypisaneTesty] ADD  DEFAULT ((1)) FOR [LiczbaProb]
GO
ALTER TABLE [dbo].[PrzypisaneTesty] ADD  DEFAULT ((0)) FOR [SposobOceniania]
GO
ALTER TABLE [dbo].[Pytania] ADD  DEFAULT (CONVERT([bit],(0))) FOR [CzyZarchiwizowane]
GO
ALTER TABLE [dbo].[SkaleOcen] ADD  DEFAULT (CONVERT([bit],(1))) FOR [CzyAktywna]
GO
ALTER TABLE [dbo].[Testy] ADD  DEFAULT ((0)) FOR [IdTypuTestu]
GO
ALTER TABLE [dbo].[Testy] ADD  DEFAULT (CONVERT([bit],(0))) FOR [LosujKolejnoscOdpowiedzi]
GO
ALTER TABLE [dbo].[Testy] ADD  DEFAULT (CONVERT([bit],(0))) FOR [LosujKolejnoscPytan]
GO
ALTER TABLE [dbo].[Testy] ADD  DEFAULT (CONVERT([bit],(0))) FOR [CzyZarchiwizowany]
GO
ALTER TABLE [dbo].[WynikiTestow] ADD  DEFAULT ((0)) FOR [MaksymalnaLiczbaPunktow]
GO
ALTER TABLE [dbo].[LogiLogowan]  WITH CHECK ADD  CONSTRAINT [FK_LogiLogowan_Uzytkownicy_UzytkownikId] FOREIGN KEY([UzytkownikId])
REFERENCES [dbo].[Uzytkownicy] ([Id])
GO
ALTER TABLE [dbo].[LogiLogowan] CHECK CONSTRAINT [FK_LogiLogowan_Uzytkownicy_UzytkownikId]
GO
ALTER TABLE [dbo].[LogiRozwiazywaniaTestu]  WITH CHECK ADD  CONSTRAINT [FK_LogiRozwiazywaniaTestu_PrzypisaneTesty_PrzypisanyTestId] FOREIGN KEY([PrzypisanyTestId])
REFERENCES [dbo].[PrzypisaneTesty] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogiRozwiazywaniaTestu] CHECK CONSTRAINT [FK_LogiRozwiazywaniaTestu_PrzypisaneTesty_PrzypisanyTestId]
GO
ALTER TABLE [dbo].[LogiRozwiazywaniaTestu]  WITH CHECK ADD  CONSTRAINT [FK_LogiRozwiazywaniaTestu_Uzytkownicy_UzytkownikId] FOREIGN KEY([UzytkownikId])
REFERENCES [dbo].[Uzytkownicy] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogiRozwiazywaniaTestu] CHECK CONSTRAINT [FK_LogiRozwiazywaniaTestu_Uzytkownicy_UzytkownikId]
GO
ALTER TABLE [dbo].[Odpowiedzi]  WITH CHECK ADD  CONSTRAINT [FK_Odpowiedzi_Pytania_PytanieId] FOREIGN KEY([PytanieId])
REFERENCES [dbo].[Pytania] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Odpowiedzi] CHECK CONSTRAINT [FK_Odpowiedzi_Pytania_PytanieId]
GO
ALTER TABLE [dbo].[ProgiOcen]  WITH CHECK ADD  CONSTRAINT [FK_ProgiOcen_SkaleOcen_IdSkaliOcen] FOREIGN KEY([IdSkaliOcen])
REFERENCES [dbo].[SkaleOcen] ([Id])
GO
ALTER TABLE [dbo].[ProgiOcen] CHECK CONSTRAINT [FK_ProgiOcen_SkaleOcen_IdSkaliOcen]
GO
ALTER TABLE [dbo].[PrzypisaneTesty]  WITH CHECK ADD  CONSTRAINT [FK_PrzypisaneTesty_Klasy_KlasaId] FOREIGN KEY([KlasaId])
REFERENCES [dbo].[Klasy] ([Id])
GO
ALTER TABLE [dbo].[PrzypisaneTesty] CHECK CONSTRAINT [FK_PrzypisaneTesty_Klasy_KlasaId]
GO
ALTER TABLE [dbo].[PrzypisaneTesty]  WITH CHECK ADD  CONSTRAINT [FK_PrzypisaneTesty_Testy_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[Testy] ([Id])
GO
ALTER TABLE [dbo].[PrzypisaneTesty] CHECK CONSTRAINT [FK_PrzypisaneTesty_Testy_TestId]
GO
ALTER TABLE [dbo].[PrzypisaneTesty]  WITH CHECK ADD  CONSTRAINT [FK_PrzypisaneTesty_Uzytkownicy_NauczycielId] FOREIGN KEY([NauczycielId])
REFERENCES [dbo].[Uzytkownicy] ([Id])
GO
ALTER TABLE [dbo].[PrzypisaneTesty] CHECK CONSTRAINT [FK_PrzypisaneTesty_Uzytkownicy_NauczycielId]
GO
ALTER TABLE [dbo].[PrzypisaneTesty]  WITH CHECK ADD  CONSTRAINT [FK_PrzypisaneTesty_Uzytkownicy_UczenId] FOREIGN KEY([UczenId])
REFERENCES [dbo].[Uzytkownicy] ([Id])
GO
ALTER TABLE [dbo].[PrzypisaneTesty] CHECK CONSTRAINT [FK_PrzypisaneTesty_Uzytkownicy_UczenId]
GO
ALTER TABLE [dbo].[Pytania]  WITH CHECK ADD  CONSTRAINT [FK_Pytania_Uzytkownicy_IdAutora] FOREIGN KEY([IdAutora])
REFERENCES [dbo].[Uzytkownicy] ([Id])
GO
ALTER TABLE [dbo].[Pytania] CHECK CONSTRAINT [FK_Pytania_Uzytkownicy_IdAutora]
GO
ALTER TABLE [dbo].[RozwiazaneOdpowiedzi]  WITH CHECK ADD  CONSTRAINT [FK_RozwiazaneOdpowiedzi_RozwiazanePytania_IdRozwiazanegoPytania] FOREIGN KEY([IdRozwiazanegoPytania])
REFERENCES [dbo].[RozwiazanePytania] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RozwiazaneOdpowiedzi] CHECK CONSTRAINT [FK_RozwiazaneOdpowiedzi_RozwiazanePytania_IdRozwiazanegoPytania]
GO
ALTER TABLE [dbo].[RozwiazanePytania]  WITH CHECK ADD  CONSTRAINT [FK_RozwiazanePytania_WynikiTestow_IdWynikuTestu] FOREIGN KEY([IdWynikuTestu])
REFERENCES [dbo].[WynikiTestow] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RozwiazanePytania] CHECK CONSTRAINT [FK_RozwiazanePytania_WynikiTestow_IdWynikuTestu]
GO
ALTER TABLE [dbo].[SkaleOcen]  WITH CHECK ADD  CONSTRAINT [FK_SkaleOcen_TypyTestow_IdTypuTestu] FOREIGN KEY([IdTypuTestu])
REFERENCES [dbo].[TypyTestow] ([Id])
GO
ALTER TABLE [dbo].[SkaleOcen] CHECK CONSTRAINT [FK_SkaleOcen_TypyTestow_IdTypuTestu]
GO
ALTER TABLE [dbo].[SkaleOcen]  WITH CHECK ADD  CONSTRAINT [FK_SkaleOcen_Uzytkownicy_IdUzytkownika] FOREIGN KEY([IdUzytkownika])
REFERENCES [dbo].[Uzytkownicy] ([Id])
GO
ALTER TABLE [dbo].[SkaleOcen] CHECK CONSTRAINT [FK_SkaleOcen_Uzytkownicy_IdUzytkownika]
GO
ALTER TABLE [dbo].[TestPytania]  WITH CHECK ADD  CONSTRAINT [FK_TestPytania_Pytania_PytanieId] FOREIGN KEY([PytanieId])
REFERENCES [dbo].[Pytania] ([Id])
GO
ALTER TABLE [dbo].[TestPytania] CHECK CONSTRAINT [FK_TestPytania_Pytania_PytanieId]
GO
ALTER TABLE [dbo].[TestPytania]  WITH CHECK ADD  CONSTRAINT [FK_TestPytania_Testy_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[Testy] ([Id])
GO
ALTER TABLE [dbo].[TestPytania] CHECK CONSTRAINT [FK_TestPytania_Testy_TestId]
GO
ALTER TABLE [dbo].[Testy]  WITH CHECK ADD  CONSTRAINT [FK_Testy_TypyTestow_IdTypuTestu] FOREIGN KEY([IdTypuTestu])
REFERENCES [dbo].[TypyTestow] ([Id])
GO
ALTER TABLE [dbo].[Testy] CHECK CONSTRAINT [FK_Testy_TypyTestow_IdTypuTestu]
GO
ALTER TABLE [dbo].[Testy]  WITH CHECK ADD  CONSTRAINT [FK_Testy_Uzytkownicy_IdAutora] FOREIGN KEY([IdAutora])
REFERENCES [dbo].[Uzytkownicy] ([Id])
GO
ALTER TABLE [dbo].[Testy] CHECK CONSTRAINT [FK_Testy_Uzytkownicy_IdAutora]
GO
ALTER TABLE [dbo].[Uzytkownicy]  WITH CHECK ADD  CONSTRAINT [FK_Uzytkownicy_Klasy_KlasaId] FOREIGN KEY([KlasaId])
REFERENCES [dbo].[Klasy] ([Id])
GO
ALTER TABLE [dbo].[Uzytkownicy] CHECK CONSTRAINT [FK_Uzytkownicy_Klasy_KlasaId]
GO
ALTER TABLE [dbo].[Uzytkownicy]  WITH CHECK ADD  CONSTRAINT [FK_Uzytkownicy_Role_RolaId] FOREIGN KEY([RolaId])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Uzytkownicy] CHECK CONSTRAINT [FK_Uzytkownicy_Role_RolaId]
GO
ALTER TABLE [dbo].[WynikiTestow]  WITH CHECK ADD  CONSTRAINT [FK_WynikiTestow_PrzypisaneTesty_PrzypisanyTestId] FOREIGN KEY([PrzypisanyTestId])
REFERENCES [dbo].[PrzypisaneTesty] ([Id])
GO
ALTER TABLE [dbo].[WynikiTestow] CHECK CONSTRAINT [FK_WynikiTestow_PrzypisaneTesty_PrzypisanyTestId]
GO
ALTER TABLE [dbo].[WynikiTestow]  WITH CHECK ADD  CONSTRAINT [FK_WynikiTestow_Uzytkownicy_UzytkownikId] FOREIGN KEY([UzytkownikId])
REFERENCES [dbo].[Uzytkownicy] ([Id])
GO
ALTER TABLE [dbo].[WynikiTestow] CHECK CONSTRAINT [FK_WynikiTestow_Uzytkownicy_UzytkownikId]
GO
USE [master]
GO
ALTER DATABASE [EgzaminyTestyDb_EN] SET  READ_WRITE 
GO
