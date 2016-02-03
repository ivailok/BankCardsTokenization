using BCT.Data;
using BCT.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCT.Server
{
    public class RequestHandler
    {
        private readonly Socket RequestSocket;
        private readonly NetworkStream SocketStream;
        private readonly BinaryReader Reader;
        private readonly BinaryWriter Writer;
        private readonly BinaryFormatter Formatter;
        private readonly UsersService UsersService;

        public RequestHandler(Socket requestSocket, UsersService usersService)
        {
            this.RequestSocket = requestSocket;
            this.SocketStream = new NetworkStream(this.RequestSocket);
            this.Reader = new BinaryReader(this.SocketStream);
            this.Writer = new BinaryWriter(this.SocketStream);
            this.Formatter = new BinaryFormatter();
            this.UsersService = usersService;
        }

        public void Execute()
        {
            while (true)
            {
                while (this.RequestSocket.Available == 0)
                {
                    Thread.Sleep(1000);
                }

                Request package = this.Formatter.Deserialize(this.SocketStream) as Request;

                Response response = new Response();
                try
                {
                    switch (package.RequestType)
                    {
                        case RequestType.Login:
                            {
                                this.UsersService.Login((Login)package.Data);
                                response.Message = "Successfully logged!";
                                break;
                            }
                        case RequestType.Register:
                            {
                                break;
                            }
                        case RequestType.RegisterToken:
                            {
                                break;
                            }
                        case RequestType.GetCardNumber:
                            {
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    response.ResponseType = ResponseType.Success;
                }
                catch (Exception e)
                {
                    response.ResponseType = ResponseType.Error;
                    response.Message = e.Message;
                }
                
                this.Formatter.Serialize(this.SocketStream, response);
            }
        }
    }
}
