using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatrDemoRequestHandler
{
    public class Ping : IRequest<Pong>
    {
        public string Message { get; set; }

        public Ping()
        {
        }
    }
}
