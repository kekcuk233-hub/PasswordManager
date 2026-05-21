using PasswordManager.Models.Base;
using PasswordManager.Services;
using PasswordManager.Models.UserData;
using PasswordManager.Core;

namespace PasswordManager.Core
{
    public class AppRunner(DependencyContainer container)
    {
        private DependencyContainer _container = container;
        public void Run()
        {
            Console.WriteLine("Password Manager");

            bool flag = true;

            while(flag)
            {
                string actions = """ 
                "Choose action: 
                1. Add new data 
                2. Check all data 
                3. Close app
                """;
                Console.WriteLine(actions);
                string choice = Console.ReadLine();
                if (int.TryParse(choice, out int result))
                {
                    switch(result)
                    {
                        case 1:
                            CoreDataModel userData = new();
                            Console.WriteLine("Add Data");
                            Console.WriteLine("Write Website: ");
                            userData.Website = Console.ReadLine();
                            Console.WriteLine("Write Email: ");
                            userData.Email = Console.ReadLine();
                            Console.WriteLine("Write Password: ");
                            userData.Password = Console.ReadLine();
                            Console.WriteLine("Write Description: ");
                            userData.Description = Console.ReadLine();
                            ResponseMsg answer = UserActions.AddData(userData);
                            Console.WriteLine(answer.Message);
                            break;
                        case 2:
                            UserActions.GetData();
                            break;
                        case 3:
                            Console.WriteLine("Exit");
                            flag = false;
                            break;
                        default:
                            Console.WriteLine("Invalid number");
                            break;
                    }
                }
                else{Console.WriteLine("Invalid number");}
            }
        }
    }
}
