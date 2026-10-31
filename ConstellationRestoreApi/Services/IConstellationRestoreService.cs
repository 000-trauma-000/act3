using ConstellationRestoreApi.Dto;

namespace ConstellationRestoreApi.Services;

public interface IConstellationRestoreService
{
    ConstellationRestoreResponse Restore(ConstellationRestoreRequest request);
}