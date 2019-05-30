using Autofac;
using MediatR;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MediatrDemoRequestHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            IMediator mediator = BuildMediator();
            mediator.Send(new Ping() { Message = "Test 12345" });
        }

        public static IMediator BuildMediator()
        {
            // Create your builder.
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Ping).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();            

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();
            return container.Resolve<IMediator>();
        }
    }
}
