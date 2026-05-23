using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;
using PasswordManager.Core;
using PasswordManager.Services;
using PasswordManager.Models.DTO;

namespace PasswordManager.Core
{
    public class AppRunner(DependencyContainer container)
    {
        private readonly IVaultService _vaultService = container.VaultService;
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
                3. Update Data
                4. Close app
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
                            ResponseMsg answer = _vaultService.AddEntry(userData);
                            Console.WriteLine(answer.Message);
                            break;
                        case 2:
                            List<CoreDataModel> data = _vaultService.GetAllEntries();
                            foreach (var d in data)
                            {
                                Console.WriteLine($"Id:{d.Id}, Website: {d.Website}, Email: {d.Email}, Password: {d.Password}, Description: {d.Description}");
                            }
                            break;
                        case 3:
                            Console.WriteLine("Write id of the desired data to update");
                            string sid = Console.ReadLine();
                            if(int.TryParse(sid, out int id))
                            {
                                UpdateDto updateData = new();
                                Console.WriteLine("Add Data");
                                Console.WriteLine("Write Website: ");
                                updateData.Website = Console.ReadLine();
                                Console.WriteLine("Write Email: ");
                                updateData.Email = Console.ReadLine();
                                Console.WriteLine("Write Password: ");
                                updateData.Password = Console.ReadLine();
                                Console.WriteLine("Write Description: ");
                                updateData.Description = Console.ReadLine();
                                ResponseMsg answer1 = _vaultService.UpdateEntry(id, updateData);
                                Console.WriteLine(answer1.Message);
                            }
                            break;
                        case 4:
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
