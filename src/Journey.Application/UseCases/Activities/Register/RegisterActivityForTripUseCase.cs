﻿using FluentValidation.Results;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Entities;

namespace Journey.Application.UseCases.Activities.Register;

public class RegisterActivityForTripUseCase
{
    public ResponseActivityJson Execute(Guid tripId, RequestRegisterActivityJson request)
    {
        var dbContext = new JourneyDbContext();

        var trip = dbContext
            .Trips
            .FirstOrDefault(trip => trip.Id == tripId);

        Validate(trip, request);

        var activity = new Activity
        {
            TripId = tripId,
            Name = request.Name,
            Date = request.Date
        };

        dbContext.Activities.Add(activity);
        dbContext.SaveChanges();

        return new ResponseActivityJson
        {
            Id = activity.Id,
            Name = activity.Name,
            Date = activity.Date,
            Status = (Communication.Enums.ActivityStatus)activity.Status
        };
    }

    private void Validate(Trip? trip, RequestRegisterActivityJson request)
    {
        if (trip is null)
            throw new NotFoundException(ResourceErrorMessages.TRIP_NOT_FOUND);

        var validator = new RegisterActivityValidator();

        var result = validator.Validate(request);

        if (!(request.Date >= trip.StartDate && request.Date <= trip.EndDate))
        {
            result.Errors.Add(new ValidationFailure("Date", ResourceErrorMessages.DATE_NOT_WITHIN_TRAVEL_PERIOD));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage)
                .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
