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
    public class RequestHandler : IDisposable
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
            string username;
            if (Authorize(out username))
            {
                while (true)
                {
                    Request request = this.Formatter.Deserialize(this.SocketStream) as Request;

                    Response response = new Response();
                    try
                    {
                        switch (request.RequestType)
                        {
                            case RequestType.Logout:
                                {
                                    response.Message = "Successfully logged out.";
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
                            case RequestType.Terminate:
                                {
                                    response.Message = "Successfully terminated.";
                                    break;
                                }
                            default:
                                {
                                    throw new InvalidOperationException(
                                        string.Format("Operation {0} is not supported.", request.RequestType));
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

                    if (request.RequestType == RequestType.Logout ||
                        request.RequestType == RequestType.Terminate)
                    {
                        break;
                    }
                }
            }

            this.Dispose();
        }

        public bool Authorize(out string username)
        {
            username = null;
            while (true)
            {
                Request request = this.Formatter.Deserialize(this.SocketStream) as Request;

                Response response = new Response();
                try
                {
                    switch (request.RequestType)
                    {
                        case RequestType.Login:
                            {
                                var login = request.Data as Login;
                                this.UsersService.Login(login);
                                username = login.Username;
                                response.Message = "Successfully logged!";
                                break;
                            }
                        case RequestType.Register:
                            {
                                var reg = request.Data as Register;
                                this.UsersService.Register(reg);
                                username = reg.Username;
                                response.Message = "Successfully registered!";
                                break;
                            }
                        case RequestType.Terminate:
                            {
                                return false;
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

                if (username != null)
                {
                    break;
                }
            }

            return true;
        }

        public void Dispose()
        {
            this.Reader.Dispose();
            this.Writer.Dispose();
            this.SocketStream.Dispose();
            this.RequestSocket.Dispose();
        }
    }
}
