var builder = DistributedApplication.CreateBuilder(args);

var proxy = builder.AddProject<Projects.VidensVejen_Yarp>("vidensvejen-yarp");

var authDatabaseSqlServer = builder.AddSqlServer("authSqlServer")
    .WithContainerName("vidensvejen-auth-sqlserver-dev")
    .WithDataVolume("vidensvejen-auth-data-dev")
    .WithLifetime(ContainerLifetime.Persistent);

var authDb = authDatabaseSqlServer.AddDatabase("AuthUserDb");

var authService = builder.AddProject<Projects.AuthService_Api>("authservice-api")
    .WithDaprSidecar()
    .WithReference(authDb)
    .WaitFor(authDb);

var exhibitionService = builder.AddProject<Projects.ExhibitionService_Api>("exhibitionservice-api");

var gameDatabaseSqlServer = builder.AddSqlServer("gameSqlServer")
    .WithContainerName("vidensvejen-game-sqlserver-dev")
    .WithDataVolume("vidensvejen-game-data-dev")
    .WithLifetime(ContainerLifetime.Persistent);

var gameDb = gameDatabaseSqlServer.AddDatabase("GameDb");

var gameService = builder.AddProject<Projects.GameService_Api>("gameservice-api")
    .WithReference(gameDb)
    .WaitFor(gameDb);

var imageDatabaseSqlServier = builder.AddSqlServer("imageSqlServer")
    .WithContainerName("vidensvejen-image-sqlserver-dev")
    .WithDataVolume("vidensvejen-image-data-dev")
    .WithLifetime(ContainerLifetime.Persistent);

var imageDb = imageDatabaseSqlServier.AddDatabase("ImageDb");

var imageService = builder.AddProject<Projects.ImageService_Api>("imageservice-api")
    .WithDaprSidecar()
    .WithReference(imageDb)
    .WaitFor(imageDb);

var statisticsService = builder.AddProject<Projects.StatisticsService_Api>("statisticsservice-api");

var userDatabaseSqlServer = builder.AddSqlServer("userSqlServer")
    .WithContainerName("vidensvejen-user-sqlserver-dev")
    .WithDataVolume("vidensvejen-user-data-dev")
    .WithLifetime(ContainerLifetime.Persistent);

var userDb = userDatabaseSqlServer.AddDatabase("UserDb");

var userService = builder.AddProject<Projects.UserService_Api>("userservice-api")
    .WithDaprSidecar()
    .WithReference(userDb)
    .WaitFor(userDb);

var kioskFrontend = builder.AddProject<Projects.VidensVejen_Kiosk_Web>("Kiosk-Frontend");

var adminFrontend = builder.AddProject<Projects.VidensVejen_Web>("Admin-Frontend");

builder.Build().Run();
