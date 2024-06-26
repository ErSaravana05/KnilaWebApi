USE [KnilaBook]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 10-05-2024 22:09:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book](
	[Publisher] [varchar](50) NULL,
	[Title] [varchar](50) NULL,
	[AuthorLastName] [varchar](50) NULL,
	[AuthorFirstName] [varchar](50) NULL,
	[Price] [numeric](18, 0) NULL,
	[SNO] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[SNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Book] ON 

INSERT [dbo].[Book] ([Publisher], [Title], [AuthorLastName], [AuthorFirstName], [Price], [SNO]) VALUES (N'Saravanan', N'Comrade', N'M', N'Saravanan', CAST(50 AS Numeric(18, 0)), 1)
SET IDENTITY_INSERT [dbo].[Book] OFF
GO
/****** Object:  StoredProcedure [dbo].[GetPublisherAuthor]    Script Date: 10-05-2024 22:10:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPublisherAuthor] 
(
@Type varchar(5)

)
AS
BEGIN
if(@Type='1')
BEGIN
SELECT Publisher,AuthorFirstName + AuthorLastName as AuthornName,Title  from Book order by Publisher,AuthornName,Title desc
END
else if(@Type='2')
BEGIN
SELECT AuthorFirstName + AuthorLastName as AuthornName,Title  from Book order by Publisher,AuthornName,Title desc

END

END
GO
