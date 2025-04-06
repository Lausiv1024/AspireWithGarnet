var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddGarnet("cache")
    .WithDataVolume(isReadOnly: false);
var sqlServer = builder.AddSqlServer("sqlserver");
var sampleDb = sqlServer.AddDatabase("sampleDb");

var apiService = builder.AddProject<Projects.AspireWithGarnet_ApiService>("apiservice")
    .WithReference(cache);

builder.AddProject<Projects.AspireWithGarnet_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(sqlServer)
    .WithReference(cache)
    .WaitFor(apiService);

if (builder.ExecutionContext.IsRunMode)
{
    builder.AddProject<Projects.AspireWithGarnet_Web_DbInitializer>("aspirewithgarnet-web-dbinitializer")
        .WithReference(sampleDb);
}

builder.Build().Run();
