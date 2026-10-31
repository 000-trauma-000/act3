using ConstellationRestoreApi.Dto;

namespace ConstellationRestoreApi.Services;

public sealed class ConstellationRestoreService : IConstellationRestoreService
{
    public ConstellationRestoreResponse Restore(ConstellationRestoreRequest request)
    {
        var missing = new List<char>();

        foreach (var referenceChar in request.Reference)
        {
            bool found = false;

            foreach (var damagedChar in request.Damaged)
            {
                if (referenceChar == damagedChar)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                missing.Add(referenceChar);
            }
        }

        missing.Sort();

        var missingSymbols = new string(missing.ToArray());

        return new ConstellationRestoreResponse
        {
            MissingSymbols = missingSymbols,
            IsComplete = missingSymbols.Length == 0
        };
    }
}