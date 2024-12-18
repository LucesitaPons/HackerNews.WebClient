# HackerNews.WebClient
Hacker News Web Client for beststories

The Hacker New Client is already published in the following URL just in case it's not necessary to publish the code: 
http://lucesitapons.somee.com/Web.Site

##INSTALL IIS IN WINDOWS
1. Open control panel
2. Go to Turn Windows features on or off.
3. Find a check box for Internet Information Services.
4. Check the boxes for any additional features needed such as ASP.NET and .NET Extensibility
5. Click OK for install.
6. Test the installation by opening the web browser and type localhost in the address bar. If everything is ok, the browser will show a welcome page.

#INSTALL THE HACKER NEWS WEB CLIENT
¡Claro! Aquí tienes las instrucciones en inglés para publicar un sitio web ASP.NET Core 8 en IIS:

### Requirements
1. **Install the .NET Core Hosting Bundle**:
   - Download the latest installer from the official Microsoft website.
   - Run the installer on your server.
   - Restart the server or run `net stop was /y` followed by `net start w3svc` in a command prompt.

2. **Create an IIS Site**:
   - Open IIS Manager.
   - Create a new site and set the physical path to the folder where your published files will be located.

3. **Publish Your ASP.NET Core App**:
   - Open your ASP.NET Core project in Visual Studio.
   - Right-click on the project and select "Publish" or use the `dotnet publish` command in the terminal.
   - Choose the publish target as "IIS" and configure the settings as needed.

4. **Configure the Application Pool**:
   - Ensure the application pool is set to the correct .NET CLR version and enable 32-bit applications if necessary.
   - Set the application pool to run under a specific identity if required.

5. **Enable In-Process Hosting**:
   - Add the following code to your `Program.cs` file to enable in-process hosting:

   ```csharp
   builder.Services.Configure<IISServerOptions>(options =>
   {
       options.AutomaticAuthentication = false;
   });
   ```

6. **Deploy the Application**:
   - Copy the published files to the IIS site folder.
   - Ensure the `web.config` file is correctly configured.
   - Optionally, place an `app_offline.htm` file in the root of your application to temporarily take the site offline during deployment.

7. **Verify the Deployment**:
   - Open your browser and navigate to the site URL to ensure everything is working correctly.

These steps should help you successfully publish your ASP.NET Core 8 application on IIS. If you encounter any issues, feel free to ask for more assistance!
