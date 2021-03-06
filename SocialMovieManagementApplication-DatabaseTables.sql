USE [SocialMovieManagement]
GO
/****** Object:  Table [dbo].[Collections]    Script Date: 7/18/2021 3:27:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collections](
	[collection_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[collection_data] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[collection_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FriendRequests]    Script Date: 7/18/2021 3:27:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendRequests](
	[requestID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NOT NULL,
	[friendID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[requestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Friends]    Script Date: 7/18/2021 3:27:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friends](
	[user_id] [int] NOT NULL,
	[friend_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SocialComments]    Script Date: 7/18/2021 3:27:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SocialComments](
	[commentID] [int] IDENTITY(1,1) NOT NULL,
	[postID] [int] NOT NULL,
	[commentText] [text] NOT NULL,
	[postedBy] [int] NOT NULL,
	[postedDate] [datetime] NOT NULL,
	[commentUsername] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[commentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SocialPosts]    Script Date: 7/18/2021 3:27:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SocialPosts](
	[postID] [int] IDENTITY(1,1) NOT NULL,
	[postedBy] [int] NOT NULL,
	[postedUsername] [varchar](50) NOT NULL,
	[postContent] [text] NOT NULL,
	[datePosted] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[postID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 7/18/2021 3:27:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfiles](
	[profile_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[country] [nvarchar](100) NOT NULL,
	[age] [int] NOT NULL,
	[favorite_movie] [text] NULL,
	[user_bio] [nvarchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[profile_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/18/2021 3:27:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [nvarchar](50) NOT NULL,
	[last_name] [nvarchar](50) NOT NULL,
	[email_address] [nvarchar](100) NOT NULL,
	[username] [nvarchar](100) NOT NULL,
	[password] [nvarchar](100) NOT NULL,
	[banned] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Collections]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Collections] CHECK CONSTRAINT [FK_UserProfile]
GO
ALTER TABLE [dbo].[FriendRequests]  WITH CHECK ADD  CONSTRAINT [FK_RequestFriendID] FOREIGN KEY([friendID])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[FriendRequests] CHECK CONSTRAINT [FK_RequestFriendID]
GO
ALTER TABLE [dbo].[FriendRequests]  WITH CHECK ADD  CONSTRAINT [FK_RequestUserID] FOREIGN KEY([userID])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[FriendRequests] CHECK CONSTRAINT [FK_RequestUserID]
GO
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_FriendID] FOREIGN KEY([friend_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_FriendID]
GO
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_UserID] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_UserID]
GO
ALTER TABLE [dbo].[SocialComments]  WITH CHECK ADD  CONSTRAINT [FK_CommentedBy] FOREIGN KEY([postedBy])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[SocialComments] CHECK CONSTRAINT [FK_CommentedBy]
GO
ALTER TABLE [dbo].[SocialComments]  WITH CHECK ADD  CONSTRAINT [FK_PostID] FOREIGN KEY([postID])
REFERENCES [dbo].[SocialPosts] ([postID])
GO
ALTER TABLE [dbo].[SocialComments] CHECK CONSTRAINT [FK_PostID]
GO
ALTER TABLE [dbo].[SocialPosts]  WITH CHECK ADD  CONSTRAINT [FK_PostedBy] FOREIGN KEY([postedBy])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[SocialPosts] CHECK CONSTRAINT [FK_PostedBy]
GO
ALTER TABLE [dbo].[UserProfiles]  WITH CHECK ADD  CONSTRAINT [FK_ProfileUser] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[UserProfiles] CHECK CONSTRAINT [FK_ProfileUser]
GO
