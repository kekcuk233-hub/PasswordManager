using PasswordManager.Core;

var container = new DependencyContainer("data.db", "meta");

new AppRunner(container).Run();
