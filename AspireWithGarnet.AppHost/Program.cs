var builder = DistributedApplication.CreateBuilder(args);

//Garnet�̃T�[�r�X��ǉ�
var cache = builder.AddGarnet("cache")
    .WithDataVolume(isReadOnly: false);

var apiService = builder.AddProject<Projects.AspireWithGarnet_ApiService>("apiservice")
    .WithReference(cache);

builder.AddProject<Projects.AspireWithGarnet_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(cache)//Garnet�̎Q�Ƃ��v���W�F�N�g�ɒǉ�
    .WaitFor(apiService);

builder.Build().Run();
