using Autofac;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Reflection;
using Ticket.Application.Behaviors;
using VacationRental.Application.CommandHandlers;
using VacationRental.Application.Commands;
using VacationRental.Application.Validations;
using VacationRental.Domain.AggregatesModel;

namespace VacationRental.Application.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(RentalBindingModel).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));


            // Register the Command's Validators (Validators based on FluentValidation library)
            builder
                .RegisterAssemblyTypes(typeof(CreateRentalCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(ValidatorRentalBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}


