using System;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using MediatR.Examples;
using System.IO;

namespace MediatrDemo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IMediator mediator = BuildMediator();
            await Run(mediator);                     
        }

        public static IMediator BuildMediator()
        {
            // Create your builder.
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(Ping).GetTypeInfo().Assembly)
                    .AsImplementedInterfaces();
            }

            WrappingWriter output = new WrappingWriter(Console.Out);
            builder.RegisterInstance(output).As<TextWriter>();

            // It appears Autofac returns the last registered types first
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(GenericRequestPreProcessor<>)).As(typeof(IRequestPreProcessor<>));
            builder.RegisterGeneric(typeof(GenericRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));
            builder.RegisterGeneric(typeof(GenericPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ConstrainedRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));


            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();

            return container.Resolve<IMediator>();
        }

        public static async Task Run(IMediator mediator)
        {
            var pong = await mediator.Send(new Ping() { Message = "Pingggg" });

            Console.WriteLine("PONG: " + pong.Response);
        }
    }
}
