var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddGarnet("cache")
    .WithDataVolume(isReadOnly: false);

var apiService = builder.AddProject<Projects.AspireWithGarnet_ApiService>("apiservice")
    .WithReference(cache);

builder.AddProject<Projects.AspireWithGarnet_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(cache)
    .WaitFor(apiService);

builder.Build().Run();
