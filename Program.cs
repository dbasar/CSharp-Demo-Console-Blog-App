// SIMPLE CONSOLE BLOG APP BY DORUK
// No database is used. Blog examples are stored in a collection.

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class Post
{
    private readonly string _title;
    private readonly string _content;
    private readonly DateTime _postTime = DateTime.Now;
    private int _votes = 0;
    private static int _idCount = 0;
    private readonly int _id = ++_idCount;

    public Post(string title, string content)
    {
        _title = title;
        _content = content;
    }
    public void Vote(int vote)
    {
        _votes += vote;
    }
    public string GetVotes() => _votes.ToString();
    public string GetID() => _id.ToString();
    public string GetTitle() => _title;
    public string GetContent() => _content;
    public string GetDate() => _postTime.ToString();
}
public class PostsManagement
{
    // A class to provide static methods for managing the posts

    public static void List(List<Post> list)
    {
        Print.Heavy($"Number of posts: {list.Count}.", true);

        if (list.Count != 0)
        {
            Print.Heavy("Posts:", true);
            for (int i = 0; i < list.Count; i++)
            {
                Print.Heavy($"{i + 1} - \t\t");
                Print.Light($"ID: #{list.ElementAt(i).GetID()} | Title: {list.ElementAt(i).GetTitle()}", true);
            }
        }        
    }
    public static Post Find(List<Post> list, string id)
    {
        // Check if the user provided the ID with a hashtag and remove it if so
        if (id.StartsWith('#'))
            id = id.Substring(1);

        // Return either the post or null
        return list.Find(post => post.GetID() == id);
    }
    public static void Delete(List<Post> list, string id)
    {
        var post = Find(list, id);

        if (post != null)
        {            
            list.Remove(post);
            Console.WriteLine("Post with the ID #{0} is deleted.", id);
        }
        else
            Console.WriteLine("Post with the specified ID is not found.");
    }
}

public class Print
{
    // Includes static methods to replace Console methods, with the purpose of providing text coloring options

    private static ConsoleColor _heavyColor = ConsoleColor.DarkCyan;
    private static ConsoleColor _lightColor = ConsoleColor.Gray;

    // These getter/setter properties are here just to spice up the class
    public ConsoleColor HeavyColor { get { return _heavyColor; } set { _heavyColor = value; } }
    public ConsoleColor LightColor { get { return _lightColor; } set { _lightColor = value; } }

    public static void Heavy(string text)
    {
        Console.ForegroundColor = _heavyColor;
        Console.Write(text);
    }
    public static void Heavy(string text, bool newLine)
    {
        Console.ForegroundColor = _heavyColor;
        Console.Write(text);
        if (newLine) Console.WriteLine();
    }
    public static void Light(string text)
    {
        Console.ForegroundColor = _lightColor;
        Console.Write(text);
    }
    public static void Light(string text, bool newLine)
    {
        Console.ForegroundColor = _lightColor;
        Console.Write(text);
        if (newLine) Console.WriteLine();
    }
    public static string ReadLine()
    {
        Console.ForegroundColor = _lightColor;
        return Console.ReadLine();
    }
    public static void NewLine()  // Just for the sake of completeness
    {
        Console.WriteLine();
    }
    public static void ResetColor()  // Again, for completeness
    {
        Console.ResetColor();
    }
}

public class Program
{
    static void Main()
    {
        Print.Heavy("Posting app by Doruk", true);
        Print.NewLine();

        // The post list to contain posts like a database
        var PostList = new List<Post>
        {
            // Two exemplary posts.
            new Post("Lorem ipsum.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."),
            new Post("Lorem ipsum dolor sit amet.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.")
        };

        while (true)
        {
            // Main menu 

            Print.Heavy("Enter a command or the ID of the post you'd like to view (ID, 'esc', 'list', 'add', 'delete'): ");
            string input = Print.ReadLine().Trim().ToLower();
            Print.NewLine();

            if (input == "esc")
                break;

            else if (input == "list")
            {
                PostsManagement.List(PostList);
            }

            else if (input == "delete")
            {
                while (PostList.Count != 0)
                {
                    // Post deleting menu

                    Print.Heavy("You are currently in the process of post deleting.", true);
                    Print.Heavy("Enter the ID of the post you wish to delete ('esc': exit): ", true);
                    input = Print.ReadLine().Trim().ToLower();

                    if (input == "esc")
                        break;

                    PostsManagement.Delete(PostList, input);
                    Print.NewLine();
                }

                if (PostList.Count == 0)
                    Print.Heavy("No posts to delete.", true);
                if (input == "esc" ||
                    (PostList.Count == 0 && input != "delete"))
                    Print.Heavy("Exited the deleting process.", true);
            }

            else if (input == "add")
            {
                // Post adding menu

                Print.Heavy("You are adding a post.", true);
                Print.Heavy("Type the title of the post ('esc': exit):", true);                
                input = Print.ReadLine().Trim();

                if (input.ToLower() != "esc")
                {
                    string title = input;

                    Print.Heavy("Type the content of the post ('esc': exit):", true);
                    input = Print.ReadLine().Trim();

                    if (input.ToLower() != "esc")
                    {
                        string content = input;

                        var post = new Post(title, content);
                        PostList.Add(post);
                    }
                }
                 
                if (input.ToLower() != "esc")
                    Print.Heavy("Post added successfully.", true);
                else
                    Print.Heavy("Post adding process cancelled.", true);
            }
            
            else
            {
                // Post ID query

                var post = PostsManagement.Find(PostList, input);

                if (post != null)
                {
                    // Post found

                    while (true)
                    {
                        Print.Heavy($"You are viewing the post #{post.GetID()}", true);
                        Print.Heavy("Enter a command ('view', 'upvote', 'downvote', 'delete', 'esc'): ");
                        input = Print.ReadLine().Trim().ToLower();
                        Print.NewLine();

                        if (input == "esc")
                        {
                            break;
                        }

                        else if (input == "delete")
                        {
                            PostsManagement.Delete(PostList, post.GetID());
                            break;
                        }

                        else if (input == "upvote")
                        {
                            post.Vote(1);
                            Print.Heavy("Post upvoted.");
                        }

                        else if (input == "downvote")
                        {
                            post.Vote(-1);
                            Print.Heavy("Post downvoted.");
                        }

                        else if (input == "view")
                        {
                            Print.Heavy("Post ID: \t");
                            Print.Light(post.GetID(), true);

                            Print.Heavy("Title: \t\t");
                            Print.Light(post.GetTitle(), true);

                            Print.Heavy("Date: \t\t");
                            Print.Light(post.GetDate(), true);

                            Print.Heavy("Votes: \t\t");
                            Print.Light(post.GetVotes(), true);

                            Print.Heavy("Content: \t");
                            Print.Light(post.GetContent(), true);
                        }

                        else
                        {
                            Print.Heavy("Invalid command.", true);
                        }

                        Print.NewLine();
                    }
                }

                else
                {
                    // Post not found or invalid command
                    Print.Heavy("Invalid command.", true);
                }
            }

            Print.NewLine();
        }

        // Exited the app
        Print.Heavy("You have exited the app.", true);

        Print.ResetColor();
    }
}
