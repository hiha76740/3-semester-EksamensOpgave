var builder = DistributedApplication.CreateBuilder(args);


var authDatabaseSqlServer = builder.AddSqlServer("authSqlServer")
    .WithContainerName("vidensvejen-auth-sqlserver-dev")
    .WithDataVolume("vidensvejen-auth-data-dev")
    .WithLifetime(ContainerLifetime.Persistent);

var authDb = authDatabaseSqlServer.AddDatabase("AuthUserDb");

var authService = builder.AddProject<Projects.AuthService_Api>("authservice-api")
    .WithDaprSidecar()
    .WithReference(authDb)
    .WaitFor(authDb);


var exhibitionDatabaseSqlServer = builder.AddSqlServer("exhibitionSqlServer")
    .WithContainerName("vidensvejen-exhibition-sqlserver-dev")
    .WithDataVolume("vidensvejen-exhibition-data-dev")
    .WithLifetime(ContainerLifetime.Persistent);

var exhibitionDb = exhibitionDatabaseSqlServer.AddDatabase("ExhibitionDb");

var exhibitionService = builder.AddProject<Projects.ExhibitionService_Api>("exhibitionservice-api")
    .WithDaprSidecar()
    .WithReference(exhibitionDb)
    .WaitFor(exhibitionDb);


var gameDatabaseSqlServer = builder.AddSqlServer("gameSqlServer")
    .WithContainerName("vidensvejen-game-sqlserver-dev")
    .WithDataVolume("vidensvejen-game-data-dev")
    .WithLifetime(ContainerLifetime.Persistent);

var gameDb = gameDatabaseSqlServer.AddDatabase("GameDb");

var gameService = builder.AddProject<Projects.GameService_Api>("gameservice-api")
    .WithDaprSidecar()
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


var statisticsService = builder.AddProject<Projects.StatisticsService_Api>("statisticsservice-api")
    .WithDaprSidecar();


var userDatabaseSqlServer = builder.AddSqlServer("userSqlServer")
    .WithContainerName("vidensvejen-user-sqlserver-dev")
    .WithDataVolume("vidensvejen-user-data-dev")
    .WithLifetime(ContainerLifetime.Persistent);

var userDb = userDatabaseSqlServer.AddDatabase("UserDb");

var userService = builder.AddProject<Projects.UserService_Api>("userservice-api")
    .WithDaprSidecar()
    .WithReference(userDb)
    .WaitFor(userDb);


var proxy = builder.AddProject<Projects.VidensVejen_Yarp>("vidensvejen-yarp")
    .WithReference(authService)
    .WithReference(imageService)
    .WithReference(userService)
    .WithReference(exhibitionService)
    .WithReference(gameService)
    .WithReference(statisticsService)
    .WaitFor(authService)
    .WaitFor(imageService)
    .WaitFor(userService)
    .WaitFor(exhibitionService)
    .WaitFor(gameService)
    .WaitFor(statisticsService);


var kioskFrontend = builder.AddProject<Projects.VidensVejen_Kiosk_Web>("Kiosk-Frontend")
    .WithHttpsEndpoint(port: 7296)
    .WithReference(proxy)
    .WaitFor(proxy);


var adminFrontend = builder.AddProject<Projects.VidensVejen_Web>("Admin-Frontend")
    .WithHttpsEndpoint(port: 7237)
    .WithReference(proxy)
    .WaitFor(proxy);


builder.Build().Run();
