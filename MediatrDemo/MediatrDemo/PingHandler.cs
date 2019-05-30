using MediatrDemo;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Examples
{
       
    public class PingHandler : IRequestHandler<Ping, Pong>
    {
        private readonly TextWriter _writer;

        public PingHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public async Task<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            Thread.Sleep(1000);

            await _writer.WriteLineAsync($"--- Handled Ping: {request.Message}");
            return new Pong { Response = request.Message + " Pong" };
        }
    }
}