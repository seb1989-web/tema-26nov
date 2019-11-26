namespace LinqAndLamdaExpressions
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var allUsers = ReadUsers("users.json");
            var allPosts = ReadPosts("posts.json");

            // 1 - find all users having email ending with ".net".
            var ex1 = allUsers.Where(email => email.Email.EndsWith(".net"));
            foreach(var user in ex1)
            {
                Console.WriteLine($"user cu email .net {user.Name}");
            }
            Console.WriteLine();
            // 2 - find all posts for users having email ending with ".net".
            var ex2 = from u in allUsers
                      join p in allPosts
                      on u.Id equals p.UserId
                      where u.Email.EndsWith(".net")
                      select new
                      {
                          u.Email,
                          u.Name,
                          p.UserId,
                          p.Body
                      };

            foreach (var item in ex2)
            {
                Console.WriteLine($"{item.Name} {item.Email} {item.Body}");
            }

            Console.WriteLine();
            // 3 - print number of posts for each user.


            var ex3 = from p in allPosts
                      group p by p.UserId into grp
                      select new 
                      {
                          user = grp.Key,
                          count = grp.Count()
                      };
            foreach (var item in ex3)
            {
                Console.WriteLine($"user {item.user} has {item.count} posts.");
            }

            Console.WriteLine();

            // 4 - find all users that have lat and long negative.

            var ex4 = allUsers.Where(u => u.Address.Geo.Lat < 0 && u.Address.Geo.Lng < 0);
            foreach (var user in ex4)
            {
                Console.WriteLine($"{user.Name} {user.Address.Geo.Lat} {user.Address.Geo.Lng}");
            }

            Console.WriteLine();
            // 5 - find the post with longest body.
            var ex5 = allPosts.OrderByDescending(l => l.Body.Length).First();
            Console.WriteLine($"{ex5.Body}");

            Console.WriteLine();
            // 6 - print the name of the employee that have post with longest body.

            var ex6 = from u in allUsers
                      join p in allPosts
                      on u.Id equals p.UserId
                      where p.Body.Length == allPosts.Max(a => a.Body.Length)
                      select u.Name;

            foreach (var item in ex6)
            {
                Console.WriteLine($"name of employee with longest body is {item} ");
            }




            Console.WriteLine();
            // 7 - select all addresses in a new List<Address>. print the list.
            List<Address> lista = allUsers.Select(a => a.Address).ToList();
            foreach (var item in lista)
            {
                Console.WriteLine($"{item.City} {item.Geo.Lat} {item.Street}");
            }
            Console.WriteLine();
            // 8 - print the user with min lat
            var ex8 = from u in allUsers
                      where u.Address.Geo.Lat == allUsers.Min(a => a.Address.Geo.Lat)
                      select u.Name;
            foreach (var item in ex8)
            {
                Console.WriteLine($"the user with min lat is {item}");
            }

            Console.WriteLine();
            // 9 - print the user with max long

            var ex9 = from u in allUsers
                      where u.Address.Geo.Lng == allUsers.Max(a => a.Address.Geo.Lng)
                      select u.Name;
            foreach (var item in ex9)
            {
                Console.WriteLine($"the user with max long is {item}");
            }


            // 10 - create a new class: public class UserPosts { public User User {get; set}; public List<Post> Posts {get; set} }
            //    - create a new list: List<UserPosts>
            //    - insert in this list each user with his posts only

            Console.WriteLine();

            // 12 - order users by number of posts
            var ex12 = from u in allUsers
                       join p in allPosts
                       on u.Id equals p.UserId
                       group p by u.Name into grp
                       orderby grp.Count()
                       select new 
                       {
                           name = grp.Key,
                           count = grp.Count()
                       };


            foreach (var item in ex12)
            {
                Console.WriteLine($"{item.name} has {item.count} posts");
            }


            Console.ReadLine();
            
        }

        private static List<Post> ReadPosts(string file)
        {
            return ReadData.ReadFrom<Post>(file);
        }

        private static List<User> ReadUsers(string file)
        {
            return ReadData.ReadFrom<User>(file);
        }
    }
}
