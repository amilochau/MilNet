[![Build Status](https://amilochau.visualstudio.com/GitHub/_apis/build/status/amilochau.MilNet?branchName=master)](https://amilochau.visualstudio.com/GitHub/_build/latest?definitionId=18&branchName=master)

# MilNet package project

MilNet is a set of .NET Core libraries. MilNet provides a ready-to-use template for your ASP.NET Core applications, with the following features:
- Open API (Swagger)
- Connection to basic services to send feedbacks, emails
- Application contacts information
- Application last release information
- Authentication/Authorization management with IdentityServer4
- Logging with ElasticSearch
- Privacy and Cookies management (RGPD)
- Basic healthchecks

Other utilities are included into MilNet:
- Exceptions for Forbidden and Not Found responses
- CSV-like files interpretation
- Service to create HTTP requests with .NET Core Http Request Factory

MilNet works well with a vue.js Front application, using MilNode packages.

Warning! Watch the [Obsolete] attributes into the source code, they indicate features that will be deleted on the next major version!

# Contribute

If you want to propose a feature or to report a bug, please create a new issue.
If you want to propose code source changes, please follow the steps below.

1. Installation process
   From your local computer, clone the repository.
   - `dotnet restore` to install all dependencies
   - `dotnet run` to build MilNet as a library

2. GitFlow
   Please follow the [GitHub Flow](https://guides.github.com/introduction/flow/):
   - Create a branch from `master`
   - Commit your code changes
   - Create a Pull Request to merge into the master `branch`

3. Conditions to complete Pull Request
   To ensure minimal quality, branch policies have been set up for pull requests to the master branch:
   - A new build must be validated before each pull request
   - A project administrator must validate the code changes

# Installation and Usage

1. Import MilNet in your ASP.NET Core project with `Import-Package MilNet.Services.AspNetCore`
2. Create your `IWebHostBuilder` with the `ConfigureMilNetServices` extension, as in the `Program.cs` file:
   ```C#
   public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
       WebHost.CreateDefaultBuilder(args)
           .ConfigureMilNetServices()
           .UseStartup<Startup>();
   ```
3. Configure your `IServiceCollection` with the `AddMilNetServices` extension, as in the `Startup.cs` file:
   ```C#
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddMilNetServices(configuration.GetSection("Services");
       
       services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
   }
   ```
4. Configuration your `IApplicationBuilder` with the `UseMilNetServices` extension, as in the `Startup.cs` file:
   ```C#
   public void Configure(IApplicationBuilder app, IHostingEnvironment env)
   {
       app.UseMilNetServices();
       
       app.UseMvcWithDefaultRoute();
   }
   ```
5. Configure MilNet in your `appsettings.json` file:
   ```JSON
   {
     "ConnectionStrings": {
       "DefaultConnection": "XXX"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Warning"
       }
     },
     "AllowedHosts": "*",
     "Services": {
       "ApplicationName": "MyAppName",
			 "Log": {
			     "ElasticSearchUrl": "http://localhost:9200"
			 },
       "Contact": {
         "Business": {
				   "Email": "business-support@mysite.com",
           "Place": "Paris, France",
           "Url": "http://mysite.com/business-support",
					 "Users": [
					     {
					         "FirstName": "Antoine",
							     "LastName": "Milochau"
					         "Email": "antoine.milochau@mysite.com",
							     "Place": "Paris, France"
							 }
					 ]
         },
         "Technical": {
				   "Email": "technical-support@mysite.com",
           "Place": "Paris, France",
           "Url": "http://mysite.com/technical-support",
					 "Users": []
         }
       },
       "Release": {
         "Definition": "Development",
         "Name": "No release"
       }
     }
   }
   ```

# Obsolete features

Watch the [Obsolete] attributes into the source code, they indicate features that will be deleted on the next major version.
