using PasswordManager.Core;
using PasswordManager.DataBase;

var container = new DependencyContainer("data.db");

new DataBaseInitializer(container.DbContext).Initialize();

new AppRunner(container).Run();
