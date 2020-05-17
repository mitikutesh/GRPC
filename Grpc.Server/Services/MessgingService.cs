using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Server.Protos;
using Microsoft.Extensions.Logging;

namespace Grpc.Server
{
    public class MessgingService : MessagingInfo.MessagingInfoBase
    {
        private readonly ILogger<GreeterService> _logger;
        public MessgingService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<MessageModel> GetMessageById(MessageInfoModel request, ServerCallContext context)
        {
            MessageModel messagem = new MessageModel();
            if(request.UserId == 1)
            {
                messagem.FirstName = "Mitiku";
                messagem.LastName = "Teshome";
            }
            else if (request.UserId == 2)
            {
                messagem.FirstName = "MT";
                messagem.LastName = "TE";

            }
            else if(request.UserId == 2)
            {
                messagem.FirstName = "BTT";
                messagem.LastName = "TTT";
            }
            else if(request.UserId == 3)
            {
                messagem.FirstName = "NS";
                messagem.LastName = "afd";
            }
            else if(request.UserId == 4)
            {
                messagem.FirstName = "afdaff";
                messagem.LastName = "afdafdaf";
            }
            else
            {
                messagem.FirstName = "unknown";
                messagem.LastName = "unknown";
            }
            return Task.FromResult(messagem);
        }

        public override async Task GetNewMessages(NewMessageRequest request, 
            IServerStreamWriter<MessageModel> responseStream, 
            ServerCallContext context)
        {
            List<MessageModel> messageModels = new List<MessageModel>
            {
                new MessageModel
                {
                    Age = 3,
                    FirstName = "Mitiku",
                    LastName = "Ok"
                },
                new MessageModel
                {
                    Age = 4,
                    FirstName = "safsdaf",
                    LastName = "afdaf"
                },
                new MessageModel
                {
                    Age = 4,
                    FirstName = "fd",
                    LastName = "asdfb"
                },
                new MessageModel
                {
                    Age = 5,
                    FirstName = "wertw",
                    LastName = "afdaf"
                }
            };

            foreach (var msg in messageModels)
            {
                await Task.Delay(10000);
                await responseStream.WriteAsync(msg);
                
            }
        }
    }
}
