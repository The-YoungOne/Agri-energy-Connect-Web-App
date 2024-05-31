
AUTHOR:

Lucian Sandile Young
Student Number: ST10039287
Email: luyoung247@gmail.com
ALT email: st10039287@vcconnect.edu.za

----------------------------------------------------------------------------------------------------

INTRODUCTION:

This project is a web application called AgriConnect_MVC, designed to facilitate connections between farmers. 
Through this platform, farmers can showcase their products, and view the products of other farmers on the marketplace.

----------------------------------------------------------------------------------------------------

PURPOSE:

The purpose of AgriConnect_MVC is to create a convenient and efficient marketplace for agricultural products, eliminating middlemen and ensuring fair prices for farmers. 
By connecting farmers, the platform aims to promote sustainability, support local agriculture, and ensure farmers are providing high-quality, fresh produce.

----------------------------------------------------------------------------------------------------

BUILT USING:

Visual Studio 2022
- Asp.NET Core Web App (Model-View-Controller), code-first approach

----------------------------------------------------------------------------------------------------

SYSTEM REQUIREMENTS:

- Operating System: Windows, Linux, macOS
- Web Browser: Google Chrome, Mozilla Firefox, Safari, Microsoft Edge
- .NET Core SDK
- Visual Studio Code or Visual Studio 2022 IDE

----------------------------------------------------------------------------------------------------

SPECIFICATIONS BEFORE RUNNING:

Before running the application, ensure that:

- .NET Core SDK is installed on your system (Visual Studio 2022).

- Ensure NuGet packages are installed (Done automatically upon building the application).

- Change the connection string to your local database.
  Connection string template:
  "Server=labVMH8OX\\\\SQLEXPRESS;Database=AgriFarmer_Connect;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"

- Database migrations are applied using Entity Framework Core. 


----------------------------------------------------------------------------------------------------

STEPS BEFORE RUNNING:

Below is a list of steps that can be followed to get the application running correctly:

1.  Ensure Visual Studio 2022 is installed on your device.
2.  Download the zip folder of the code.
3.  Unzip the folder.
4.  Navigate into the folder until finding the "AgriConnect MVC.sln".
5.  Right-click it and select Open with -> Visual Studio 2022.
6.  Upon opening build the program or click CNTRL and B at the same time.
7.  Open the appsettings.json in the solution explorer.
8.  Change the DefaultConnection to your own for your local device (you may use the connection string template under SPECIFICATIONS BEFORE RUNNING).
9.  Open the Tools dropdown menu at the top of the window.
10. Select "Nuget Package Manager", followed by "package manager console".
11. Type "add-migration" followed by naming the migration (eg: add-migration FullMigrate) then click enter.
12. Next, type "update-database" and click enter

After following these steps, the application can be run. It will automatically populate the database with default data that can be tested with.

----------------------------------------------------------------------------------------------------

DEFAULT PROFILE CREDENTIALS:

For an Employee login:
- Email: employee@employee.com
- Password: Admin1234#

For a Farmer login:
- Email: john@doefarming.com
- Password: Farmer1234#

----------------------------------------------------------------------------------------------------

CORE FEATURES:

 User Authentication and Authorization: 
- Allows users to register, log in, and manage their profiles. Different roles (employee, farmer) have different access levels.

Employee Profile Management: 
- Employees can create, edit, and delete profiles like theirs. 
  Only an employee can create another employee profile within the system once logged in.

Farmer Profile Management: 
- Farmers can create their profiles, and wait for approval from an employee before gaining further access. 
  Upon gaining that access, farmers get access to the product features.

Product Management: 
- Farmers can add, edit, and delete their products, including details such as name, category, quantity, production date, and image.

Marketplace: 
- Farmers and employees can browse all products listed by farmers, filter products based on various criteria, and view the profile of the farmers who listed specific products.

----------------------------------------------------------------------------------------------------

FAQs:

How can I register as a farmer?
- Agri Farmer Connect offers the user the ability to register as a farmer via the registration page. 
  Select the "Become a Farmer" text on the top right next to "log in" and fill in all the required information. You must then await the approval of your farmer account.

Can farmers leave reviews for products?
- Currently, the platform does not support leaving reviews for products. However, this feature may be implemented in future updates.

Why can I not see the marketplace?
- The marketplace is a list of all the farmer's products made on the site. It is only available to approved farmers who have successfully logged into the system.

----------------------------------------------------------------------------------------------------

ACKNOWLEDGEMENTS:

I would like to acknowledge the support provided by the following platforms and individuals:

1. ChatGPT - For providing solutions to errors faced throughout development, as well as supportive design elements for the front end.

2. Manoj Deshwal (YouTube) - For aiding with the importing and modifications on the user identities.

3. tutorialsEU-C# (YoutTube) - Guiding the integration of user roles into the application, as well as creating permanent default user information.

----------------------------------------------------------------------------------------------------

REFERENCES:

Scrumstudy.com. (2017). Scrum Fundamentals Certified - SCRUM study. [online] Available at: https://www.scrumstudy.com/certification/scrum-fundamentals-certified [Accessed 18 Oct. 2019].

pubs.opengroup.org. (n.d.). The TOGAF® Standard, Version 9.2. [online] Available at: https://pubs.opengroup.org/architecture/togaf92-doc/arch/.

‌Bonham, A. (2020). Microservices — When to React Vs. Orchestrate. [online] Capital One Tech. Available at: https://medium.com/capital-one-tech/microservices-when-to-react-vs-orchestrate-c6b18308a14c.

‌‌IEnableMuch (n.d.). SQL Server Said, PostgreSQL Said. APPLY and LATERAL. [online] Available at: https://www.anicehumble.com/2013/09/sql-server-said-postgresql-said-apply-lateral.html [Accessed 31 May 2024].

----------------------------------------------------------------------------------------------------
