using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediatrDemoRequestHandler
{
    public class PingHandler : IRequestHandler<Ping, Pong>
    {
        public async Task<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("handled async ping: " + request.Message);
            
            return new Pong { Response = request.Message + " Pong" };
        }
    }
}