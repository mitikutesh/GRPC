using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Server;
using Grpc.Server.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grpc.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        public GrpcChannel channel;
        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
            channel = GrpcChannel.ForAddress("https://localhost:32789/");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = new MessagingInfo.MessagingInfoClient(channel);
            var request = new MessageInfoModel()
            {
               UserId = 2
            };
            var reply = await client.GetMessageByIdAsync(request);
            return Ok(reply);
            //return await Task.FromResult(Ok("Welcome to GRPC client"));
        }

        [HttpGet("messages")]
        public async IAsyncEnumerable<MessageModel> GetMessagesAsync()
        {
            var client = new MessagingInfo.MessagingInfoClient(channel);
            List<MessageModel> msgs = new List<MessageModel>();
            using (var call = client.GetNewMessages(new NewMessageRequest()))
            {
                while(await call.ResponseStream.MoveNext())
                {
                    var msg = call.ResponseStream.Current;
                    yield return msg;
                }
            }
            //return await Task.FromResult(Ok("Welcome to GRPC client"));
        }
        [HttpPost("Greeting")]
        public async Task<IActionResult> PostGreeting(string name)
        {
            var client = new Greeter.GreeterClient(channel);
            var request = new HelloRequest()
            {
                Name = name
            };
            var reply = await client.SayHelloAsync(request);
            return Ok(reply.Message);
        }
    }
}
