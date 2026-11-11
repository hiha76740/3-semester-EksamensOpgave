var builder = DistributedApplication.CreateBuilder(args);

var proxy = builder.AddProject<Projects.VidensVejen_Yarp>("vidensvejen-yarp");

var authService = builder.AddProject<Projects.AuthService_Api>("authservice-api");

var exhibitionService = builder.AddProject<Projects.ExhibitionService_Api>("exhibitionservice-api");

var gameService = builder.AddProject<Projects.GameService_Api>("gameservice-api");

var imageService = builder.AddProject<Projects.ImageService_Api>("imageservice-api");

var statisticsService = builder.AddProject<Projects.StatisticsService_Api>("statisticsservice-api");

var userService = builder.AddProject<Projects.UserService_Api>("userservice-api");

var kioskFrontend = builder.AddProject<Projects.VidensVejen_Kiosk_Web>("Kiosk-Frontend");

var adminFrontend = builder.AddProject<Projects.VidensVejen_Web>("Admin-Frontend");

builder.Build().Run();
