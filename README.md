# LotteryAPI

### Few basic rules:

For the admins:
1) Start session, without a session users can't fill out tickets,
2) The session can end in three ways:
    - by drawing the numbers,
    - by ending the session, in the "session" menu or
    - when it reaches its end date, which is seven days since it's started.
3) When the drawing is finished, the session is automatically closed. The players can check their "lucky" numbers after that.

Admin's account:
Username: "jdoe"
Password: "admin123"

For the users:

To be able to fill out a ticket, the user must be registered first.

### Instructions

1) Open the solution "Lottery.sln" in Visual Studio, 
2) Right Click on "WebAPI" in the Solution Explorer, and choose "Set as StartUp Project",
3) Then open "Package Manager Console", you can find it under "Tools > NuGet Package Manager > Package Manager Console",
4) When the "Package Manager Console" window opens, in "Default Project" dropdown menu select "Lottery.DataAccess"
5) In the "Package Manager Console" write "Add-Migration MigrationsName", you can choose any name you want instead of "MigrationsName".
6) When the migration finishes building, in the "Package Manager Console" window write "Update-Database".
7) When it finishes, you can run the api, by clicking on "IIS Express" or by clicking on F5 function key. When it finishes loading, in the browser window, you gonna see a message "Lottery API is active.".

After this you are ready to run the lottery application, either react-lottery, react-redux-lottery or vue-lottery.
