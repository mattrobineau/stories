using System;

namespace Stories.Enums
{
    [Flags]
    public enum Permissions
    {
        None = 0,

        // Story permissions
        DeleteStory = 1 << 1,
        CreateStory = 1 << 2,
        EditStory = 1 << 3,

        // Comment permissions
        DeleteComment = 1 << 4,
        CreateComment = 1 << 5,
        EditComment = 1 << 6,

        // User commands
        BanUser = 1 << 7,
        DeleteUser = 1 << 8,
        ModifyUser = 1 << 9
    }
}
