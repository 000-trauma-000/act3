using ConstellationRestoreApi.Dto;
using System.Collections.Generic;

namespace ConstellationRestoreApi.Validation;

public static class RestoreRequestValidator
{
    public static IEnumerable<string> Validate(ConstellationRestoreRequest request)
    {
        var errors = new List<string>();

        if (request is null)
        {
            errors.Add("Запрос не должен быть пустым.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(request.Reference))
        {
            errors.Add("Поле reference обязательно.");
        }
        else
        {
            foreach (var c in request.Reference)
            {
                if (c < 'A' || c > 'Z')
                {
                    errors.Add("Поле reference должно содержать только латинские буквы от A до Z.");
                    break;
                }
            }

            var seen = new HashSet<char>();
            foreach (var c in request.Reference)
            {
                if (!seen.Add(c))
                {
                    errors.Add("Поле reference должно содержать уникальные символы.");
                    break;
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Reference))
        {
            foreach (var damagedChar in request.Damaged)
            {
                bool inReference = false;

                foreach (var referenceChar in request.Reference)
                {
                    if (damagedChar == referenceChar)
                    {
                        inReference = true;
                        break;
                    }
                }

                if (!inReference)
                {
                    errors.Add("Поле damaged содержит символы, отсутствующие в reference.");
                    break;
                }
            }
        }

        return errors;
    }
}